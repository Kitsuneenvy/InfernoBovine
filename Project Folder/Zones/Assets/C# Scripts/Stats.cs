using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;



public enum auraInfluence {none = 0, lawful, holy, sinister, magical}


[System.Serializable]
public class Stats : MonoBehaviour {

	public int cost = 10;
	public List<GameObject> createdObjects = new List<GameObject>();
	public bool structure = false;
	public bool moving = false;
	bool startmove = false;
	public float speed = 1;
	Vector2 targetLocation = Vector2.zero;
	Vector3 originalPosition = Vector3.zero;
	Vector3 nextWaypointPosition = Vector3.zero;
	int currentWaypoint = 0;

	Path currentPath;
	Seeker seekerComponent;

	public int auraInf = 0;

	// Use this for initialization
	void Start () {
		if(this.tag=="Unit"){
			seekerComponent = this.GetComponent<Seeker>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(this.tag=="Unit"){
			if(startmove==true){
				seekerComponent.StartPath(transform.position,targetLocation, OnPathComplete);
				startmove = false;
			}
			if(moving==true){
				transform.position = Vector3.MoveTowards(transform.position,nextWaypointPosition,speed*Time.deltaTime);

				if(Vector2.Distance(new Vector2(transform.position.x,transform.position.y),nextWaypointPosition)<0.05f){
					if(currentWaypoint == currentPath.vectorPath.Count-1){
						moving =false;
						return;
					} else {
					currentWaypoint++;
					nextWaypointPosition = currentPath.vectorPath[currentWaypoint];
					}
				} 
			}
		}
	}

	public void startMove(Vector2 newPosition){
		originalPosition = this.transform.position;
		targetLocation = newPosition;
		startmove = true;
	}

	void OnPathComplete(Path p){
		if(!p.error){
			currentWaypoint = 0;
			currentPath = p;
			moving=true;
			nextWaypointPosition = p.vectorPath[currentWaypoint];
		}
	}

	void OnSave(){
		if(this.GetComponent<Seeker>()!=null){
			this.GetComponent<Seeker>().enabled = false;
		}
	}

	void OnLoad(){
		if(this.GetComponent<Seeker>()!=null){
			this.GetComponent<Seeker>().enabled = true;
		}
	}
}
