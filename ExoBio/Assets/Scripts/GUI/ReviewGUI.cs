using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewGUI : GUIScreen {
	
	List<Texture2D> pics;
	List<int> scores;
	int currentIndex = 0;
	Rect pictureRect = new Rect(targetWidth/2f - 500, 60, 1000, 600);
	Rect previewRect = new Rect(targetWidth/2f - 500, 660, 1000, 300);
	Rect pictureMovingRect = new Rect(0, 60, targetWidth, 600);
	Vector2 previewScroll, newScrollPosition, oldScrollPosition;
	BigPicture currentPic;
	float scrollTime = .6f;
	
	void Start () {
		if (DataHolder.pictures.Count > 0){
			pics = new List<Texture2D>(DataHolder.pictures.Keys);
			scores = new List<int>(DataHolder.pictures.Values);
			
			currentPic = this.gameObject.AddComponent<BigPicture>();
			currentPic.picture = pics[0];
			currentPic.setLocalBounds(pictureRect);

			StartCoroutine(currentPic.FadeIn());
		}
		else{
			pics = new List<Texture2D>();
			scores = new List<int>();
		}
		Screen.lockCursor = false;
		Screen.showCursor = true;
		StartCoroutine(FadeIn());
	}
	
	protected override void DrawGUI (){
		
		//Previews
		int index = 0;
		GUILayout.BeginArea(previewRect);
		previewScroll = GUILayout.BeginScrollView(previewScroll, GUILayout.Width(1000), GUILayout.Height(200));
		GUILayout.BeginHorizontal();
		foreach (Texture2D pic in pics){
			if (GUILayout.Button(pic, GUILayout.Width(300), GUILayout.Height(170))){
				StartCoroutine(ToIndex(index));
			}
			index++;
		}		
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		//Leave Button
		if (GUI.Button(new Rect(targetWidth/2f - 200, 900, 400, 100),"Done")){
			StartCoroutine(TransitionGUI.SwitchLevel("spaceship"));	
		}
	}
	
	IEnumerator ToIndex(int index){
		if (index != currentIndex){
			BigPicture newPic = this.gameObject.AddComponent<BigPicture>();
			newPic.picture = pics[index];
			newPic.score = scores[index];
			newPic.setLocalBounds(pictureRect);

			float actualDistance = Mathf.Clamp(304*index - 350, 0, 300*pics.Count);
			float scrollDistance = actualDistance * 1000f / (310f*(pics.Count));
			oldScrollPosition = previewScroll;
			newScrollPosition = new Vector2(actualDistance,0);

			StartCoroutine(Scroll(scrollTime));
			
			if (index > currentIndex){
				StartCoroutine(newPic.MoveLeft (newPic.size, .6f));
				yield return StartCoroutine(currentPic.MoveLeft (currentPic.size, .6f));
				Destroy(currentPic);
			}
			else{
				StartCoroutine(newPic.MoveRight (newPic.size, .6f));
				yield return StartCoroutine(currentPic.MoveRight (currentPic.size, .6f));
				Destroy(currentPic);
			}			
			
			currentPic = newPic;
			currentIndex = index;
		}
	}
	
	IEnumerator Scroll(float time){
		Timer scrollTimer = new Timer(time);
		while (!scrollTimer.IsFinished()){
			previewScroll = Vector2.Lerp(oldScrollPosition, newScrollPosition, Mathf.Sin(Mathf.PI*scrollTimer.Percent()/2f));
			yield return 0;
		}
	}
}

internal class BigPicture : GUIScreen {
	
	public Texture2D picture;
	public int score;
	public Rect size = new Rect(0,0,1000,600);
	public Rect area;
	public GUIStyle scoreStyle;
	
	public void setLocalBounds(Rect area){
		this.localBounds = area;
	}
	
	void Start(){
		scoreStyle = new GUIStyle();
		scoreStyle.fontSize = 30;
		scoreStyle.alignment = TextAnchor.MiddleCenter;
		scoreStyle.normal.textColor = Color.white;
	}

	protected override void DrawGUI (){
		GUI.DrawTexture(size, picture);
		GUI.Label(new Rect(0, 550, 1000, 50)," "+score+" ",scoreStyle);
	}
}