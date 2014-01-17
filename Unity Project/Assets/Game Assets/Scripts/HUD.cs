using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUI.Box (new Rect (0, Screen.height - 20, Screen.width, 20),"");
		GUI.Box (new Rect (0,0, Screen.width, 50),"");

	}
}
