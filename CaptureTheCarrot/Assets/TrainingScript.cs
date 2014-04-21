using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class TrainingScript : MonoBehaviour {

	public const int INPUTS = 4;
	public const int OUTPUTS = 2;

	// Template for Bunny prefab
	public GameObject bunnyPrefab;

	// Default bunny spawn location
	GameObject spawnLoc;

	// List of all our bunny objects
	private ArrayList bunnies;

	// Holds the seconds since the start of the game
	private float time;

	// The seconds till we replace the worst bunny
	private int SEC_TIL_REMOVE_BUNNY = 10;

	// rtNEAT Loop:
	/*
	1. Calculate the adjusted fitness of all current individuals in the population
	2. Remove the agent with the worst adjusted fitness from the population provided one has been alive sufficiently long enough so that it has been properly evaluated
	3. Re-estimate the average fitness F for all species
	4. Choose a parent species to create the new offspring
	5. Adjust d dynamically and reassign all agents to species
	6. Place the new agent in the world
	 */

	// Use this for initialization
	void Start () {
		spawnLoc = GameObject.Find("BunnySpawn");
		bunnies = new ArrayList();
		time = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
		// Left click to create a bunny
		if (Input.GetMouseButtonDown(0)) {
			CreateBunny();
		}

		// Check if the time is up
		if (TimeUp()) {
			// TODO: Use NEAT to choose worst bunny and replace with best bunny - rtNEAT Loop
		}
		
		// Loops through all existing bunnies, and moves them randomly
		// TODO: Use NEAT to determine actions of each bunny
		foreach(GameObject bunnyObj in bunnies) {
			// Use this to access anything associated with a specific bunny. To add NEAT stuff, 
			// just make public variables inside the BunnyControl class.
			BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();

			bunny.CalculateRadar(GameObject.Find("Carrot"));

			// Move bunnies with arrow keys
			/*
			if (Input.GetKey ("a"))
			    bunny.MoveLeft();
		    if (Input.GetKey ("d"))
			    bunny.MoveRight();
		    if (Input.GetKey ("w"))
			    bunny.MoveStraight();
			if (Input.GetKey ("s"))
				bunny.MoveBack();
			*/

			// Randomly move bunnies, with a bias toward moving straight
			/*
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
			if (Vector3.Distance(bunny.transform.position, spawnLoc.transform.position) > 30) {
				RespawnBunny(bunnyObj);
			}
			*/
		}
	}

	// Spawns a new Bunny GameObject and adds it to the bunnies list
	void CreateBunny() {
		GameObject bunnyObj = (GameObject)Instantiate(bunnyPrefab, spawnLoc.transform.position, Quaternion.identity);
		
		// Create a brain for the bunny
		BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();		
		bunny.brain = new SimpleNeuralNetwork(INPUTS, OUTPUTS);
		
		bunnies.Add(bunnyObj);
	}

	// Respawns the specified Bunny at the spawn location
	// TODO: Evolve bunny's neural network
	void RespawnBunny(GameObject bunnyObj) {
		bunnyObj.transform.position = spawnLoc.transform.position;
		bunnyObj.transform.rotation = Quaternion.identity;
	}

	bool TimeUp() {
		time = Time.fixedTime;

		if (time % SEC_TIL_REMOVE_BUNNY == 0) {
			return true;
		} else {
			return false;
		}
	}
}
