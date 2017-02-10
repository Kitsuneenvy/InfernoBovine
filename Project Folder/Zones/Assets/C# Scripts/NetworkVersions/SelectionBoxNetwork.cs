using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SelectionBoxNetwork : NetworkBehaviour {


	public bool enabledBox = false;
	float lifespan = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer){
			return;
		}
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

//	void OnTriggerStay2D(Collider2D collider){
//		if(enabledBox){
//			if(!Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Contains(collider.gameObject)){
//				Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Add(collider.gameObject);
//				Camera.main.gameObject.GetComponent<GameGUI>().newSelection = true;
//			}
//		}
//	}
	void OnTriggerEnter2D(Collider2D collider){
		if(enabledBox){
			if(!Camera.main.gameObject.GetComponent<GameGUINetwork>().selectedObjects.Contains(collider.gameObject)){
				Camera.main.gameObject.GetComponent<GameGUINetwork>().selectedObjects.Add(collider.gameObject);
				Camera.main.gameObject.GetComponent<GameGUINetwork>().newSelection = true;
			}
		}
	}

}
