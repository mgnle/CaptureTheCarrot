using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if (GUI.Button (new Rect(Screen.width/20, Screen.height/6, 150, 100), "Train")) {
			Application.LoadLevel("TrainingScene");
		}
		/*
		if (GUI.Button (new Rect(Screen.width/20, Screen.height/6 + 120, 150, 100), "Test")) {
			Application.LoadLevel("TestingMenu");
		}
		*/
	}
}
