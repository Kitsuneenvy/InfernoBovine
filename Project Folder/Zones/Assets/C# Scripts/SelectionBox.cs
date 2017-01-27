using UnityEngine;
using System.Collections;

public class SelectionBox : MonoBehaviour {


	public bool enabledBox = false;
	float lifespan = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(enabledBox){
			lifespan-=Time.deltaTime;
			if(lifespan<=0){
				GameObject.Destroy(this.gameObject,0.01f);
			}
		}
	}

	void OnTriggerStay2D(Collider2D collider){
		if(enabledBox){
			if(!Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Contains(collider.gameObject)){
				Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Add(collider.gameObject);
			}
		}
	}

}
