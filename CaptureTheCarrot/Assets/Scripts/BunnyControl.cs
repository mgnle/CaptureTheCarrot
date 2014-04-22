using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

[RequireComponent (typeof (CharacterController))]

public class BunnyControl : MonoBehaviour {
		
	// Neural Network bunny "brain"
	public INeuralNetwork brain;
	
	// Enum for the actions that can be taken
	public enum Action
	{
		MoveLeft,
		MoveRight,
		MoveForward,
		MoveBackward,
		Fire
	}
	// Movement properties
	public float moveDistance;
	public float rotationAngle;

	// Access variables
	public Vector3 bunnyPos;

	// Movement vector
	public Vector3 moveVector = Vector3.zero;

	CharacterController controller;
	CollisionFlags collisionFlags;
	bool isGrounded;

	// initial position
	Vector3 initialPosition;
	Quaternion initialRotation;
	
	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();

		initialPosition = transform.position;
		initialRotation = transform.rotation;
		bunnyPos = initialPosition;
	}
	
	// Update is called once per frame
	void Update () {
		bunnyPos = transform.position;
		
		// TODO: Populate the neural network input array with the correct inputs
		
		
		brain.InputSignalArray = new float[]{3, 5, 1, 0};
		brain.Activate();
		float maxValue = 0;
		Action action = Action.MoveForward;
		for(int i=0; i<brain.OutputSignalArray.Length; i++)
		{
			if (brain.OutputSignalArray[i] > maxValue)
			{
				maxValue = Math.Abs(brain.OutputSignalArray[i]);
				action = (Action)i;
			}
		}
		
		switch(action)
		{
			case Action.MoveLeft:
				MoveLeft();
				break;
			case Action.MoveRight:
				MoveRight();
				break;
			case Action.MoveBackward:
				MoveBack();
				break;
			case Action.MoveForward:
			default:
				MoveStraight();
				break;
		}		
	}

	/* Rotates bunny to the right and moves forward */
	public void MoveRight() {
		transform.Rotate(0f, rotationAngle, 0f);
		Vector3 oldPos = transform.position;
		moveVector = transform.forward * moveDistance;
		transform.position = oldPos + moveVector;
	}

	/* Rotates bunny to the left and moves forward */
	public void MoveLeft() {
		transform.Rotate(0f, -rotationAngle, 0f);
		Vector3 oldPos = transform.position;
		moveVector = transform.forward * moveDistance;
		transform.position = oldPos + moveVector;
	}

	/* Moves bunny forward */
	public void MoveStraight() {
		Vector3 oldPos = transform.position;
		moveVector = transform.forward * moveDistance;
		transform.position = oldPos + moveVector;
	}

	/* Moves bunny backward */
	public void MoveBack() {
		Vector3 oldPos = transform.position;
		moveVector = -1 * transform.forward * moveDistance;
		transform.position = oldPos + moveVector;
	}

	public float[] FindRadarValues(ArrayList objs) {
		float[] radars = new float[4];
		foreach(GameObject obj in objs) {
			radars[CalculateRadar(obj)] += CalculateDistance(obj);
		}
		return radars;
	}
	
	public float CalculateDistance(GameObject obj) {
		Vector3 vector = obj.transform.position - transform.position;
		float distance = vector.magnitude;
		return distance;
	}
	
	public int CalculateRadar (GameObject obj)
	{
		Vector3 facing = transform.forward;
		Vector3 perpendicular = new Vector3(facing.z, facing.y, -facing.x);
		Vector3 carrot = obj.transform.position - transform.position;
		float degree = Vector3.Angle (perpendicular, carrot);
		//Debug.Log(degree);
		if(transform.InverseTransformPoint(obj.transform.position).z < 0)
			return -1;
		
		if ((degree >= 0) && (degree <= 45)){
			Debug.Log (0);
			return 0;
		} else if (degree >= 45 && degree <= 90) {
			Debug.Log (1);
			return 1;
		} else if (degree >= 90 && degree <= 135) {
			Debug.Log (2);
			return 2;
		} else if (degree >= 135 && degree <= 180) {
			Debug.Log (3);
			return 3;
		} else {
			Debug.Log ("Unknown Degree in method CalculateRadar");
			return -1;
		}
	}
}
