using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewGUI : GUIScreen {
	
	List<Texture2D> pics;
	List<int> scores;
	int currentIndex = 0;
	Rect previewRect = new Rect(targetWidth/2f - 500, 660, 1000, 300);
	Rect pictureRect = new Rect(targetWidth/2f - 500, 60, targetWidth, 600);
	Rect pictureMovingRect = new Rect(0, 60, targetWidth, 600);
	Vector2 previewScroll, newScrollPosition, oldScrollPosition;
	BigPicture currentPic;
	Timer scrollTimer;
	float scrollTime = 1f;
	
	void Start () {
		scrollTimer = new Timer(scrollTime);
		scrollTimer.Stop();
		pics = new List<Texture2D>(DataHolder.pictures.Keys);
		scores = new List<int>(DataHolder.pictures.Values);
		FadeIn();
		Screen.lockCursor = false;
		Screen.showCursor = true;
		currentPic = this.gameObject.AddComponent<BigPicture>();
		currentPic.picture = pics[0];
		currentPic.area = pictureRect;
		currentPic.FadeIn();
	}
	
	protected override void DrawGUI (){
		//Left Button
		
		//Right Button
		
		//Picture
		
		//Previews
		int index = 0;
		GUILayout.BeginArea(previewRect);
		previewScroll = GUILayout.BeginScrollView(previewScroll, GUILayout.Width(1000), GUILayout.Height(200));
		GUILayout.BeginHorizontal();
		foreach (Texture2D pic in pics){
			if (GUILayout.Button(pic, GUILayout.Width(300), GUILayout.Height(170))){
				ToIndex(index);
			}
			index++;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		//Leave Button
		if (GUI.Button(new Rect(targetWidth/2f - 200, 900, 400, 100),"Done")){
			TransitionGUI.SwitchLevel("spaceship");	
		}
	}
	
	void Left(){
		
	}
	
	void Right(){
		
	}
	
	void ToIndex(int index){
		if (index != currentIndex){
			BigPicture newPic = this.gameObject.AddComponent<BigPicture>();
			newPic.picture = pics[index];
			newPic.score = scores[index];
			newPic.area = pictureRect;
			if (index > currentIndex){
				currentPic.MoveLeft (pictureMovingRect, 1f);
				newPic.MoveLeft (pictureMovingRect, 1f);
			}
			else{
				currentPic.MoveRight (pictureMovingRect, 1f);
				newPic.MoveRight (pictureMovingRect, 1f);
			}
			
			float actualDistance = Mathf.Clamp(304*index - 350, 0, 300*pics.Count);
			float scrollDistance = actualDistance * 1000f / (310f*(pics.Count));
			oldScrollPosition = previewScroll;
			newScrollPosition = new Vector2(actualDistance,0);
			scrollTimer = new Timer(scrollTime);
			
			
			currentPic = newPic;
			currentIndex = index;
		}
	}
	
	void Update () {
		if (!scrollTimer.IsFinished()){
			print (Time.time);
			previewScroll = Vector2.Lerp(oldScrollPosition, newScrollPosition, Mathf.Sin(Mathf.PI*scrollTimer.Percent()/2f));			
		}
		else if (!scrollTimer.IsStopped()){
			scrollTimer.Stop();	
		}
	}
}

internal class BigPicture : GUIScreen {
	
	public Texture2D picture;
	public int score;
	public Rect size = new Rect(0,0,1000,600);
	public Rect area;
	public GUIStyle scoreStyle;
	
	void Start(){
		scoreStyle = new GUIStyle();
		scoreStyle.fontSize = 30;
		scoreStyle.alignment = TextAnchor.MiddleCenter;
		scoreStyle.normal.textColor = Color.white;
	}

	protected override void DrawGUI (){
		GUI.BeginGroup(area);
		GUI.DrawTexture(size, picture);
		GUI.Label(new Rect(0, 550, 1000, 50)," "+score+" ",scoreStyle);
		GUI.EndGroup();
	}
	
	void Update(){
		if (!displayed){
			Destroy(this);
		}
	}
}