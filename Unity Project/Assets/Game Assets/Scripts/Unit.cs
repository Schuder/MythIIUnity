using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	//Unit Stats
	public string type;
	public int team;

	public float healthMax;
	public float health;

	public int speed;

	public bool proximityAttack;
	public float proximityRange;

	public float attackDamage;
	public float attackDamageMargin;
	public float attackMissFraction;
	public float attackRange;
	public float attackTime;

	public int reactionTime;	

	public Vector2 screenPosition;
	private float scaleY;

	public void Start () {

		scaleY = transform.Find ("HealthBar").localScale.y;
		Transform model = transform.Find("Model");
		Color teamColor = Color.white;
		Color teamColorAlt = Color.white;
		if (team == 1) {
			teamColor = new Color(0.75f,0.75f,1.0f);
			teamColorAlt = new Color(1.0f,1.0f,1.0f);
		}
		else if(team ==2) {
			teamColor = new Color(0.25f,0.25f,0.25f);
			teamColorAlt = new Color(0.75f,0.25f,0.25f);
		}

		model.Find ("armor1").renderer.material.color = teamColorAlt;
		model.Find ("crotch_guard").renderer.material.color = teamColor;
		model.Find ("crotch_guard1").renderer.material.color = teamColor;
		model.Find ("crotch_guard2").renderer.material.color = teamColor;
		
		model.Find ("glutius_guard").renderer.material.color = teamColor;
		model.Find ("glutius_guard1").renderer.material.color = teamColor;
		model.Find ("glutius_guard2").renderer.material.color = teamColor;
		model.Find ("glutius_guard3").renderer.material.color = teamColor;
		
		model.Find ("helmet_finished").renderer.material.color = teamColor;
		
		//GameObject.Find ("left_arm").renderer.material.color = teamColor;
		//GameObject.Find ("left_boot").renderer.material.color = teamColor;
		model.Find ("left_shoulder_guard").renderer.material.color = teamColor;
		
		//GameObject.Find ("right_arm3").renderer.material.color = teamColor;
		//GameObject.Find ("right_boot").renderer.material.color = teamColor;
		model.Find ("right_shoulder_guard").renderer.material.color = teamColor;


	}

	void Update () {
		if (health >= 0) {
			Transform healthBar = transform.Find("HealthBar");
			healthBar.localScale = new Vector3 (healthBar.localScale.x,scaleY * (health / healthMax), healthBar.localScale.z);
			healthBar.localPosition = new Vector3 (healthBar.localPosition.x, -1*((scaleY-healthBar.localScale.y)/2f) ,healthBar.localPosition.z);
		}
		else {

			if(GameObject.Find("Player").GetComponent<UnitControl>().selectedUnits.Contains(gameObject)) {
				GameObject.Find("Player").GetComponent<UnitControl>().selectedUnits.Remove(gameObject);
			}
			if(GameObject.Find ("Game Manager").GetComponent<UnitManager>().unitsOnScreen.Contains(gameObject)){
				GameObject.Find("Game Manager").GetComponent<UnitManager>().unitsOnScreen.Remove(gameObject);
			}
			GameObject.Find("Game Manager").GetComponent<UnitManager>().unitsInGame.Remove(gameObject);
			Destroy(gameObject.GetComponent<Unit>());
			Destroy(gameObject);

		}
	}

	public void OnBecameVisible() {
		print ("I'm here!");
		GameObject.Find ("Game Manager").GetComponent<UnitManager> ().unitsOnScreen.Add(this.gameObject);
	}

	public void OnBecameInvisible() {
		print ("I'm gone!");
		GameObject.Find ("Game Manager").GetComponent<UnitManager> ().unitsOnScreen.Remove(this.gameObject);
	}	
}
