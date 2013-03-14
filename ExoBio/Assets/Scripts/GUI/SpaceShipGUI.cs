using UnityEngine;
using System.Collections;

public class SpaceShipGUI : GUIScreen {

	public GUISkin skin;
	
	void Start () {
		FadeIn();
		skin.button.fontSize = 50;
		Screen.showCursor = true;
		Screen.lockCursor = false;
	}
	
	protected override void DrawGUI (){
		GUI.skin = skin;
		if (GUI.Button(new Rect(targetWidth/2f-200, targetHeight/2 - 100, 400, 200), "Leave Ship")){
			TransitionGUI.SwitchLevel("SceneTest");
		}
	}
}
