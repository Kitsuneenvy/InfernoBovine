using UnityEngine;
using System.Collections;

public class SelectionBox : MonoBehaviour {


	public bool enabledBox = false;
	public bool rightClick = false;
	bool called = false;
	float lifespan = 0.05f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(enabledBox){

			if(this.GetComponent<BoxCollider2D>().size.x<0.15f){
				this.GetComponent<BoxCollider2D>().size = new Vector2(0.15f,this.GetComponent<BoxCollider2D>().size.y);
			}
			if(this.GetComponent<BoxCollider2D>().size.y<0.15f){
				this.GetComponent<BoxCollider2D>().size = new Vector2(this.GetComponent<BoxCollider2D>().size.x,0.15f);
			}
			lifespan-=Time.deltaTime;
			if(lifespan<=0){
				if(called==false){
					Camera.main.gameObject.GetComponent<GameGUI>().setTargetObject(null,false,false);
				}
				GameObject.Destroy(this.gameObject,0.001f);
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
		
		called = true;
		if(enabledBox){
			if(rightClick==false){
				if(collider.tag!="Aura"){
					if(!Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Contains(collider.gameObject)){
						Camera.main.gameObject.GetComponent<GameGUI>().selectedObjects.Add(collider.gameObject);
						Camera.main.gameObject.GetComponent<GameGUI>().newSelection = true;
					}
				}
			} else {
				if(collider.tag=="Unit"||collider.tag.Contains("Structure")){
					if(collider.GetComponent<Stats>().team!=1){
						Camera.main.gameObject.GetComponent<GameGUI>().setTargetObject(collider.gameObject,false,false);
					}
				} else if (collider.tag =="Mine"){
					Camera.main.gameObject.GetComponent<GameGUI>().setTargetObject(collider.gameObject,false,true);
				}
			}
		}
	}

}
