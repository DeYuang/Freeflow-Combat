using UnityEngine;
using System.Collections;

public class GUI_ComboMeter : MonoBehaviour {

	public Color normalColor = Color.white;
	public Color specialColor = Color.yellow;
	public Color focusColor = Color.red;

	void Update () {
	
		if(Player_Core.combo > 0)
			GetComponent<GUIText>().text = Player_Core.combo + "x";
		else
			GetComponent<GUIText>().text = null;

		GetComponent<GUIText>().color = normalColor;
		if(Player_Core.combo > 4)
			GetComponent<GUIText>().color = specialColor;
		if(Player_Core.combo > 7)
			GetComponent<GUIText>().color = focusColor;
	}
}
