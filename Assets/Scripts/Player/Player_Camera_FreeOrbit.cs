using UnityEngine;
using System.Collections;

public class Player_Camera_FreeOrbit : MonoBehaviour {

	private float distance = 0F;
	public GameObject target = null;

	public float rotateSpeedX = 4F;
	public float rotateSpeedY = 2F;

	public float cameraSmoothSpeed = 5F;
	private Vector3 cameraSmoothPos = Vector3.zero;

	void Start () {
	
		distance = Vector3.Distance(transform.position, target.transform.position);
		cameraSmoothPos = target.transform.position;
	}

	void LateUpdate () {
	
		cameraSmoothPos = Vector3.Lerp (cameraSmoothPos, target.transform.position, cameraSmoothSpeed * Time.deltaTime);
		transform.position = cameraSmoothPos;

		if(Input.GetAxis ("Mouse X") != 0F || Input.GetAxis ("Mouse Y") != 0F) // Mouse input and controller input
			transform.eulerAngles = new Vector3(transform.eulerAngles.x + -Input.GetAxis ("Mouse Y") * rotateSpeedY, transform.eulerAngles.y + Input.GetAxis ("Mouse X") * rotateSpeedX, 0F);
		transform.Translate(Vector3.back * distance);
	}
}
