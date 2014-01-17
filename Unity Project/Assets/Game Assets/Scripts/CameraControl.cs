using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public float cameraSpeed = 0.1f, rotateSpeed = 1.0f;
	public int maxHeight = 50;
	
	float screenBorder = 5f;
	float mouseSpeed = .75f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		
		if ( Input.mousePosition.x >= Screen.width - screenBorder ) {
			transform.Translate (Vector3.right*mouseSpeed);
		}

		if (Input.mousePosition.x <= 0 + screenBorder) {
			transform.Translate (Vector3.left*mouseSpeed);
		}

		if(Input.mousePosition.y <= 0) {
			transform.Translate (Vector3.back*mouseSpeed);
		}

		if ( Input.mousePosition.y >= Screen.height+screenBorder*20) {
			transform.Translate (Vector3.forward*mouseSpeed);
		}


		float x=0.0f, y=0.0f, z=0.0f;
		float rotate = 0.0f, orbit=0.0f;
		Vector3 position = transform.position;

		if (Input.GetKey("w")) {
			z+=cameraSpeed;
		}
		if(Input.GetKey("s")) {
			z-=cameraSpeed;
		}
		if(Input.GetKey("x")) {
			x+=cameraSpeed;
		}
		if(Input.GetKey("z")) {
			x-=cameraSpeed;
		}
		if (Input.GetKey ("a")) {
			rotate-=rotateSpeed;
		}
		if (Input.GetKey ("d")) {
			rotate+=rotateSpeed;
		}
		if (Input.GetKey ("q")) {
			orbit+=rotateSpeed;
		}
		if (Input.GetKey ("e")) {
			orbit-=rotateSpeed;
		}
		if (Input.GetKey ("c")) {
			if(position.y > 1) {
				z+=cameraSpeed;
				y-=cameraSpeed;
			}
		}
		if (Input.GetKey ("v")) {
			if(position.y < maxHeight) {
				z-=cameraSpeed;
				y+=cameraSpeed;
			}
		}

		transform.Translate (x, y, z);
		transform.eulerAngles = new Vector3(0,rotate + transform.eulerAngles.y,0);
		transform.RotateAround (position+transform.forward*(orbit+10), Vector3.up, orbit);
	}
}
