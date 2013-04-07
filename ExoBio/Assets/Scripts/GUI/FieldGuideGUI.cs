using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldGuideGUI : GUIScreen {
	
	int width = 500, height = 600;
	FieldGuidePage current;
	FieldGuideHome home;
	Rect screenRect;
	bool isHome = true;
	bool isOpen = false;
	
	void Start(){
		localBounds = new Rect(0, targetHeight - height, width, height);
		depth = 100;
		screenRect = new Rect(0, 0, width, height);
		home = gameObject.AddComponent<FieldGuideHome>();
		home.guideScreen = localBounds;
		home.parent = this;
//		StartCoroutine(FadeIn());
	}
	
	protected override void DrawGUI (){
		GUI.BeginGroup(screenRect);
		GUI.Box(new Rect(0,0,width,height),"");
		if (GUI.Button(new Rect(10, 10, 50, 50), "<")){
			if (!isHome){
				Home ();
				isHome = true;
			}
			else{
				Close();
			}
		}
		GUI.EndGroup();
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Tab)){
			if (!isOpen)
				Open ();
			else
				Close();
		}
	}
	
	void Open(){
		Screen.lockCursor = false;
		Screen.showCursor = true;
		isOpen = true;
		StartCoroutine(MoveRight(screenRect, .3f));
		if (isHome)
			StartCoroutine(home.MoveRight(screenRect, .3f));	
		else
			StartCoroutine(current.MoveRight(screenRect, .3f));
	}
	
	public void Home(){
		isHome = true;
		StartCoroutine(home.MoveRight(screenRect, .3f));
		StartCoroutine(current.MoveRight(screenRect, .3f));
	}
	
	void Close(){
		StartCoroutine(MoveLeft(screenRect, .3f));
		if (isHome)
			StartCoroutine(home.MoveLeft(screenRect, .3f));
		else
			StartCoroutine(current.MoveLeft(screenRect, .3f));
		isOpen = false;
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	public void SwitchToCreature(string name){
		Dictionary<string, GUIContent> info = DataHolder.GetCreatureData(name);
		isHome = false;
		FieldGuidePage newPage = gameObject.AddComponent<FieldGuidePage>();
		newPage.name = info["name"];
		newPage.image = info["image"];
		newPage.description = info["description"];
		newPage.features = info["features"];
		newPage.notes = info["notes"];
		newPage.width = width;
		newPage.height = height;
		newPage.picWidth = 200;
		newPage.picHeight = 150;
		newPage.guideScreen = localBounds;
		StartCoroutine(home.MoveLeft(screenRect, .4f));
		StartCoroutine(newPage.MoveLeft(screenRect, .4f));
		current= newPage;
	}
}

internal class FieldGuidePage : GUIScreen {
	public GUIContent name, image, description, features, notes;
	public int width, height, picWidth, picHeight;
	public Rect guideScreen;
	Rect size;
	GUIStyle labelStyle, headerStyle,imageStyle;
	
	void Start(){
		localBounds = guideScreen;
		depth = 1;
		labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.white;
		labelStyle.fontSize = 24;
		labelStyle.wordWrap = true;
		
		imageStyle = new GUIStyle();
		imageStyle.alignment = TextAnchor.MiddleCenter;
		
		headerStyle = new GUIStyle();
		headerStyle.normal.textColor = Color.white;
		headerStyle.fontSize = 40;
		headerStyle.wordWrap = true;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		
		size = new Rect(15, 15, guideScreen.width-15, guideScreen.height - 15);
		width = (int)guideScreen.width-30;
		height = (int)guideScreen.height - 30;
	}
	
	protected override void DrawGUI (){
		GUI.BeginGroup(size);
		GUILayout.BeginVertical(GUILayout.Width(width-60));
//		GUILayout.Space(30);
		GUILayout.Label(name, headerStyle, GUILayout.Width(width));
		GUILayout.Space(10);
		GUILayout.Label(image, imageStyle, GUILayout.Width(width));
		GUILayout.Space(20);
		GUILayout.Label(description, labelStyle, GUILayout.Width(width));
		GUILayout.Space(20);
		GUILayout.Label("Features: " + features.text, labelStyle, GUILayout.MaxWidth(width));
		GUILayout.Space(20);
		GUILayout.Label("Notes: \n" + notes.text, labelStyle, GUILayout.MaxWidth(width));
		GUILayout.EndVertical();
		GUI.EndGroup();
	}
}

internal class FieldGuideHome : GUIScreen {
	List<string> creatures;
	public Rect guideScreen;
	Rect size;
	GUIStyle labelStyle, headerStyle;
	public FieldGuideGUI parent;
	
	void Start(){
		localBounds = guideScreen;
		depth = 1;
		labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.white;
		labelStyle.fontSize = 30;
		
		headerStyle = new GUIStyle();
		headerStyle.normal.textColor = Color.white;
		headerStyle.fontSize = 50;
		headerStyle.wordWrap = true;
		headerStyle.alignment = TextAnchor.MiddleCenter;

		size = new Rect(30, 15, guideScreen.width-30, guideScreen.height - 15);
		creatures = new List<string>(DataHolder.creatureInfo.Keys);
	}
	
	protected override void DrawGUI (){
		GUILayout.BeginArea(size);
		GUILayout.BeginVertical();
//		GUILayout.Space(30);
		GUILayout.Label("Field Guide", headerStyle, GUILayout.Width(guideScreen.width));
		GUILayout.Space(20);
		foreach (string creature in creatures){
			if (GUILayout.Button(creature, labelStyle, GUILayout.Height(30))){
				parent.SwitchToCreature(creature);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}