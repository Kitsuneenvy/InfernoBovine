using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach(GameObject mine in GameObject.FindGameObjectsWithTag("Mine")){
			mine.GetComponent<Mine>().addReturnPoint(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
