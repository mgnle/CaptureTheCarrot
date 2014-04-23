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

	public Item selectedItem;
	int numItemTypes = 4;

	public Texture carrotIcon;
	public Texture bunnyIcon;
	public Texture enemyBunnyIcon;
	public Texture mudIcon;

	public int boxDim = 80;

	GUIText selectionLabel;
	string selectionText = "";

	Texture[] icons;

	// Use this for initialization
	void Start () {
		selectedItem = (Item)0;
		icons = new Texture[] {carrotIcon, bunnyIcon, enemyBunnyIcon, mudIcon};
		selectionLabel = GameObject.Find ("Selection Label").guiText;
	}

	void OnGUI () {
		GUI.enabled = true;
		GUI.backgroundColor = Color.red;
		selectedItem = (Item) (GUI.SelectionGrid(new Rect(Screen.width/2 - 60*(numItemTypes/2), Screen.height - 80, boxDim*numItemTypes, boxDim), (int)selectedItem, icons, numItemTypes, "button"));

		switch(selectedItem)
		{
		case Item.Bunny:
			selectionText = "Bunny";
			break;
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

		selectionLabel.text = selectionText;
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
			selectedItem = (Item)numItemTypes;
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
