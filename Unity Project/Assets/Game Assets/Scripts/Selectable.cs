using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	public bool unitSelected = false;
	private Transform selected; 
	private Transform unit;
	private float lastHealth;
	private Color healthColor;
	private float colorBrilliance = 255.0f;
	// Use this for initialization
	void Start () {
		selected = transform.Find("HealthBar");
		unit = selected.parent;
		lastHealth = unit.GetComponent<Unit> ().health;
		healthColor = new Color(0,1.0f,0);
		selected.renderer.material.color = healthColor;
	}
	
	// Update is called once per frame
	void Update () {

		if (unitSelected) {
			selected.renderer.enabled = true;
		} 
		else {
			selected.renderer.enabled = false;
		}

		if (unit.GetComponent<Unit> ().health != lastHealth && unit.GetComponent<Unit> ().health > 0) {
			lastHealth = unit.GetComponent<Unit> ().health;

			if(healthColor.r <1) {

				healthColor.r=(-1*(lastHealth*5.1f-(unit.GetComponent<Unit> ().healthMax*5.1f)))/colorBrilliance;

			}
			else if(healthColor.g > 0) {
				healthColor.g=(lastHealth*5.1f)/colorBrilliance;
			}
			selected.renderer.material.color  = healthColor;

		}
	
	}

	void OnGUI() {
	}
}
