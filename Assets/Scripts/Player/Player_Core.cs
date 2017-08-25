using UnityEngine;
using System.Collections;

public class Player_Core : MonoBehaviour {

	public GameObject Player = null;
	static public GameObject player = null;
	public float life = 100F;
	static public int combo = 0;	

	void Awake(){

		player = Player;
	}
}
