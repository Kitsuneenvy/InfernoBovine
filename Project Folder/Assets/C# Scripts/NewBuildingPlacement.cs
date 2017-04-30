using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBuildingPlacement : MonoBehaviour {

	public bool placeable = true;
	public bool overlap = false;
	public bool auraOverlap = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(overlap==true||auraOverlap==true){
			placeable = false;
		} else {
			placeable = true;
		}
		if(this.tag!="Aura"){
			if(this.transform.childCount>0){
				auraOverlap = this.transform.GetChild(0).GetComponent<NewBuildingPlacement>().auraOverlap;
			}
			if(auraOverlap == true){
				this.GetComponent<SpriteRenderer>().color = Color.green;
			} 
			if(overlap == true){
				this.GetComponent<SpriteRenderer>().color = Color.magenta;
			}
			if(auraOverlap==true&&overlap==true){
				this.GetComponent<SpriteRenderer>().color = Color.red;
			}
			if(placeable == true){
				this.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D colliderObject){
		if(this.tag.Contains("Aura")){
			if(colliderObject.tag=="Aura"){
				auraOverlap = true;
			}
		} else if(tag.Contains("Structure")){
			if(colliderObject.tag.Contains("Structure")){
				overlap = true;
			}
		}
	}
	void OnTriggerStay2D(Collider2D colliderObject){
		if(this.tag.Contains("Aura")){
			if(colliderObject.tag=="Aura"){
				auraOverlap = true;
			}
		} else if(tag.Contains("Structure")){
			if(colliderObject.tag.Contains("Structure")){
				overlap = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D colliderObject){
		if(this.tag.Contains("Aura")){
			if(colliderObject.tag=="Aura"){
				auraOverlap = false;
			}
		} else if(tag.Contains("Structure")){
			if(colliderObject.tag.Contains("Structure")){
				overlap = false;
			}
		}
	}
}
