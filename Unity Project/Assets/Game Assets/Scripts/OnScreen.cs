using UnityEngine;
using System.Collections;

public class OnScreen : MonoBehaviour {

	private UnitManager manager;
	private GameObject unit;

	// Use this for initialization
	void Start () {
		unit = this.gameObject.transform.parent.gameObject;
		manager = GameObject.Find ("Game Manager").GetComponent<UnitManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("head").renderer.isVisible &&( manager.unitsOnScreen.Contains(unit)!=true)) {
			manager.unitsOnScreen.Add(unit);
		}
		else {
			if ((GameObject.Find("head").renderer.isVisible!=true) && (manager.unitsOnScreen.Contains(unit)==true)) {
				manager.unitsOnScreen.Remove(unit);
			}
		}
	}
}
