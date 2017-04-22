using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AuraNetwork : NetworkBehaviour {

	public int auraRange = 5;

	// Use this for initialization
	void Start () {
		this.GetComponent<CircleCollider2D>().radius = auraRange;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer){
			return;
		}
	
	}

	void AuraEffects(){
		
	}
}
