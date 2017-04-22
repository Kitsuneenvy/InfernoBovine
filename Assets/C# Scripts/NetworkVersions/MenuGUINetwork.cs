using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class MenuGUINetwork : NetworkBehaviour {

	bool options = false;
	bool fullscreen = false;
	Rect menuRect = new Rect(Screen.width/10,Screen.height/10,Screen.width-Screen.width/5,Screen.height-Screen.height/5);

	float volumeLevel = 100;


	public GUISkin defaultSkin;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer){
			return;
		}
	}

	void OnGUI(){
		GUI.skin = defaultSkin;
		if(!options){
			GUI.Window(0,menuRect,menuFunction,"");
		}
		if(options){
			GUI.Window(1,menuRect,optionsFunction,"");
		}
	}

	void menuFunction(int id){
		GUILayout.BeginVertical();
		if(GUILayout.Button("Start game")){
			SceneManager.LoadScene(1);
		}
		if(GUILayout.Button("Options")){
			options = true;
		}
		GUILayout.EndHorizontal();
	}

	void optionsFunction(int id){
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Fullscreen :");
		fullscreen = GUILayout.Toggle(fullscreen,"");
		GUILayout.EndHorizontal();
		GUILayout.Label("Volume :" + volumeLevel.ToString("F0"));
		volumeLevel = GUILayout.HorizontalSlider(volumeLevel,0,100);

		if(GUILayout.Button("Return to Menu")){
			options = false;
		}
		GUILayout.EndVertical();
	}
}
