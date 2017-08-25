using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Freeflow_Controller : MonoBehaviour {

	static public List<GameObject> enemies = new List<GameObject>();
	static public List<GameObject> attackers = new List<GameObject>();
	static public int attackType = -1;/* the type of attack
	-1 = no attack
	0 = melee
	1 = sword/knive
	2 = unblockable
	*/
	
	void Start () {
	
		attackers.Clear();
	}

	void Update () {
	
		if(enemies.Count > 0){
			if(attackers.Count == 0 && Player_Freeflow_Spartan.state != 2 && Player_Freeflow_Spartan.state != 3){
				attackers.Clear();

				// Temp
				attackers.Add(enemies[Random.Range(0, enemies.Count)]);
				attackType = 0;

				// not temp
				foreach(GameObject enemy in attackers){
					enemy.GetComponent<Enemy_Combat_Melee>().attacking = true;
				}
			}
		}
		else{
			// player has won combat
		}
	}
}
