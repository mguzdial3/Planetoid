using UnityEngine;
using System.Collections;

public class PauseMenuGUI : GUIScreen {
	
	float width = 500f;
	float height = 200f;
	float buttonWidth, buttonHeight, heightBlock;
	bool paused = false;
	public GUISkin skin;
	Texture2D blackTex;
	
	void Start (){
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,new Color(0,0,0,.3f));
		blackTex.Apply();
		width = 400;
		height = 150;
		buttonWidth = 100f;
		buttonHeight = 50f;
	}
	
	protected override void DrawGUI(){
		GUI.skin = skin;
		GUI.DrawTexture(new Rect(0,0,2*targetWidth,2*targetHeight),blackTex, ScaleMode.StretchToFill);
		GUI.BeginGroup(new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");
		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "Pause", skin.customStyles[0]);
		if (GUI.Button(new Rect(width/2 - buttonWidth - 5, 80, buttonWidth, buttonHeight),"Main Menu"))
			MainMenu();
		if (GUI.Button(new Rect(width/2 + 5, 80f, buttonWidth, buttonHeight), "Login"))
			Login();
		GUI.EndGroup();
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (paused){
				ScaleOut(.2f);
				paused = false;
			}
			else{
				ScaleIn (.2f);
				paused = true;
			}
		}
	}
	
	void MainMenu(){
		TransitionGUI.SwitchLevel("mainmenu");
		ScaleOut();
	}
	
	void Login(){
		TransitionGUI.SwitchLevel("loginscene");
		ScaleOut();
	}	
}
