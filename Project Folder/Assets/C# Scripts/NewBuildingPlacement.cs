using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBuildingPlacement : MonoBehaviour {

	bool placeable = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D colliderObject){
		if(collider.tag.Contains("structure")||colliderObject.tag.Contains("Aura")){
			placeable = false;
			this.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
	void OnTriggerStay2D(Collider2D colliderObject){
		if(collider.tag.Contains("structure")||colliderObject.tag.Contains("Aura")){
			placeable = false;
			this.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
	void OnTriggerExit2D(Collider2D colliderObject){
		if(collider.tag.Contains("structure")||colliderObject.tag.Contains("Aura")){
			placeable = true;
			this.GetComponent<SpriteRenderer>().color = Color.blue;
		}
	}
}
