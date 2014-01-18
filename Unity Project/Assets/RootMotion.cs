using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class RootMotion : MonoBehaviour {
	
	void OnAnimatorMove()
	{
		Animator animator = GetComponent<Animator>(); 
		
		if (animator)
		{
			Vector3 newPosition = transform.position;
			newPosition.z += 1.6f * Time.deltaTime;                                 
			transform.position = newPosition;
		}
	}
}