using UnityEngine;
using System.Collections;

public class Player_Freeflow_Hurtbox : MonoBehaviour {

	void OnTriggerEnter(Collider other){

		if(other.tag == "Enemy"){
			//hit enemy
			other.GetComponent<Enemy_Combat_Melee>().takeDamage(Player_Core.player.transform.position);
			if(other.GetComponent<Enemy_Combat_Melee>().attacking == true){
				other.GetComponent<Enemy_Combat_Melee>().attacking = false;
				Player_Freeflow_Controller.attackers.Remove(other.gameObject);
			}

			if(Player_Freeflow_Spartan.state == 1){
				Player_Core.combo ++;
				Player_Freeflow_Spartan.state = 3;
			}

			Player_Freeflow_Spartan.critTimer = Player_Freeflow_Spartan.critResponseTime;
		}
	}
}
