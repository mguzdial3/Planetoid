using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewGUI : GUIScreen {
	
	List<Texture2D> pics;
	int currentIndex = 0;
	Rect previewRect = new Rect(targetWidth/2f - 500, 660, 1000, 300);
	Rect pictureRect = new Rect(targetWidth/2f - 500, 60, 1000, 600);
	Vector2 previewScroll;
	BigPicture currentPic;
	
	void Start () {
		pics = new List<Texture2D>(DataHolder.pictures.Keys);
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
			newPic.area = pictureRect;
			if (index > currentIndex){
				currentPic.MoveLeft (pictureRect);
				newPic.MoveLeft (pictureRect);
			}
			else{
				currentPic.MoveRight (pictureRect);
				newPic.MoveRight (pictureRect);
			}
			currentPic = newPic;
			currentIndex = index;
		}
	}
	
	void Update () {
	
	}
}

internal class BigPicture : GUIScreen {
	
	public Texture2D picture;
	public Rect size = new Rect(0,0,1000,600);
	public Rect area;

	protected override void DrawGUI (){
		GUI.BeginGroup(area);
		GUI.DrawTexture(size, picture);
		GUI.EndGroup();
	}
	
	void Update(){
		if (!displayed){
			Destroy(this);
		}
	}
}