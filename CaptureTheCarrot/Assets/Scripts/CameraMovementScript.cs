using UnityEngine;
using System.Collections;

/* Camera code adapted from http://answers.unity3d.com/questions/13524/rts-style-camera-scrolling.html */

// TODO: Add zoom and boundaries

public class CameraMovementScript : MonoBehaviour {

	public int scrollDistance = 5; 
	public float scrollSpeed = 70;
	public float zoomSpeed = 10;

	public Vector3 camPos;

	private GameObject camera;
	
	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Main Camera");
		camPos = camera.transform.position;
	}
	
	// Move Camera Script
	float horizontalSpeed = 40f;
	float verticalSpeed = 40f;

	// Update is called once per frame
	void Update () {
		
		float mousePosX = Input.mousePosition.x; 
		float mousePosY = Input.mousePosition.y; 

		// Uncomment for mouse controls
		/*
		if (mousePosX < scrollDistance) 
		{ 
			transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime); 
		} 
		
		if (mousePosX >= Screen.width - scrollDistance) 
		{ 
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime); 
		}
		
		if (mousePosY < scrollDistance) 
		{ 
			transform.Translate(transform.forward * -scrollSpeed * Time.deltaTime); 
		} 
		
		if (mousePosY >= Screen.height - scrollDistance) 
		{ 
			transform.Translate(transform.forward * scrollSpeed * Time.deltaTime); 
		}
		*/

		// Move camera with WASD keys
		if (Input.GetKey("a")) 
		{ 
			transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime); 
		} 
		
		if (Input.GetKey("d")) 
		{ 
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime); 
		}
		
		if (Input.GetKey("s")) 
		{ 
			transform.Translate(transform.forward * -scrollSpeed * Time.deltaTime); 
		} 
		
		if (Input.GetKey("w")) 
		{ 
			transform.Translate(transform.forward * scrollSpeed * Time.deltaTime); 
		}

		if (Input.GetKeyDown("-") || Input.GetAxis ("Mouse ScrollWheel") < 0) {
			camera.transform.Translate(Vector3.back * zoomSpeed);
		}

		if (Input.GetKeyDown("=") || Input.GetAxis ("Mouse ScrollWheel") > 0) {
			camera.transform.Translate(Vector3.forward * zoomSpeed);
		}

		camPos = camera.transform.position;
	}
}
