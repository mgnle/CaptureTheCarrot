using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class TrainingScript : MonoBehaviour {

	// Template for prefabs
	public GameObject bunnyPrefab;
	public GameObject enemyBunnyPrefab;
	public GameObject carrotPrefab;
	public GameObject mudPrefab;
	
	// Default bunny spawn location
	GameObject spawnLoc;

	// List of all our bunny objects
	private List<GameObject> bunnies;
	
	// List of all species
	private List<Species> species;

	// Holds the seconds since the start of the game
	private float time;

	// The seconds till we replace the worst bunny
	private int SEC_TIL_REMOVE_BUNNY = 4;
	
	// Total number of bunnies spawned
	private int bunniesSpawned = 0;

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
		species = new List<Species>();
		time = Time.fixedTime;
		gui = GameObject.Find("Terrain").GetComponent<TrainingGUIScript>();

		bunniesSpawned = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Spawn the bunnies!
		float t = Time.fixedTime;
		if (bunniesSpawned < Constants.NUM_BUNNIES && t > 0 && t % 1 == 0) {
			CreateBunny();
			bunniesSpawned++;
		}
				
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
			
			bunny.setSliders(gui.carrotProximityReward ,gui.enemyAttackReward);
			
			//if (bunny.CalculateOnTargetSensor() == 1)
				//bunny.FireCabbageGun();

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
	
	void OnGUI() {
		// Check for Load/Save/Reset button
		GUI.enabled = true;
		if (GUI.Button (new Rect(Screen.width/2 + 200, Screen.height - 80, 100, 70), "Save")) {
			
		}
		if (GUI.Button (new Rect(Screen.width/2 + 320, Screen.height - 80, 100, 70), "Load")) {
			
		}
		if (GUI.Button (new Rect(Screen.width/2 + 440, Screen.height - 80, 100, 70), "Fight!")) {
			Application.LoadLevel("TestingMenu");
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
		
		AssignBunnyToSpecies(bunny);
	}

	// Spawns an enemy bunny
	void CreateEnemyBunny(Ray ray) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit)) {
			GameObject bunnyObj = (GameObject)Instantiate(enemyBunnyPrefab, new Vector3(hit.point.x, 0.8f, hit.point.z), Quaternion.identity);
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
		
			// Choose worst agent
			BunnyControl worstBunny = RemoveWorstBunny();
			if(worstBunny == null) return;
			
			// Choose the best parent species
			Species parentSpecies = ChooseParentSpecies();

			BunnyControl bestBunny = null;
			BunnyControl secondBestBunny = null;			
			parentSpecies.ChooseParents(out bestBunny, out secondBestBunny);
			
			// Create a new brain from the best parents
			SimpleNeuralNetwork newBrain = new SimpleNeuralNetwork(bestBunny.brain, secondBestBunny.brain);
			
			// Replace the old brain with the new one
			worstBunny.brain = newBrain;
			worstBunny.birthday = Time.fixedTime;
			
			// Reassign agent to species
			AssignBunnyToSpecies(worstBunny);
			
			// TODO: Reassign all agents to species based on a per-species dynamic compatability threshold
			
			//RespawnBunny(worstBunny);
		}			
	}
	
	public void AssignBunnyToSpecies(BunnyControl bunny)
	{
		// Assign bunny to species
		bool assigned = false;
		foreach(Species s in species)
		{
			foreach(BunnyControl member in s.GetMembers())
			{
				int disjoint = 0;
				int N = 1;
                double weightedAverage = 0;
                
                bunny.brain.DistanceFrom(member.brain, out disjoint, out N, out weightedAverage);
                
                float d = (((float)disjoint*Constants.DISJOINT_MULTIPLIER)/(float)N) + ((float)weightedAverage*Constants.WEIGHT_AVERAGE_MULTIPLIER);
                if (d <= Constants.COMPATABILITY_THRESHOLD)
                {
                	s.Add(bunny);
                	assigned = true;
                	break;
                }
			
			}	
		}
		if(!assigned)
		{
			Species newSpecies = new Species();
			newSpecies.Add(bunny);
		}
    }
	
	public BunnyControl RemoveWorstBunny()
	{
		float minFitness = float.MaxValue;
		BunnyControl minBunny = null;
		Species minBunnySpecies = null;
	
		// Find bunny with lowest adjusted fitness
		foreach(Species s in species)
		{
			Dictionary<BunnyControl, float> adjustedFitnessMap = s.CalculateAdjustedFitness();
			foreach(BunnyControl member in adjustedFitnessMap.Keys)
			{
				if (HasBeenAliveLongEnough(member) && adjustedFitnessMap[member] < minFitness)
				{
					minFitness = adjustedFitnessMap[member];
					minBunny = member;
					minBunnySpecies = s;
				}
			}
		}
		
		if(minBunny != null)
		{
			// Remove bunny from it's species
			minBunnySpecies.Remove(minBunny);
		}
				
		return minBunny;
	}
	
	public Species ChooseParentSpecies()
	{
		float prob = 0;
		System.Random gen = new System.Random();
		double i = gen.NextDouble();
		
		// Calculate total fitness of population
		float totalFitness = 0f;
		foreach(Species s in species)
		{
			totalFitness += s.GetAverageFitness();
		}
		
		foreach(Species s in species)
		{
			float prob2 = (s.GetAverageFitness()/totalFitness);
			if(i >= prob && i < prob2)
			{
				return s;
			}
			prob = prob2;
        }
        return null;
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
