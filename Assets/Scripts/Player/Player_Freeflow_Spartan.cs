using UnityEngine;
using System.Collections;

public class Player_Freeflow_Spartan : MonoBehaviour {

	public Animation anim = null;
	public AnimationClip idle = null;
	public AnimationClip move = null;
	public AnimationClip run = null;
	public AnimationClip[] attacks;
	public AnimationClip block = null;
	public float blockDistance = 5F;

	public GameObject hurtbox = null;
	public GameObject shieldHurtbox = null;

	static public int state = 0; /* global player state
	0 == idle
	1 == attacking
	2 == blocking
	3 == freeflow                               */

	private Vector3 faceRotation = Vector3.zero;

	static public float critResponseTime = 0.2F;
	static public float critTimer = 0F;

	void Update () {

		if(anim.isPlaying == false){
			if(state == 1){
				//players attack missed completely
				state = 0;
				anim.clip = idle;
				anim.Play();
				hurtbox.SetActive(false);
				shieldHurtbox.SetActive(false);
				Player_Core.combo = 0;
			}
			if(state == 2){
				// block is done
				state = 0;
				anim.clip = idle;
				anim.Play();
				shieldHurtbox.SetActive(false);
			}
			if(state == 3){
				// attack did hit enemies
				state = 0;
				hurtbox.SetActive(false);
				shieldHurtbox.SetActive(false);
				anim.clip = idle;
				anim.Play();
			}
		}

		if(state == 0 || state == 3){
			if(state == 3 && critTimer < 0F)
				return;
			else if(state == 3)
				critTimer -= Time.deltaTime;
			if(Input.GetButtonDown("Fire1")){
				state = 1;
				anim.Rewind();
				anim.clip = attacks[Random.Range(0, attacks.Length -1)];
				anim.Play ();
				hurtbox.SetActive(true);

				transform.eulerAngles = new Vector3(0F, Camera.main.transform.eulerAngles.y, 0F);
			}
			else if(Input.GetButtonDown("Fire2")){
				if(Player_Freeflow_Controller.attackers.Count > 0){
					foreach(GameObject enemy in Player_Freeflow_Controller.attackers){
						if(Vector3.Distance(transform.position, enemy.transform.position) < blockDistance){
							//succesful block
							state = 2;
							Player_Core.combo ++;
							anim.CrossFade(block.name);

							transform.LookAt(enemy.transform);
							transform.eulerAngles = new Vector3(0F, transform.eulerAngles.y, 0F);

							enemy.GetComponent<Enemy_Combat_Melee>().attacking = false;
							Player_Freeflow_Controller.attackers.Remove(enemy);
							//enemy.GetComponent<Enemy_Combat_Melee>().takeDamage(transform.position);
							shieldHurtbox.SetActive(true);
							break; // this is so the player has to block for every enemy that is attacking
						}
					}
				}
			}
			else if(Input.GetAxis("Vertical") != 0F || Input.GetAxis("Horizontal") != 0F){

				// turn to face
				faceRotation = Camera.main.transform.eulerAngles;
				if(Input.GetAxis("Vertical") < 0F){
					faceRotation = new Vector3(0F, faceRotation.y - 180F, 0F);
				}
				if(Input.GetAxis("Horizontal") != 0F){
					if(Input.GetAxis("Vertical") < 0F)
						faceRotation = new Vector3(0F, faceRotation.y + (-90F * Input.GetAxis("Horizontal")), 0F);
					else
						faceRotation = new Vector3(0F, faceRotation.y + (90F * Input.GetAxis("Horizontal")), 0F);
				}
				faceRotation = new Vector3(0F, faceRotation.y, 0F);

				// rotate towards the rotation calculated above
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(faceRotation), Time.deltaTime * 250F);

				if(Input.GetButton ("Run")){
					transform.parent.Translate (transform.forward * Time.deltaTime * 4F);
					anim.CrossFade(run.name);
				}
				else{
					transform.parent.Translate (transform.forward * Time.deltaTime);
					anim.CrossFade(move.name);
				}
			}
		}
	}
}
