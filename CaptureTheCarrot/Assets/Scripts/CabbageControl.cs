using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class CabbageControl : MonoBehaviour {
	
	public float speed = 300f;
	
	public CharacterController control;
	
	public float life;
	
	public GameObject explosionParticles;
	
	public GameObject carrot;
	
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		control.Move(transform.forward * speed * Time.deltaTime);
		CollisionDetection();
		life+=Time.deltaTime;
		if(life >=5) {
			Destroy(gameObject);
			GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		if(!hit.collider.name.Equals("Terrain")){
			GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
			Destroy(gameObject);
		}
	}
	
	
	public void CollisionDetection() {
		if(Vector3.Distance(control.transform.position,carrot.transform.position)<2){
			//Debug.Log("hit something");
			Destroy(gameObject);
		}	
	}
}
