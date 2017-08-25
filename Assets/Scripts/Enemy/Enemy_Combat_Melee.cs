using UnityEngine;
using System.Collections;

public class Enemy_Combat_Melee : MonoBehaviour {

	public GameObject counterQue = null;
	public bool attacking = false;
	public float maxRange = 4F;
	public float idleRange = 8F;
	public Animation anim = null;
	public AnimationClip idle = null;
	public AnimationClip run = null;
	public AnimationClip[] attacks;
	public AnimationClip hit = null;
	public float moveSpeed = 4F;

	public float counterAlpha = 0F;
	
	void Start () {
	
		counterQue.SetActive(false);
		Player_Freeflow_Controller.enemies.Add(gameObject);
	}

	void Update () {
	
		transform.LookAt(Player_Core.player.transform);
		transform.eulerAngles = new Vector3(0F, transform.eulerAngles.y, 0F);

		if(Vector3.Distance(Player_Core.player.transform.position, transform.position) > idleRange){
			anim.CrossFade(run.name);
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

			if(attacking){
				attacking = false;
				Player_Freeflow_Controller.attackers.Remove(gameObject);
			}
			// no need to do the rest of the script, the enemy is way too far away anyway
			// see it as script lodding, if you like
			return;
		}

		if(attacking){
			if(Vector3.Distance(Player_Core.player.transform.position, transform.position) < maxRange){
				counterQue.SetActive (true);
				counterAlpha = Mathf.Lerp (counterAlpha, 1F, Time.deltaTime * 10F);
				counterQue.GetComponent<Renderer>().material.color = new Color(counterQue.GetComponent<Renderer>().material.color.r, counterQue.GetComponent<Renderer>().material.color.g, counterQue.GetComponent<Renderer>().material.color.b, counterAlpha);
				// attack anim

				// temp
				anim.CrossFade(idle.name);

				// when done with attack
				//Player_Freeflow_Controller.attackers.Remove(gameObject);
				//attacking = false;
			}
			else{
				anim.CrossFade(run.name);
				transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
			}
			return;
		}
		else if(counterQue.activeSelf){
			counterAlpha = 0F;
			counterQue.SetActive (false);
		}

		if(anim.clip != hit)
			anim.CrossFade(idle.name);
		else if(anim.isPlaying == false){
			anim.clip = idle;
			anim.Play();
		}
	}

	public void takeDamage(Vector3 origin){

		transform.LookAt(origin);
		transform.eulerAngles = new Vector3(0F, transform.eulerAngles.y, 0F);

		anim[hit.name].speed = 0.5f;
		anim.clip = hit;
		anim.Play();
	}
}
