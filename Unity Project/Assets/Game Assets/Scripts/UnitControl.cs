using UnityEngine;
using System.Collections.Generic;

public class UnitControl : MonoBehaviour {

	public List<GameObject> selectedUnits;
	private float doubleClickStart = 0.0f;
	public int unitSeperation = 2;
	public int rowSeperation = 1;
	public int formation = 0;
	private Vector2 firstPoint;
	public int team = 0;
	private bool drawSelection = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		drawSelection = false;
		if (Input.GetMouseButtonDown (0)) {
			firstPoint = Input.mousePosition;
			if ((Time.time - doubleClickStart) < 0.3f) {
				doubleLeftClick();
				doubleClickStart = -1;
				return;
			}
			else {
				doubleClickStart = Time.time;
			}

			leftClick();
		}
		else if(Input.GetMouseButtonDown(1)) {

			rightClick();

		}
		else if(Input.GetMouseButton(0)) {
			drawSelection =true;
		}
		else if(Input.GetMouseButton(1)) {
		}

	}

	void leftClick() {
		Camera cam = Camera.main;

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int selectablesLayer = 1 << 10;
		
		Ray groundRay = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit groundHit;
		int groundLayer = 1 << 8;

		if(Physics.Raycast(ray,out hit,Mathf.Infinity,selectablesLayer) && Input.GetKey("left shift")) {
			selectUnit(hit.collider.gameObject);
			return;
		}
		
		if(Physics.Raycast(ray,out hit,Mathf.Infinity,selectablesLayer)&&!drawSelection&&!(Input.GetKey("left shift"))) {
			deselectAllUnits();
			selectUnit(hit.collider.gameObject);
			return;
		}
		
		if(Physics.Raycast(groundRay,out groundHit,Mathf.Infinity,groundLayer)&&!drawSelection&&!(Input.GetKey("left shift"))) {
			deselectAllUnits();
			return;
		}
	}
	void rightClick() {
		Camera cam = Camera.main;

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		Ray groundRay = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit groundRayHit;
		int groundLayer = 1 << 8;
		int selectablesLayer = 1 << 10;

		if(Physics.Raycast(ray,out rayHit,Mathf.Infinity,selectablesLayer)) {
			selectedUnitsAttack(rayHit.collider.gameObject);
		}
		else if(Physics.Raycast(groundRay,out groundRayHit,Mathf.Infinity,groundLayer)) {
			print (selectedUnits.Count);
			switch(formation) {
			case 0: formation0(groundRayHit.point); break;
			}
		}
	}
	void doubleLeftClick() {
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int selectablesLayer = 1 << 10;
		
		Ray groundRay = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit groundHit;
		int groundLayer = 1 << 8;
		
		if(Physics.Raycast(ray,out hit,Mathf.Infinity,selectablesLayer) && Input.GetKey("left shift")) {
			selectUnitsOfSameType(hit.collider.gameObject);
			return;
		}
		
		if(Physics.Raycast(ray,out hit,Mathf.Infinity,selectablesLayer)) {
			deselectAllUnits();
			selectUnitsOfSameType(hit.collider.gameObject);
			return;
		}
		
		if(Physics.Raycast(groundRay,out groundHit,Mathf.Infinity,groundLayer)) {
			deselectAllUnits();
			return;
		}
	}

	void OnGUI() {
		if (drawSelection&&firstPoint.x != Input.mousePosition.x && firstPoint.y != Input.mousePosition.y) {

			Vector2 secondPoint = Input.mousePosition;
			Rect selectionGUI = new Rect(firstPoint.x,Screen.height-firstPoint.y,secondPoint.x-firstPoint.x,-1*((Screen.height-firstPoint.y)-(Screen.height-secondPoint.y)));
			GUI.Box(selectionGUI,"");

			foreach(GameObject unit in GameObject.Find("Game Manager").GetComponent<UnitManager>().unitsOnScreen) {
				Vector2 pos = Camera.main.WorldToScreenPoint(unit.transform.position);
				pos.y = Screen.height-pos.y;
				if(selectionGUI.Contains(pos,true)&&!(selectedUnits.Contains(unit))) {
					selectUnit(unit);
				}
				else if(!(selectionGUI.Contains(pos,true))&&selectedUnits.Contains(unit)) {
					deselectUnit(unit);
				}
			}

		}


	}

	void selectedUnitsAttack(GameObject target){
		int targetTeam = target.GetComponent<Unit> ().team;
		foreach (GameObject unit in selectedUnits) {
			if(unit.GetComponent<Unit>().team != targetTeam) {
				unit.GetComponent<UnitAI>().attack(target);
			}
			else {
				unit.GetComponent<UnitAI>().move(target.transform.position,true,1);
			}
		}
	}

	Vector3 pivotVector3(Vector3 target, Vector3 origin, Vector3 angles) {
		Vector3 dir;
		Vector3 newTarget;

		dir = target - (origin);
		dir = Quaternion.Euler(angles) * dir;
		newTarget = dir + (origin);

		return newTarget;
	}

	void formation0(Vector3 origin){
		int count = 0;
		float cameraRotation = (GameObject.Find("Player").transform.eulerAngles.y);
		int totalUnits = selectedUnits.Count;
		int unitsInRow = 0;
		Vector3 target;
		Vector3 angles =  new Vector3(0, (cameraRotation), 0);
		float rows = Mathf.Ceil(totalUnits/ 4.0f);
		print (selectedUnits.Count);
		for(int i = 0; i < rows; i++) {
			if(totalUnits-4 >= 0) {
				totalUnits-=4;
				unitsInRow = 4;
			}
			else {
				unitsInRow = totalUnits;
			}

			if(unitsInRow == 1) {
				target = origin+new Vector3(0,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				count++;
			}

			else if(unitsInRow==2) {
				target =origin+new Vector3(unitSeperation/2,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				count++;

				target = origin+new Vector3(-unitSeperation/2,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				count++;
			}
			else if(unitsInRow==3) {
				target = origin+new Vector3(0,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;

				target = origin+new Vector3(-unitSeperation,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;

				target = origin+new Vector3(unitSeperation,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;
			}
			else {
				target = origin+new Vector3(unitSeperation/2,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;

				target = origin+new Vector3(-unitSeperation/2,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;

				target = origin+new Vector3((unitSeperation/2)+unitSeperation,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;

				target = origin+new Vector3((-unitSeperation/2)-unitSeperation,0,i*-rowSeperation);
				target = pivotVector3(target,origin,angles);
				StartCoroutine(selectedUnits[count].GetComponent<UnitAI>().move(target,true,i));
				
				count++;
			}
				
		}
	}

	void deselectAllUnits() {
		foreach(GameObject unit in selectedUnits) {
			unit.GetComponent<Selectable>().unitSelected = false;
		}
		selectedUnits.Clear ();
	}

	void selectUnit(GameObject unit) {
		if(unit.GetComponent<Unit>().team == this.team) {
			unit.GetComponent<Selectable>().unitSelected = true;
			selectedUnits.Add (unit);
		}
	}


	void deselectUnit(GameObject unit) {
		unit.GetComponent<Selectable>().unitSelected = false;
		selectedUnits.Remove (unit);
	}

	void selectUnitsOfSameType(GameObject sourceUnit) {
		if(sourceUnit.GetComponent<Unit>().team == this.team) {
			sourceUnit.GetComponent<Selectable>().unitSelected = true;
		}
		foreach(GameObject unit in GameObject.Find("Game Manager").GetComponent<UnitManager>().unitsOnScreen) {
			if(unit.GetComponent<Unit>().type == sourceUnit.GetComponent<Unit>().type&&unit.GetComponent<Unit>().team == this.team) {
				selectedUnits.Add(unit);
				unit.GetComponent<Selectable>().unitSelected = true;
			}
		}
	}

}
