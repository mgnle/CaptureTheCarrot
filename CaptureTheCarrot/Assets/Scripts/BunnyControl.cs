﻿using UnityEngine;
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
		MoveBackward,
		MoveForward,
		Fire
	}
	// Movement properties
	public float moveDistance;
	public float rotationAngle;

	// Access variables
	public Vector3 bunnyPos;

	// Movement vector
	public Vector3 moveVector = Vector3.zero;
	
	// Template for Cabbage prefab
	public GameObject cabbagePrefab;	
	
	// Array of inputs
	float[] inputArray = new float[]{1f, 1f, 1f, 1f};
	
	// Fitness of the neural network and the count for
	// calculations
	float fitness = 0;
	int fitnessCount = 0;

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
				
		CalculateOnTargetSensor();
				
		brain.InputSignalArray = inputArray;
		brain.Activate();
		float maxValue = 0f;
		Action action = Action.MoveForward;
		for(int i=0; i<brain.OutputSignalArray.Length; i++)
		{
			if (brain.OutputSignalArray[i] > maxValue)
			{
				maxValue = Math.Abs(brain.OutputSignalArray[i]);
				if(i < 0)
				{
					action = (Action)(i*2);
				}
				else
				{
					action = (Action)(i*2 + 1);
				}
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

	/* Fires a cabbage gun in the bunny's forward direction */
	public void FireCabbageGun() {
		GameObject cabbageObj = (GameObject)Instantiate(cabbagePrefab, transform.position + transform.forward * 2f + new Vector3(0f, 1f, 0f), transform.rotation);
	}

	public void FindRadarValues(GameObject obj) {
		float[] radars = new float[4];
		//foreach(GameObject obj in objs) {
			if (CalculateRadar(obj) != -1)
				radars[CalculateRadar(obj)] += CalculateDistance(obj);
		//}
		inputArray = radars;
	}
	
	public float CalculateDistance(GameObject obj) {
		Vector3 vector = obj.transform.position - transform.position;
		float distance = vector.magnitude;
		return distance;
	}
	
	/* Returns the radar that the object is in according to the position
	   the bunny is facing. Returns -1 if the object is not in one of the
	   radars (not in the bunny's sight).
	   -1: behind bunny somewhere
	   0: 0 degrees to 45 degrees
	   1: 45 degrees to 90 degrees
	   2: 90 degrees to 135 degrees
	   3: 135 degrees to 180 degrees */
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
			return 0;
		} else if (degree >= 45 && degree <= 90) {
			return 1;
		} else if (degree >= 90 && degree <= 135) {
			return 2;
		} else if (degree >= 135 && degree <= 180) {
			return 3;
		} else {
			Debug.Log ("Unknown Degree in method CalculateRadar");
			return -1;
		}
	}
	
	/* Returns 1 (full activation) if the onTargetSensor collides with
	   anything within 100 units. Return 0 otherwise. */
	public int CalculateOnTargetSensor() {
		Vector3 position = transform.position + transform.forward;
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(position, transform.forward, out hit, 100))
			return 1;
        else
            return 0;
    }
	
	/* Calculates the fitness of the neural network by finding the
	   average distance from the object. Then takes the inverse
	   to find a number between 0 and 1. The higher the fitness, the
	   better the neural network. */
	public float CalculateFitness(GameObject obj) {
		fitnessCount++;
		if (fitness != 0)
			fitness = 1 / fitness;
		fitness = 1 / ((fitness*(fitnessCount-1) + CalculateDistance(obj)) / fitnessCount);
		Debug.Log (fitness);
		return fitness;
	}
}










