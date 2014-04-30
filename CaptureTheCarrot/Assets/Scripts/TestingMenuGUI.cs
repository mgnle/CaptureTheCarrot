using UnityEngine;
using System.Collections;

public class TestingMenuGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if (GUI.Button (new Rect(Screen.width/20, Screen.height/6, 150, 100), "Load AI")) {
		
		}
		if (GUI.Button (new Rect(Screen.width/20, Screen.height/6 + 120, 150, 100), "Fight!")) {
		
		}
	}
}
