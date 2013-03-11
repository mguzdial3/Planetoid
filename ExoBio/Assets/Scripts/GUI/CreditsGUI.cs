using UnityEngine;
using System.Collections;

public class CreditsGUI : GUIScreen {
	
	float width = 500f;
	float height = 200f;
	float buttonWidth, buttonHeight, leftMargin, topMargin, heightBlock;
	public GUISkin skin;
	string username = "";
	string password = "";
	
	protected override void Start (){
		base.Start ();
		width = 400;
		height = 220;
		buttonWidth = 100f;
		buttonHeight = 50f;
		leftMargin = 15f;
		topMargin = 15f;
	}
	
	protected override void DrawGUI(){
		GUI.skin = skin;
		GUI.BeginGroup(new Rect(Screen.width/2f - width/2f, Screen.height/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");
		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "Credits", skin.customStyles[0]);
		GUI.Label(new Rect(width/2 - buttonWidth, 80, 2*buttonWidth, buttonHeight),"Our team is awesome.",skin.customStyles[1]);
		if (GUI.Button(new Rect(width/2 - buttonWidth/2f, 150f, buttonWidth, buttonHeight), "Back"))
			Back();
		GUI.EndGroup();
	}
	
	void Back(){
		this.gameObject.GetComponent<MainMenuGUI>().ScaleIn();
		ScaleOut();
	}	
}
