﻿using UnityEngine;
using System.Collections;

public class TrainingScript : MonoBehaviour {

	// Template for Bunny prefab
	public GameObject bunnyPrefab;

	// Default bunny spawn location
	GameObject spawnLoc;

	// List of all our bunny objects
	private ArrayList bunnies;

	// Use this for initialization
	void Start () {
		spawnLoc = GameObject.Find("BunnySpawn");
		bunnies = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		// Left click to create a bunny
		if (Input.GetMouseButtonDown(0)) {
			CreateBunny();
		}

		// Loops through all existing bunnies, and moves them randomly
		// TODO: Use NEAT to determine actions of each bunny
		foreach(GameObject bunnyObj in bunnies) {
			// Use this to access anything associated with a specific bunny. To add NEAT stuff, 
			// just make public variables inside the BunnyControl class.
			BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();

			// Randomly move bunnies, with a bias toward moving straight
			int moveDirIndex = Random.Range(0, 20);
			if (moveDirIndex == 0) {
				bunny.MoveLeft();
			}
			else if (moveDirIndex == 1) {
				bunny.MoveRight();
			}
			else {
				bunny.MoveStraight();
			}

			// Respawn bunny if far away from spawn
			if (Vector3.Distance(bunny.transform.position, spawnLoc.transform.position) > 20) {
				RespawnBunny(bunnyObj);
			}
		}
	}

	// Spawns a new Bunny GameObject and adds it to the bunnies list
	void CreateBunny() {
		bunnies.Add((GameObject)Instantiate(bunnyPrefab, spawnLoc.transform.position, Quaternion.identity));
	}

	// Respawns the specified Bunny at the spawn location
	// TODO: Evolve bunny's neural network
	void RespawnBunny(GameObject bunnyObj) {
		bunnyObj.transform.position = spawnLoc.transform.position;
		bunnyObj.transform.rotation = Quaternion.identity;
	}
}
