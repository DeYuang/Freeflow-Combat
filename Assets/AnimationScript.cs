using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

	public GameObject 		target 				= null;
	public AnimationClip 	animationClip 		= null;
	public Animation 		animationOverride 	= null;
	public float 			distance 			= 1F;

	void Start(){

		if(animationOverride == null)
			animationOverride = GetComponent<Animation>();
		animationOverride.clip = animationClip;
	}

	void Update () {
	
		if(target == null)
			return;

		if(Vector3.Distance(transform.position, target.transform.position) > distance)
			animationOverride[animationClip.name].time = animationOverride.clip.length;
		else if(Vector3.Distance(transform.position, target.transform.position) == 0F)
			animationOverride[animationClip.name].time = 0F;
		else
			animationOverride[animationClip.name].time = Mathf.Lerp(0F, animationOverride.clip.length, Mathf.Lerp(0F, 1F, Vector3.Distance(transform.position, target.transform.position) / distance));
	}
}
