using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class TestingScript : MonoBehaviour {

	// Template for prefabs
	public GameObject bunnyPrefab;
	public GameObject enemyBunnyPrefab;
	public GameObject carrotPrefab;
	public GameObject mudPrefab;
	
	// Default bunny spawn location
	GameObject spawnLoc;
	GameObject enemySpawnLoc;

	// List of all our bunny objects
	private List<GameObject> bunnies;
	private List<GameObject> enemyBunnies;
	
	// Find all carrots, enemy bunnies, and mud pits
	GameObject[] carrotArray;
	GameObject[] enemyArray;
	GameObject[] mudArray;
	
	private int winVal;
	private bool gameOver;
	
	// Holds the seconds since the start of the game
	private float time;

	// The seconds till we replace the worst bunny
	private int SEC_TIL_REMOVE_BUNNY = 4;
	
	// Total number of bunnies spawned
	private int bunniesSpawned = 0;

	//TrainingGUIScript gui;

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
		enemySpawnLoc = GameObject.Find("EnemyBunnySpawn");
		bunnies = new List<GameObject>();
		enemyBunnies = new List<GameObject>();
		time = Time.fixedTime;
		//gui = GameObject.Find("Terrain").GetComponent<TrainingGUIScript>();

		GlobalVars.isTesting = true;
		bunniesSpawned = 0;
		winVal = 0;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) return;
		
		// Spawn the bunnies!
		float t = Time.fixedTime;
		if (bunniesSpawned < Constants.NUM_BUNNIES && t > 0 && t % 1 == 0) {
			if (GlobalVars.bunnyBrains != null) {
				//Debug.Log("Spawing global bunny!");
				CreateBunny(GlobalVars.bunnyBrains[bunniesSpawned]);
			}
			else {
				CreateBunny();
			}
			
			CreateEnemyBunny();
			bunniesSpawned++;
		}
		
		carrotArray = (GameObject.FindGameObjectsWithTag("Carrot"));
		enemyArray = (GameObject.FindGameObjectsWithTag("EnemyBunny"));
		mudArray = (GameObject.FindGameObjectsWithTag("Mud"));
		
		// Fix null cases
		if(carrotArray == null)
			carrotArray = new GameObject[0];
		if (enemyArray == null)
			enemyArray = new GameObject[0];
		if (mudArray == null)
			mudArray = new GameObject[0];
					
		// Loops through all existing bunnies
		foreach(GameObject bunnyObj in bunnies) {
			// Use this to access anything associated with a specific bunny. To add NEAT stuff, 
			// just make public variables inside the BunnyControl class.
			BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();
			
			/*
			if (bunny.CalculateOnTargetSensor() == 1)
				bunny.FireCabbageGun();
			*/
			
			bunny.setInputArray(carrotArray, enemyArray, mudArray);
			
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
		CheckForWin();
	}
	
	void OnGUI() {
		if (GUI.Button (new Rect(Screen.width/2 + 440, Screen.height - 80, 100, 70), "Main Menu")) {
			Application.LoadLevel("MainMenu");
		}
		
		if (true) {
			if (winVal == 1) {
				GUI.Box (new Rect(Screen.width/2 - 100, Screen.height/2, 200, 50), "YOU WIN");
				Time.timeScale = 0;
			}
			else if (winVal == 2) {
				GUI.Box (new Rect(Screen.width/2 - 100, Screen.height/2, 200, 50), "YOU LOSE");
				Time.timeScale = 0;
			}
		}
	}
	
	// Spawns a new bunny in the bunny hole and adds it to the bunnies list
	void CreateBunny() {
		GameObject bunnyObj = (GameObject)Instantiate(bunnyPrefab, new Vector3(spawnLoc.transform.position.x, 0.8f, spawnLoc.transform.position.z), Quaternion.identity);
		
		// Create a brain for the bunny
		BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();		
		bunny.brain = new SimpleNeuralNetwork(Constants.INPUTS, Constants.OUTPUTS);
		bunny.birthday = Time.fixedTime;
		
		bunnies.Add(bunnyObj);
	}
	
	// Spawns a bunny with a specified brain.
	void CreateBunny(SimpleNeuralNetwork bunnyData) {
		GameObject bunnyObj = (GameObject)Instantiate(bunnyPrefab, new Vector3(spawnLoc.transform.position.x, 0.8f, spawnLoc.transform.position.z), Quaternion.identity);
		
		// Put specified brain in the bun
		BunnyControl bunny = bunnyObj.GetComponent<BunnyControl>();		
		bunny.isTesting = true;
		//Debug.Log (bunnyData.GetComponent<BunnyControl>().brain.InputCount);
		//bunny.brain = bunnyData.GetComponent<BunnyControl>().brain;
		bunny.brain = bunnyData;
		bunny.birthday = Time.fixedTime;
		
		
		bunnies.Add(bunnyObj);
		//Debug.Log(bunnies.Count);
	}

	// Spawns an enemy bunny
	void CreateEnemyBunny() {
		GameObject bunnyObj = (GameObject)Instantiate(enemyBunnyPrefab, new Vector3(enemySpawnLoc.transform.position.x, 0.8f, enemySpawnLoc.transform.position.z), Quaternion.identity);
		bunnyObj.transform.Rotate (0f, 180f, 0f);
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
	
	int CheckForWin() {
		foreach (GameObject obj in bunnies) {;
			//Debug.Log (Vector3.Distance(obj.transform.position, GameObject.Find("Carrot").transform.position));
			if (Vector3.Distance(obj.transform.position, GameObject.Find("Carrot").transform.position) < 2f) {
				winVal = 1;
			}
		}
		foreach (GameObject obj in enemyArray) {
			//Debug.Log (Vector3.Distance(obj.transform.position, GameObject.Find("EnemyCarrot").transform.position));
			if (Vector3.Distance(obj.transform.position, GameObject.Find("EnemyCarrot").transform.position) < 2f) {
				winVal = 2;
			}
		}

		return winVal;
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
