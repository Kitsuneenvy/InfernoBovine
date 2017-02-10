using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

	public int auraRange = 5;

	// Use this for initialization
	void Start () {
		this.GetComponent<CircleCollider2D>().radius = auraRange;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AuraEffects(){
		
	}
}
