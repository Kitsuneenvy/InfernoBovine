using UnityEngine;
using UnityEngine.EventSystems;
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
	public bool newSelection = false;

	bool showUnitButton = false;


	Rect resourceMenuRect = new Rect(Screen.width/10,Screen.height/10,Screen.width/10,Screen.height/15);
	Rect selectionMenuRect = new Rect(Screen.width/10,Screen.height-Screen.height/5,Screen.width/5,Screen.height/6);
	Rect constructionMenuRect = new Rect(Screen.width-Screen.width/3-Screen.width/10,Screen.height-Screen.height/5,Screen.width/3,Screen.height/6);
	Rect constructionListRect = new Rect(0,0,0,0);

	List<Rect> guiRects = new List<Rect>();

	Vector2 originalSelectPosition = Vector2.zero;
	Vector2 endSelectPosition = Vector2.zero;

	int gold = 2000;

	public GUISkin defaultSkin;
	public Texture primary;
	public Texture secondary;
	public Texture support;
	public Texture2D structureSelect;
	public Texture2D unitSelect;

	RaycastHit2D rayHit;

	public GameObject selectionBox;
	public GameObject newBox = null;

	public List<GameObject> primaryBuildings = new List<GameObject>();
	public List<GameObject> selectedObjects = new List<GameObject>();
	List<Texture2D> selectedTextures = new List<Texture2D>();

	Vector2 movementWaypoint = Vector2.zero;
	bool moving = false;

	// Use this for initialization
	void Start () {
		constructionListRect = new Rect(constructionMenuRect.x,constructionMenuRect.y-Screen.height/3,constructionMenuRect.width,Screen.height/3);


	}
	
	// Update is called once per frame
	void Update () {
		if(primaryStructureListOpen||secondaryStructureListOpen||supportStructureListOpen){
			if(!guiRects.Contains(constructionListRect)){
				guiRects.Add(constructionListRect);
			}
		} else {
			if(guiRects.Contains(constructionListRect)){
				guiRects.Remove(constructionListRect);
			}
		}
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
			guiRects.Add(resourceMenuRect);
			guiRects.Add(selectionMenuRect);
			guiRects.Add(constructionMenuRect);

			if(placingObject==false){
				bool clickingGUI = false;

				foreach(Rect uiRect in guiRects){
					if(uiRect.Contains(new Vector3(Input.mousePosition.x,Screen.height-Input.mousePosition.y,0))){
						
						clickingGUI = true;
					}
				}
				if(clickingGUI){
					
				} else {
					selectingObjects = true;
					selectedObjects.Clear();
					selectedTextures.Clear();
					showUnitButton = false;
					newBox = GameObject.Instantiate(selectionBox);
					originalSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}
			} else {
				GameObject.Instantiate(selectedObjects[0],new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Quaternion.identity);
				placingObject = false;
				gold-=selectedObjects[0].GetComponent<Stats>().cost;
				selectedObjects.Clear();
				selectedTextures.Clear();
				this.GetComponent<AstarPath>().Scan();
			}
		}
		if(Input.GetKeyUp(KeyCode.Mouse0)){
			guiRects.Clear();
			selectingObjects = false;
			if(newBox!=null){
					endSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					newBox.transform.position =  originalSelectPosition + (endSelectPosition - originalSelectPosition)/2;
					newBox.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endSelectPosition.x - originalSelectPosition.x),Mathf.Abs(endSelectPosition.y - originalSelectPosition.y));
					newBox.GetComponent<SelectionBox>().enabledBox = true;
				}
		}

		if(Input.GetKeyDown(KeyCode.Mouse1)){
			movementWaypoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moving = true;
		}

		if(moving==true){
			if(selectedObjects.Count>0){
				foreach(GameObject selected in selectedObjects){
					if(selected.GetComponent<Stats>().structure==false){
						selected.GetComponent<Stats>().startMove(movementWaypoint);
					}
				}
			}
			moving = false;
		}
		if(newSelection==true){
			newSelectionFunction();
			newSelection = false;
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
		GUILayout.Label(gold.ToString());
	}
	void selectionMenuFunction(int id){
		GUILayout.BeginHorizontal();
			foreach(Texture2D selectTexture in selectedTextures){
				GUILayout.Label(selectTexture);
			}
		if(showUnitButton==true){
			int i = selectedObjects[0].GetComponent<Stats>().auraInf;
			if(GUILayout.Button(selectedObjects[0].GetComponent<Stats>().createdObjects[i].GetComponent<SpriteRenderer>().sprite.texture)){
				GameObject.Instantiate(selectedObjects[0].GetComponent<Stats>().createdObjects[i], (selectedObjects[0].transform.position+ Vector3.left),Quaternion.identity);
				gold-=selectedObjects[0].GetComponent<Stats>().createdObjects[i].GetComponent<Stats>().cost;
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
			selectedObjects.Clear();
			selectedTextures.Clear();
			placingObject = true;
			selectedObjects.Add(primaryBuildings[0]);
			selectedTextures.Add(selectedObjects[0].GetComponent<SpriteRenderer>().sprite.texture);
		}
		if(GUILayout.Button(primaryBuildings[1].GetComponent<SpriteRenderer>().sprite.texture)){
			selectedObjects.Clear();
			selectedTextures.Clear();
			placingObject = true;
			selectedObjects.Add(primaryBuildings[1]);
			selectedTextures.Add(selectedObjects[0].GetComponent<SpriteRenderer>().sprite.texture);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

	}

	void secondaryListFunction(int id){

	}

	void supportListFunction(int id){

	}

	public void newSelectionFunction(){
		if(selectedObjects.Count==1){
			selectedTextures.Add(selectedObjects[0].GetComponent<SpriteRenderer>().sprite.texture);
			if(selectedObjects[0].GetComponent<Stats>().createdObjects.Count>0){
				showUnitButton = true;
			} else {
				showUnitButton = false;
			}
		}
		if(selectedObjects.Count>1){
			int structureCount = 0;
			int unitCount = 0;
			foreach(GameObject selectedObject in selectedObjects){
				if(selectedObject.GetComponent<Stats>().structure == true){
					structureCount++;
				} else if (selectedObject.GetComponent<Stats>().structure == false){
					unitCount++;
				}
			
			}
			if(structureCount!=0){
				for(int i = 0; i<structureCount; i++){
					selectedTextures.Add(structureSelect);
				}
			}
			if(unitCount!=0){
				for(int j = 0; j<unitCount; j++){
					selectedTextures.Add(unitSelect);
				}
			}
		}
	}
}
