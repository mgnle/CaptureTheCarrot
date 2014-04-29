using UnityEngine;
using System.Collections;

public class TrainingGUIScript : MonoBehaviour {

	public enum Item
	{
		Carrot,
		Bunny,
		EnemyBunny,
		Mud
	}

	// Item to be placed
	public Item selectedItem;
	int numItemTypes = 3;
	
	// Current values of sliders
	public float carrotProximityReward;
	public float carrotProximityDistance;
	public float enemyProximityReward;
	public float enemyProximityDistance;
	public float mudProximityReward;
	public float mudProximityDistance;
	public float enemyAttackReward;

	public Texture carrotIcon;
	public Texture enemyBunnyIcon;
	public Texture mudIcon;

	public int boxDim = 80;

	string selectionText = "";

	Texture[] icons;

	// Use this for initialization
	void Start () {
		selectedItem = (Item)0;
		icons = new Texture[] {carrotIcon, enemyBunnyIcon, mudIcon};
		carrotProximityReward = 0.0f;
		carrotProximityDistance = 0.0f;
		enemyProximityReward = 0.0f;
		enemyProximityDistance = 0.0f;
		mudProximityReward = 0.0f;
		mudProximityDistance = 0.0f;
		enemyAttackReward = 0.0f;
	}

	void OnGUI () {
		GUI.enabled = true;
		GUI.backgroundColor = Color.red;
		selectedItem = (Item) (GUI.SelectionGrid(new Rect(Screen.width/2 - 60*(numItemTypes/2) - boxDim/2, Screen.height - 80, boxDim*numItemTypes, boxDim), (int)selectedItem, icons, numItemTypes, "button"));

		// Item selector
		switch(selectedItem)
		{
		case Item.EnemyBunny:
			selectionText = "Enemy Bunny";
			break;
		case Item.Carrot:
			selectionText = "Carrot";
			break;
		case Item.Mud:
			selectionText = "Mud Pit";
			break;
		}

		GUI.Box (new Rect(Screen.width/2 - 45, Screen.height - 110, 120, 25), selectionText);
		
		// Sliders
		enemyAttackReward = GUI.VerticalSlider(new Rect(60, Screen.height - 80, 20, 70), enemyAttackReward, 100f, -100f);
		GUI.Box (new Rect(10, Screen.height - 110, 120, 25), "Attack Enemy");
		GUI.Box (new Rect(25, Screen.height - 55, 30, 25), Mathf.Floor(enemyAttackReward).ToString());
		carrotProximityReward = GUI.VerticalSlider(new Rect(190, Screen.height - 80, 20, 70), carrotProximityReward, 100f, -100f);
		GUI.Box (new Rect(140, Screen.height - 110, 120, 25), "Approach Carrot");
		GUI.Box (new Rect(155, Screen.height - 55, 30, 25), Mathf.Floor(carrotProximityReward).ToString());
		mudProximityReward = GUI.VerticalSlider(new Rect(320, Screen.height - 80, 20, 70), mudProximityReward, 100f, -100f);
		GUI.Box (new Rect(270, Screen.height - 110, 120, 25), "Approach Mud");
		GUI.Box (new Rect(285, Screen.height - 55, 30, 25), Mathf.Floor(mudProximityReward).ToString());
		enemyProximityReward = GUI.VerticalSlider(new Rect(450, Screen.height - 80, 20, 70), enemyProximityReward, 100f, -100f);
		GUI.Box (new Rect(400, Screen.height - 110, 120, 25), "Approach Enemy");
		GUI.Box (new Rect(415, Screen.height - 55, 30, 25), Mathf.Floor(enemyProximityReward).ToString());
		
	}

	public void IncrementSelection() {
		if ((int)selectedItem < numItemTypes)
			selectedItem++;
		else
			selectedItem = (Item)0;
	}

	public void DecrementSelection() {
		if ((int)selectedItem > 0)
			selectedItem--;
		else
			selectedItem = (Item)numItemTypes - 1;
	}

	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
			DecrementSelection();
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
			IncrementSelection();
		*/
	}
}
