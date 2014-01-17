using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {
	
	public List<GameObject> unitsInGame;
	public List<GameObject> unitsOnScreen;

	// Use this for initialization
	void Start () {
		foreach (Transform child in GameObject.Find("Units").transform) {
			unitsInGame.Add(child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
