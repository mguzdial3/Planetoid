using UnityEngine;
using System.Collections;

public class TransitionGUI : GUIScreen {
	
	Texture2D blackTex;
	bool switchingLevel;
	static string levelName;
	static TransitionGUI guiReference;
	
	// Use this for initialization
	void Start () {
		depth = 0;
//		DontDestroyOnLoad(this.gameObject);
		guiReference = this;
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,Color.black);
		blackTex.Apply();
		displayed = true;
		FadeOut(.6f);
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0, targetWidth, targetHeight), blackTex,ScaleMode.StretchToFill);
	}
	
	void Update(){
		if (switchingLevel && transitionTimer.IsFinished()){
			Application.LoadLevel(levelName);
		}
	}
	
	public static void SwitchLevel(string levelName){
		TransitionGUI.levelName = levelName;
		TransitionGUI.guiReference.FadeIn();
		TransitionGUI.guiReference.switchingLevel = true;
	}
}
