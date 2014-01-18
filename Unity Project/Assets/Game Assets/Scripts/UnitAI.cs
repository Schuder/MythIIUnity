using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class UnitAI : MonoBehaviour {

	private Seeker seeker;
	private CharacterController controller;
	public float nextWaypointDistance;
	private int speed;
	private bool proximityAttack;
	private float proximityRange;
	private float attackRange;
	private float attackDamage;
	public float attackDamageMargin;
	public float attackMissFraction;
	private int reactionTime;
	private int currentWaypoint;
	private float attackTime;

	private Path path;
	private int frameCount = 0;
	private Unit thisUnit;
	public GameObject target;

	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		thisUnit = this.GetComponent<Unit>();

		speed = thisUnit.speed;
		proximityAttack = thisUnit.proximityAttack;
		proximityRange = thisUnit.proximityRange;
		attackRange = thisUnit.attackRange;
		attackDamage = thisUnit.attackDamage;
		reactionTime = thisUnit.reactionTime;
		attackTime = thisUnit.attackTime;
		attackDamageMargin = thisUnit.attackDamageMargin;
		attackMissFraction = thisUnit.attackMissFraction;
	}
	
	// Update is called once per frame
	void Update () {
		frameCount++;
		if (frameCount == int.MaxValue) {
			frameCount = 0;
		}

		if (!target) {
			transform.Find ("Model").GetComponent<Animator>().SetBool("attacking",false);
		}

		if (proximityAttack) {
			if(frameCount%reactionTime==0){

				if(!target) {
					lookForTarget();
				}
				else {
					if(Vector3.Distance (transform.position,target.transform.position) > proximityRange) {
						target = null;
					}
				}
			}
		}

		if (target) {
			print ("there is a target");
			if(frameCount%reactionTime==0) {
				if(Vector3.Distance (transform.position,target.transform.position) > attackRange) {
					followPath();
				}

			}
			if(Vector3.Distance (transform.position,target.transform.position) < attackRange&&frameCount%attackTime==0) {
				print ("doing damage");
				transform.Find ("Model").GetComponent<Animator>().SetBool("attacking",true);
				damageTarget();
			}
		}

		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			//Debug.Log ("End Of Path Reached");
			path=null;
			transform.Find ("Model").GetComponent<Animator>().SetBool("moving",false);
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.deltaTime;
		controller.SimpleMove (dir);
		controller.transform.LookAt (new Vector3(path.vectorPath[currentWaypoint].x , transform.position.y , path.vectorPath[currentWaypoint].z));

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}	
	}

	public void damageTarget() {
		int hitChance = Random.Range (1,101);
		int hitMargin;
		float damage=0.0f;
		if (!((hitChance / 100f) <= attackMissFraction)) {
			hitMargin = Random.Range(1,101);
			if(hitMargin > 50) {
				damage = this.attackDamage+this.attackDamageMargin;
			}
			else if(hitMargin < 50) {
				damage = this.attackDamage-this.attackDamageMargin;
			}
			else {
				damage = this.attackDamage;
			}
		}
		print (damage);
		target.GetComponent<Unit>().health-=damage;
	}

	public void lookForTarget() {

		List<GameObject> _temp = GameObject.Find("Game Manager").GetComponent<UnitManager>().unitsInGame;

		foreach (GameObject unit in _temp) {
			if(Vector3.Distance(this.transform.position,unit.transform.position) < proximityRange&& unit.GetComponent<Unit>().team != this.GetComponent<Unit>().team) {
				target = unit;
				break;
			}
		}

	}

	public void followPath() {
		Path _temp = seeker.GetNewPath (transform.position, target.transform.position);
		transform.Find ("Model").GetComponent<Animator>().SetBool("moving",true);
		seeker.StartPath (_temp, OnPathComplete);
	}

	public void OnPathComplete (Path p) {
		//Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}

	public IEnumerator move(Vector3 targetPos,bool cancelAttack,int delay) {
		if (cancelAttack) {
			target = null;
		}
		Path _temp = seeker.GetNewPath (transform.position, targetPos);
		yield return new WaitForSeconds(Random.Range(0,5)*0.15f);
		transform.Find ("Model").GetComponent<Animator>().SetBool("moving",true);
		seeker.StartPath (_temp, OnPathComplete);
	}

	public void attack(GameObject unit) {
		target = unit;
	}
}
