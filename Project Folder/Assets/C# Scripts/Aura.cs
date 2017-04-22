using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

	public int auraRange = 5;
	public auraInfluence auraType;

	// Use this for initialization
	void Start () {
		this.GetComponent<CircleCollider2D>().radius = auraRange;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AuraEffects(){
		
	}

	void OnTriggerStay2D(Collider2D triggeringObject){
		if(triggeringObject.tag == "SecondaryStructure"){
			triggeringObject.gameObject.GetComponent<Stats>().auraInf = (int)auraType;
		}	
	}
}
