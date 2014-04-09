using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class BunnyControl : MonoBehaviour {

	// Movement properties
	public float moveSpeed;

	// Debug variables
	public bool isMoving;

	// Movement vector
	public Vector3 moveVector = Vector3.zero;

	CharacterController controller;
	CollisionFlags collisionFlags;
	bool isGrounded;

	// initial position
	Vector3 initialPosition;
	Quaternion initialRotation;
	Vector3 horizontalVelocity;
	
	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();

		initialPosition = transform.position;
		initialRotation = transform.rotation;

		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1)) {
			moveVector = new Vector3(1f, -0.001f, 0f);
			moveVector = transform.TransformDirection(moveVector);
			moveVector *= moveSpeed;
			isMoving = true;
		}
		else {
			moveVector = new Vector3(0f, -0.001f, 0f);
			isMoving = false;
		}

		collisionFlags = controller.Move(moveVector * Time.deltaTime);	
	}
}
