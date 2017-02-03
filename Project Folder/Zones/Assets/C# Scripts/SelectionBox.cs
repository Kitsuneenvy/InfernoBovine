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
			if(this.GetComponent<BoxCollider2D>().size.x<0.1f){
				this.GetComponent<BoxCollider2D>().size = new Vector2(0.1f,this.GetComponent<BoxCollider2D>().size.y);
			}
			if(this.GetComponent<BoxCollider2D>().size.y<0.1f){
				this.GetComponent<BoxCollider2D>().size = new Vector2(this.GetComponent<BoxCollider2D>().size.x,0.1f);
			}
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
	void OnTriggerEnter2D(Collider2D collider){
		if(enabledBox){
			if(!Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Contains(collider.gameObject)){
				Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Add(collider.gameObject);
			}
		}
	}

}
