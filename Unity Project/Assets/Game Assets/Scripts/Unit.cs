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
	private UnitManager unitManager;

	public void Start () {
		unitManager = GameObject.Find ("Game Manager").GetComponent<UnitManager>();
		scaleY = transform.Find ("HealthBar").localScale.y;

		Color teamColor1 = new Color (51/255f,39/255f,32/255f);
		Color teamColor2 = new Color (0.1f,0.1f,0.1f);
		Color teamColor3 = new Color (104/255f,102/255f,99/255f);

//		transform.Find ("Model").Find ("BetaHighResMeshes").Find ("Beta_HighLimbsGeo").renderer.material.color = teamColor1;
//		transform.Find ("Model").Find ("BetaHighResMeshes").Find ("Beta_HighJointsGeo").renderer.material.color = teamColor2;
//		transform.Find ("Model").Find ("BetaHighResMeshes").Find ("Beta_HighTorsoGeo").renderer.material.color = teamColor3;


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
		//print ("I'm here!");
		if(!unitManager.unitsOnScreen.Contains(this.gameObject)) {
			unitManager.unitsOnScreen.Add(this.gameObject);
		}
	}

	public void OnBecameInvisible() {
		//print ("I'm gone!");
		if(unitManager.unitsOnScreen.Contains(this.gameObject)) {
			unitManager.unitsOnScreen.Remove(this.gameObject);
		}

	}	
}
