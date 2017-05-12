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

	bool tooltipDisplay = false;
	bool buildingHovered = false;
	string tooltipLine1 = "1";
	string tooltipLine2 = "2";

	bool placingObject = false;
	bool selectingObjects = false;
	public bool newSelection = false;

	bool showUnitButton = false;
	bool workerSelected = false;

	List<int> workerIndices = new List<int>();


	Rect resourceMenuRect = new Rect(Screen.width/10,Screen.height/10,Screen.width/10,Screen.height/15);
	Rect selectionMenuRect = new Rect(Screen.width/10,Screen.height-Screen.height/5,Screen.width/5,Screen.height/6);
	Rect constructionMenuRect = new Rect(Screen.width-Screen.width/3-Screen.width/10,Screen.height-Screen.height/5,Screen.width/3,Screen.height/6);
	Rect constructionListRect = new Rect(0,0,0,0);
	Rect tooltipRect = new Rect(0,0,Screen.width/6,Screen.height/8);

	List<Rect> guiRects = new List<Rect>();

	Vector2 originalSelectPosition = Vector2.zero;
	Vector2 endSelectPosition = Vector2.zero;

	public int[] gold = new int[5];

	public GUISkin defaultSkin;
	public Texture primary;
	public Texture secondary;
	public Texture support;
	public Texture2D structureSelect;
	public Texture2D unitSelect;

	RaycastHit2D rayHit;

	public GameObject selectionBox;
	public GameObject newBox = null;
	public GameObject targetObject = null;

	public List<GameObject> primaryBuildings = new List<GameObject>();
	public List<string> primaryDescriptions = new List<string>();
	public List<GameObject> selectedObjects = new List<GameObject>();
	List<Texture2D> selectedTextures = new List<Texture2D>();

	Vector2 movementWaypoint = Vector2.zero;
	bool moving = false;

	GameObject objectToPlace = null;
	GameObject objectPlacePreview = null;
	GameObject newBuilding = null;

	// Use this for initialization
	void Start () {
		gold = new int[5];
		gold[1] = 2000;
		constructionListRect = new Rect(constructionMenuRect.x,constructionMenuRect.y-Screen.height/3,constructionMenuRect.width,Screen.height/3);


	}
	
	// Update is called once per frame
	void Update () {
		if(buildingHovered == true){
			tooltipDisplay = true;
		} else {
			tooltipDisplay = false;
		}
		buildingHovered = false;
		if(objectToPlace!=null&&objectPlacePreview==null){
			objectPlacePreview = GameObject.Instantiate(objectToPlace,new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Quaternion.identity) as GameObject;
			objectPlacePreview.GetComponent<PolygonCollider2D>().isTrigger = true;
	//		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(objectPlacePreview, "Assets/C# Scripts/GameGUI.cs (87,4)", "NewBuildingPlacement");
			objectPlacePreview.AddComponent<NewBuildingPlacement>();
			if(objectPlacePreview.transform.childCount>0){
				objectPlacePreview.transform.GetChild(0).GetComponent<NewBuildingPlacement>().enabled = true;
				objectPlacePreview.transform.GetChild(0).gameObject.layer = 12;
			}
		} else if(objectToPlace ==null){
			GameObject.DestroyImmediate(objectPlacePreview);
		}
		if(objectPlacePreview!=null){
			objectPlacePreview.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		}
		if(!workerSelected){
			primaryStructureListOpen = false;
			secondaryStructureListOpen = false;
			supportStructureListOpen = false;
		}
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
				bool clickingGUI = checkClickOnGui();

				if(clickingGUI){
					
				} else {
					selectingObjects = true;
					selectedObjects.Clear();
					selectedTextures.Clear();
					workerSelected = false;
					showUnitButton = false;
					newBox = GameObject.Instantiate(selectionBox);
					originalSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}
			} else {
				if(objectPlacePreview.GetComponent<NewBuildingPlacement>().placeable == true){
					newBuilding = GameObject.Instantiate(objectToPlace,new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y),Quaternion.identity) as GameObject;
					newBuilding.GetComponent<Stats>().health = 1;
					newBuilding.GetComponent<Stats>().finishedBuilding = false;
					placingObject = false;
					gold[selectedObjects[0].GetComponent<Stats>().team]-=objectToPlace.GetComponent<Stats>().cost;
					objectToPlace = null;
					if(newBuilding.transform.childCount>0){
						newBuilding.transform.GetChild(0).gameObject.layer = 10;
						newBuilding.transform.GetChild(0).GetComponent<NewBuildingPlacement>().enabled = false;
					}
					if(workerSelected){
						setTargetObject(newBuilding,true,false);
					}
					newBuilding = null;
					this.GetComponent<AstarPath>().Scan();
				}
			}
		}
		if(Input.GetKeyUp(KeyCode.Mouse0)){
			guiRects.Clear();
			selectingObjects = false;
			if(newBox!=null){
					endSelectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					newBox.transform.position =  originalSelectPosition + (endSelectPosition - originalSelectPosition)/2;
					newBox.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endSelectPosition.x - originalSelectPosition.x),Mathf.Abs(endSelectPosition.y - originalSelectPosition.y));
					newBox.GetComponent<SelectionBox>().rightClick = false;	
					newBox.GetComponent<SelectionBox>().enabledBox = true;
				}
		}

		if(Input.GetKeyDown(KeyCode.Mouse1)){
			//If you right click on an enemy
			targetObject = null;
			newBox = GameObject.Instantiate(selectionBox);
			newBox.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			newBox.GetComponent<SelectionBox>().rightClick = true;	
			newBox.GetComponent<SelectionBox>().enabledBox = true;

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


		if(workerSelected){
			constructionMenuRect = GUI.Window(2,constructionMenuRect,constructionMenuFunction,"");
		}

		if(primaryStructureListOpen&&workerSelected){
			constructionListRect = GUI.Window(3,constructionListRect,primaryListFunction,"");
		}

		if(secondaryStructureListOpen&&workerSelected){
			constructionListRect = GUI.Window(3,constructionListRect,secondaryListFunction,"");
		}

		if(supportStructureListOpen&&workerSelected){
			constructionListRect = GUI.Window(3,constructionListRect,supportListFunction,"");
		}
		if(tooltipDisplay){
			tooltipRect = GUI.Window(4,tooltipRect,tooltipFunction,"");
		}


	}

	void resourceMenuFunction(int id){
		GUILayout.Label(gold[1].ToString());
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
				gold[selectedObjects[0].GetComponent<Stats>().team]-=selectedObjects[0].GetComponent<Stats>().createdObjects[i].GetComponent<Stats>().cost;
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
		foreach(GameObject building in primaryBuildings){
			if(GUILayout.Button(building.GetComponent<SpriteRenderer>().sprite.texture)){
				objectToPlace = building;
				placingObject = true;
			}
			if(Event.current.type == EventType.Repaint&&GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)){
				tooltipRect.x = Input.mousePosition.x+15;
				tooltipRect.y = Screen.height - Input.mousePosition.y+15;
				foreach(string testString in primaryDescriptions){
					if(testString.Contains(building.name)){
						tooltipLine1 = testString;
						tooltipLine1 = tooltipLine1.Replace("\\n","" + System.Environment.NewLine);
					}
				}
				buildingHovered = true;
			} 
		}
		GUILayout.EndHorizontal();

		if(GUILayout.Button("Save Game")){
			Camera.main.GetComponent<SaveLoadUtility>().SaveGame("sses");
		}



		if(GUILayout.Button("Load Game")){
			selectedTextures.Clear();
			selectedObjects.Clear();
			workerSelected = false;
			showUnitButton = false;
			Camera.main.GetComponent<SaveLoadUtility>().LoadGame("sses");
		}
		GUILayout.EndVertical();

	}

	void secondaryListFunction(int id){

	}

	void supportListFunction(int id){

	}

	public void newSelectionFunction(){
		if(selectedObjects.Count==1){
			if(selectedObjects[0].GetComponent<Stats>().worker == true){
				workerSelected = true;
				workerIndices.Add(selectedObjects.IndexOf(selectedObjects[0]));
			} else {
				workerSelected = false;
			}
			selectedTextures.Add(selectedObjects[0].GetComponent<SpriteRenderer>().sprite.texture);
			if(selectedObjects[0].GetComponent<Stats>().createdObjects.Count>0&&selectedObjects[0].GetComponent<Stats>().finishedBuilding==true){
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
				if(selectedObject.GetComponent<Stats>().worker == true){
					workerIndices.Add(selectedObjects.IndexOf(selectedObject));
					workerSelected = true;
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

	bool checkClickOnGui(){
		bool guiClick = false;
		foreach(Rect uiRect in guiRects){
			if(uiRect.Contains(new Vector3(Input.mousePosition.x,Screen.height-Input.mousePosition.y,0))){
				guiClick = true;
			}
		}
		return guiClick;
	}

	public void setTargetObject(GameObject newTarget, bool workerAssign,bool assignMine){
		if(assignMine){
			foreach(int index in workerIndices){
				newTarget.GetComponent<Mine>().assignWorker(selectedObjects[index]);
				selectedObjects[index].GetComponent<Stats>().assignedMine = newTarget;
				selectedObjects[index].GetComponent<Stats>().attackTarget = newTarget;
				selectedObjects[index].GetComponent<Stats>().startMove();
			}
		} else {
			foreach(int index in workerIndices){
				if(selectedObjects[index].GetComponent<Stats>().assignedMine!=null){
					if(selectedObjects[index].activeSelf == false){
						selectedObjects[index].SetActive(true);
						selectedObjects[index].GetComponent<Stats>().assignedMine.GetComponent<Mine>().workerWorking = false;
					}
					selectedObjects[index].GetComponent<Stats>().assignedMine.GetComponent<Mine>().removeWorker(selectedObjects[index]);
					selectedObjects[index].GetComponent<Stats>().assignedMine = null;
				}
			}
		}
		if(workerAssign){
			foreach(int index in workerIndices){
				selectedObjects[index].GetComponent<Stats>().attackTarget = newTarget;
				selectedObjects[index].GetComponent<Stats>().startMove();
			}
		} else {
			if(newTarget == null){
				if(selectedObjects.Count>0){
					foreach(GameObject selected in selectedObjects){
						if(selected.GetComponent<Stats>().structure==false){
							selected.GetComponent<Stats>().attackTarget = null;
						}
					}
				}
				movementWaypoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				moving = true;
			} else {
				targetObject = newTarget;
				if(selectedObjects.Count>0){
					foreach(GameObject selected in selectedObjects){
						if(selected.GetComponent<Stats>().structure==false){
							selected.GetComponent<Stats>().attackTarget = targetObject;
							selected.GetComponent<Stats>().startMove();
						}
					}
				}
			}
		}
	}

	void tooltipFunction(int id){
		GUILayout.BeginVertical();
		GUILayout.Label(tooltipLine1);
		GUILayout.EndVertical();
	}

}
