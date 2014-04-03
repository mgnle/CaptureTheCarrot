using UnityEngine;
using System.Collections;

public class CameraMovementScript : MonoBehaviour {

	public int scrollDistance = 5; 
	public float scrollSpeed = 70;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Move Camera Script
	float horizontalSpeed = 40f;
	float verticalSpeed = 40f;

	// Update is called once per frame
	void Update () {
		
		float mousePosX = Input.mousePosition.x; 
		float mousePosY = Input.mousePosition.y; 

		
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
	}
}
