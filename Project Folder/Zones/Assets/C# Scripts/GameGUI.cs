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
	bool selectingObjects = false;

	Rect resourceMenuRect = new Rect(Screen.width/10,Screen.height/10,Screen.width/10,Screen.height/15);
	Rect selectionMenuRect = new Rect(Screen.width/10,Screen.height-Screen.height/5,Screen.width/5,Screen.height/6);
	Rect constructionMenuRect = new Rect(Screen.width-Screen.width/3-Screen.width/10,Screen.height-Screen.height/5,Screen.width/3,Screen.height/6);
	Rect constructionListRect = new Rect(0,0,0,0);

	Vector2 originalSelectPosition = Vector2.zero;
	Vector2 endSelectPosition = Vector2.zero;

	public GUISkin defaultSkin;
	public Texture primary;
	public Texture secondary;
	public Texture support;

	RaycastHit2D rayHit;

	public GameObject selectionBox;
	public GameObject newBox = null;

	public List<GameObject> primaryBuildings = new List<GameObject>();
	public List<GameObject> selectedObjects = new List<GameObject>();
	List<Texture2D> selectedTextures = new List<Texture2D>();

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
				selectingObjects = true;
				selectedObjects.Clear();
				newBox = GameObject.Instantiate(selectionBox);
				originalSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//				rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
//				if(rayHit.transform == null){
//					selectedObjects.Clear();
//				} else {
//					selectedObjects.Add(rayHit.transform.gameObject);
//				}
			} else {
				GameObject.Instantiate(selectedObjects[0],new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Quaternion.identity);
				placingObject = false;
			}
		}
		if(Input.GetKeyUp(KeyCode.Mouse0)){
			selectingObjects = false;
			if(newBox!=null){
				endSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newBox.transform.position =  originalSelectPosition + (endSelectPosition - originalSelectPosition)/2;
				newBox.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endSelectPosition.x - originalSelectPosition.x),Mathf.Abs(endSelectPosition.y - originalSelectPosition.y));
				newBox.GetComponent<SelectionBox>().enabledBox = true;
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
		if(selectedObjects.Count==1){
			GUILayout.Label(selectedObjects[0].GetComponent<SpriteRenderer>().sprite.texture);
			GUILayout.Label("Unit image here");
			GUILayout.Label("and here");
		}
		if(selectedObjects.Count>1){
			int i = 0;
			foreach(GameObject selectedObject in selectedObjects){
				selectedTextures.Add(selectedObjects[i].GetComponent<SpriteRenderer>().sprite.texture);
				GUILayout.Label(selectedTextures[i]);
				i++;
			}

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
		if(GUILayout.Button(primaryBuildings[0].GetComponent<SpriteRenderer>().sprite.texture)){
			placingObject = true;
			selectedObjects.Add(primaryBuildings[0]);
		}
		if(GUILayout.Button(primaryBuildings[1].GetComponent<SpriteRenderer>().sprite.texture)){
			placingObject = true;
			selectedObjects.Add(primaryBuildings[1]);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

	}

	void secondaryListFunction(int id){

	}

	void supportListFunction(int id){

	}
}
