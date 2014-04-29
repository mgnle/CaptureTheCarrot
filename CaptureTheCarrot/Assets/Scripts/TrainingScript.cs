using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class TrainingScript : MonoBehaviour {

	public const int INPUTS = 5+1;	// 1 bias node
	public const int OUTPUTS = 2;

	// Template for prefabs
	public GameObject bunnyPrefab;
	public GameObject enemyBunnyPrefab;
	public GameObject carrotPrefab;
	public GameObject mudPrefab;
	
	// Number of bunnies to spawn
	public int numBunnies = 10;

	// Default bunny spawn location
	GameObject spawnLoc;

	// List of all our bunny objects
	private List<GameObject> bunnies;

	// Holds the seconds since the start of the game
	private float time;

	// The seconds till we replace the worst bunny
	private int SEC_TIL_REMOVE_BUNNY = 4;

	TrainingGUIScript gui;

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
		bunnies = new List<GameObject>();
		time = Time.fixedTime;
		gui = GameObject.Find("Terrain").GetComponent<TrainingGUIScript>();
		
		// Spawn the bunnies!
		for (int i=0; i<numBunnies; i++) {
			CreateBunny();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Left click to create a bunny at spawn location
		/*
		if (Input.GetMouseButtonDown(0)) {
			CreateBunny();
		}
		*/
		
		// Right click to add current selection to map
		if (Input.GetMouseButtonDown(1)) {
			// Delete object if LCtrl+Right Click (may have consequenses)
			if (Input.GetButton("Fire1")) {
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
					if (!hit.collider.name.Equals("Terrain")) {
						Destroy(hit.collider.gameObject);
					}
				}
			}
			else {
				switch(gui.selectedItem)
				{
				case TrainingGUIScript.Item.EnemyBunny:
					CreateEnemyBunny(Camera.main.ScreenPointToRay (Input.mousePosition));
					break;
				case TrainingGUIScript.Item.Carrot:
					CreateCarrot(Camera.main.ScreenPointToRay (Input.mousePosition));
					break;
				case TrainingGUIScript.Item.Mud:
					CreateMud(Camera.main.ScreenPointToRay (Input.mousePosition));
					break;
				}
			}
		}

		// Check if the time is up
		if (TimeUp()) {
			ReplaceWorstBunny();
		}
		
		// Loops through all existing bunnies, and moves them randomly
		foreach(GameObject bunnyObj in bunnies) {
			// Use this to access anything associated with a specific bunny. To add NEAT stuff, 
			// just make public variables inside the BunnyControl class.
			BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();
			
			if (bunny.CalculateOnTargetSensor() == 1)
				bunny.FireCabbageGun();

			GameObject carrot = GameObject.Find("Carrot");
			if (carrot != null) {
				List<GameObject> carrotArray = new List<GameObject>();
				
				carrotArray.Add (carrot);
				bunny.FindRadarValues(carrotArray);		
			}
			// For testing cabbage gun
			/*
			if (Input.GetKeyDown ("f"))
				bunny.FireCabbageGun();
			*/
			
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
			}*/

			// Respawn bunny if far away from spawn
			/*if (Vector3.Distance(bunny.transform.position, spawnLoc.transform.position) > 30) {
				RespawnBunny(bunnyObj);
			}*/
			
		}
	}

	// Spawns a new bunny in the bunny hole and adds it to the bunnies list
	void CreateBunny() {
		GameObject bunnyObj = (GameObject)Instantiate(bunnyPrefab, new Vector3(spawnLoc.transform.position.x, 0.8f, spawnLoc.transform.position.z), Quaternion.identity);
		
		// Create a brain for the bunny
		BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();		
		bunny.brain = new SimpleNeuralNetwork(INPUTS, OUTPUTS);
		bunny.birthday = Time.fixedTime;
		
		bunnies.Add(bunnyObj);
	}

	// Spawns an enemy bunny
	void CreateEnemyBunny(Ray ray) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit)) {
			GameObject bunnyObj = (GameObject)Instantiate(enemyBunnyPrefab, new Vector3(hit.point.x, 0.8f, hit.point.z), Quaternion.identity);
			
			// Create a brain for the bunny
			//BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();		
			//bunny.brain = new SimpleNeuralNetwork(bunnyObj, INPUTS, OUTPUTS);
		}
	}

	// Spawns a carrot
	void CreateCarrot(Ray ray) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit)) {
			GameObject carrotObj = (GameObject)Instantiate(carrotPrefab, hit.point, Quaternion.identity);
		}
	}

	// Spawns a mud pit
	void CreateMud(Ray ray) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit)) {
			GameObject mudObj = (GameObject)Instantiate(mudPrefab, new Vector3(hit.point.x, 0.15f, hit.point.z), Quaternion.identity);
		}
	}

	// Respawns the specified Bunny at the spawn location
	void RespawnBunny(GameObject bunnyObj) {
		bunnyObj.transform.position = new Vector3(spawnLoc.transform.position.x, 0.8f, spawnLoc.transform.position.z);
		bunnyObj.transform.rotation = Quaternion.identity;
	}
	
	// Get the bunny with the lowest fitness and replace it with the two highest fitness bunnies
	void ReplaceWorstBunny() {
		if(bunnies.ToArray().Length > 0) {
			GameObject worstBunny = bunnies.ToArray()[0];
			GameObject bestBunny = bunnies.ToArray()[0];
			GameObject secondBestBunny = bunnies.ToArray()[0];
			
			foreach(GameObject bunnyObj in bunnies) {
				float worstBunnyEval = worstBunny.GetComponent<BunnyControl>().brain.Evaluate();
				float bestBunnyEval = bestBunny.GetComponent<BunnyControl>().brain.Evaluate();
				float secondBestBunnyEval = secondBestBunny.GetComponent<BunnyControl>().brain.Evaluate();
			
				BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();
				float bunnyEval = bunny.brain.Evaluate();
				
				// Only respawns if it has been alive for long enough
				if(HasBeenAliveLongEnough(bunny) && bunnyEval < worstBunnyEval) {
					worstBunny = bunnyObj;
				}
				
				if(bunnyEval > bestBunnyEval) {
					bestBunny = bunnyObj;
				}
				else if(bunnyEval > secondBestBunnyEval) {
					secondBestBunny = bunnyObj;
				}				
			}
			
			// If the default worst bunny has not been alive long enough remove no one
			if(!HasBeenAliveLongEnough(worstBunny.GetComponent<BunnyControl>())) {
				return;
			}
			
			// Take two best bunnies, create new neural network combining both, and place in game
			SimpleNeuralNetwork bestBrain = bestBunny.GetComponent<BunnyControl>().brain;
			SimpleNeuralNetwork secondBestBrain = secondBestBunny.GetComponent<BunnyControl>().brain;
			SimpleNeuralNetwork newBrain = new SimpleNeuralNetwork(bestBrain, secondBestBrain);
			worstBunny.GetComponent<BunnyControl>().brain = newBrain;
			worstBunny.GetComponent<BunnyControl>().birthday = Time.fixedTime;
			RespawnBunny(worstBunny);
		}			
	}

	bool TimeUp() {
		time = Time.fixedTime;

		if (time > 0 && time % SEC_TIL_REMOVE_BUNNY == 0) {
			return true;
		} else {
			return false;
		}
	}
	
	bool HasBeenAliveLongEnough(BunnyControl bunny) {
		time = Time.fixedTime;
		
		if (time > bunny.birthday + Constants.TIME_ALIVE_THRESHOLD) {
			return true;
		} else {
			return false;
		}
	}

}
