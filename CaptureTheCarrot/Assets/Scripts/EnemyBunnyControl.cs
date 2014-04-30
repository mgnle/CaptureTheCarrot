using UnityEngine;
using System.Collections;

public class EnemyBunnyControl : MonoBehaviour {

	public Vector3 moveVector;
	public int rotationAngle;
	public float moveDistance;
	
	// Use this for initialization
	void Start () {
		rotationAngle = 45;
		moveDistance = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
				
		// Randomly move bunnies, with a bias toward moving straight
		int moveDirIndex = Random.Range(0, 20);
		if (moveDirIndex == 0) {
			MoveLeft();
		}
		else if (moveDirIndex == 1) {
			MoveRight();
		}
		else if (moveDirIndex == 2) {
			StandStill();
		}
		else {
			MoveStraight();
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
	
	/* Bunny doesn't move */
	public void StandStill() {
		
	}
}
