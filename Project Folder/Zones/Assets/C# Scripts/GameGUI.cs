using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUI : MonoBehaviour {

	bool resourceMenuOpen = true;
	bool selectionMenuOpen = true;
	bool constructionMenuOpen = true;

	bool primaryStructureListOpen = false;
	bool secondaryStructureListOpen = false;
	bool supportStructureListOpen = false;

	bool placingObject = false;

	Rect resourceMenuRect = new Rect(Screen.width/10,Screen.height/10,Screen.width/10,Screen.height/15);
	Rect selectionMenuRect = new Rect(Screen.width/10,Screen.height-Screen.height/5,Screen.width/5,Screen.height/6);
	Rect constructionMenuRect = new Rect(Screen.width-Screen.width/3-Screen.width/10,Screen.height-Screen.height/5,Screen.width/3,Screen.height/6);
	Rect constructionListRect = new Rect(0,0,0,0);

	public GUISkin defaultSkin;
	public Texture primary;
	public Texture secondary;
	public Texture support;

	RaycastHit2D rayHit;

	GameObject selectedObject = null;

	public List<GameObject> PrimaryBuildings = new List<GameObject>();

	// Use this for initialization
	void Start () {
		constructionListRect = new Rect(constructionMenuRect.x,constructionMenuRect.y-Screen.height/3,constructionMenuRect.width,Screen.height/3);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = false;
			supportStructureListOpen = false;
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			primaryStructureListOpen = true;
			secondaryStructureListOpen = false;
			supportStructureListOpen = false;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = true;
			supportStructureListOpen = false;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = false;
			supportStructureListOpen = true;
		}
		if(Input.GetKeyDown(KeyCode.Mouse0)){
			if(placingObject==false){
				rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
				if(rayHit.transform == null){
					selectedObject = null;
				} else {
					selectedObject = rayHit.transform.gameObject;
				}
			} else {
				Debug.Log(selectedObject.ToString());
				GameObject.Instantiate(selectedObject,new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Quaternion.identity);
				placingObject = false;
			}
		}
	}

	void OnGUI(){
		GUI.skin = defaultSkin;
		if(resourceMenuOpen){
			resourceMenuRect = GUI.Window(0,resourceMenuRect,resourceMenuFunction,"");
		}


		if(selectionMenuOpen){
			selectionMenuRect = GUI.Window(1,selectionMenuRect,selectionMenuFunction,"");
		}


		if(constructionMenuOpen){
			constructionMenuRect = GUI.Window(2,constructionMenuRect,constructionMenuFunction,"");
		}

		if(primaryStructureListOpen){
			constructionListRect = GUI.Window(3,constructionListRect,primaryListFunction,"");
		}

		if(secondaryStructureListOpen){
			constructionListRect = GUI.Window(3,constructionListRect,secondaryListFunction,"");
		}

		if(supportStructureListOpen){
			constructionListRect = GUI.Window(3,constructionListRect,supportListFunction,"");
		}


	}

	void resourceMenuFunction(int id){
		GUILayout.Label("GOLD");
	}
	void selectionMenuFunction(int id){
		GUILayout.BeginHorizontal();
		if(selectedObject!=null){
			GUILayout.Label(selectedObject.GetComponent<SpriteRenderer>().sprite.texture);
			GUILayout.Label("Unit image here");
			GUILayout.Label("and here");
		}
		GUILayout.EndHorizontal();
	}
	void constructionMenuFunction(int id){
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(primary)){
			primaryStructureListOpen = !primaryStructureListOpen;
			secondaryStructureListOpen = false;
			supportStructureListOpen = false;
		}
		if(GUILayout.Button(secondary)){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = !secondaryStructureListOpen;
			supportStructureListOpen = false;
		}
		if(GUILayout.Button(support)){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = false;
			supportStructureListOpen = !supportStructureListOpen;
		}
		GUILayout.EndHorizontal();
	}

	void primaryListFunction(int id){
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(PrimaryBuildings[0].GetComponent<SpriteRenderer>().sprite.texture)){
			placingObject = true;
			selectedObject = PrimaryBuildings[0];
		}
		if(GUILayout.Button(PrimaryBuildings[1].GetComponent<SpriteRenderer>().sprite.texture)){
			placingObject = true;
			selectedObject = PrimaryBuildings[1];
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

	}

	void secondaryListFunction(int id){

	}

	void supportListFunction(int id){

	}
}
