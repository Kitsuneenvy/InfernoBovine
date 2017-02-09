using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats : MonoBehaviour {

	public int cost = 10;
	public List<GameObject> createdObjects = new List<GameObject>();
	public bool structure = false;
	public bool moving = false;
	public float speed = 1;
	Vector2 targetLocation = Vector2.zero;
	Vector3 originalPosition = Vector3.zero;
	float distanceToTarget = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(moving==true){
			if(Vector2.Distance(new Vector2(transform.position.x,transform.position.y),targetLocation)>0.05f){
				transform.position = Vector3.MoveTowards(transform.position,targetLocation,speed*Time.deltaTime);
			} else {
				moving =false;
			}
		}
	
	}

	public void startMove(Vector2 newPosition){
		moving = true;
		originalPosition = this.transform.position;
		targetLocation = newPosition;
		distanceToTarget = Vector3.Distance(originalPosition, targetLocation);
	}
}
