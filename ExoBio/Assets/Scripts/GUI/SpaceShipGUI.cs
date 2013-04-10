using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceShipGUI : GUIScreen {

	public GUISkin skin;
 	int experience, gainedExperience, nextLevelExperience, promotedLevelExperience;
	string currentRank, newRank;
	float gridScale;
	bool animateExperience = true;
	Timer experienceTimer;
	Texture2D blackTex;
	string[] items;
	Vector2 itemScroll = Vector2.zero;
	Rect experienceRect;
	
	void Start () {
		FadeIn();
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,Color.black);
		blackTex.Apply();
		items = new string[]{"None"};
		experience = (int) DataHolder.GetExperience();
		currentRank = DataHolder.currentRank;
		List<string> newItems = GetPowerUps(currentRank);
		if (newItems.Count > 0)
			items = newItems.ToArray();
		newRank = NextRank(currentRank);
		nextLevelExperience = GetExperience(NextRank(currentRank));
		promotedLevelExperience = nextLevelExperience;
		if (experience > nextLevelExperience){
			gridScale = 600/(experience*1.5f);
		}
		else{
			gridScale = 600/(experience*1.5f);			
		}
		
		skin.button.fontSize = 50;
		skin.label.fontSize = 50;
		skin.label.alignment = TextAnchor.MiddleCenter;
		
		StartCoroutine(ExperienceAnimation());
		Persist(true);
		
		Screen.showCursor = true;
		Screen.lockCursor = false;
		StartCoroutine(FadeIn(.6f));
	}
	
	IEnumerator ExperienceAnimation(){	
		animateExperience = true;
		Timer experienceTimer = new Timer(3f);
		while (!experienceTimer.IsFinished()){
			experienceRect = new Rect(100, (600 - experience*gridScale) + experience*gridScale*(1-experienceTimer.Percent()), 300, experience*gridScale*experienceTimer.Percent());
			yield return 0;
		}

		if (experience >= nextLevelExperience){
			LevelUp();
		}

		animateExperience = false;
	}
	
	string GetRank(int score){
		string rank = "Intern";
		if (score > 1100)
			rank = "Junior Photographer";
		if (score > 4400)
			rank = "Full-Time Photographer";
		if (score > 10000)
			rank = "Photojournalist";
		return rank;
	}
	
	int GetExperience(string rank){
		int experience = 0;
		switch (rank){
		case "Junior Photographer":	
			experience = 1100;
			break;
		case "Full-Time Photographer":
			experience = 4400;
			break;
		case "Photojournalist":
			experience = 10000;
			break;
		}
		return experience;
	}
	
	public void LoadData(){
		
	}
	
	protected override void DrawGUI (){
		GUI.skin = skin;
		RankPanel();
		ExperiencePanel();
		PowerUpPanel();
		
		if (GUI.Button(new Rect(targetWidth/2f + 15, 800, 500, 200), "Leave Ship")){
			StartCoroutine(TransitionGUI.SwitchLevel("SceneTest-With Creatures"));
		}
	}
	
	void RankPanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f - 515, 80, 1030, 300));
		GUI.Box(new Rect(0, 0, 1030, 200), "");
		GUI.Label(new Rect(15, 15, 1000, 170),"Rank: " + currentRank);
		skin.label.fontSize = 30;
		GUI.Label(new Rect(600,150,400,50),"Next: " + newRank);
		skin.label.fontSize = 50;
		GUI.EndGroup();		
	}
	
	void ExperiencePanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f - 515, 300, 500, 700));
		GUI.Box(new Rect(0, 0, 500, 700), "");
		GUI.Label(new Rect(15, 600, 470, 100),"Experience");
		
		// Graph Area
		GUI.Box(new Rect(50,50,400,550),"");
		
		// Experience Bar
		if (animateExperience){
			GUI.Box(experienceRect,"");
		}
		else{
			GUI.Box(new Rect(100, 600 - experience*gridScale, 300, experience*gridScale),"");
		}
		
		// Requirement Line
		GUI.DrawTexture(new Rect(50, 600 - nextLevelExperience*gridScale, 400, 3), blackTex);
		
		
		// Secondary Requirement Line
		GUI.DrawTexture(new Rect(50, 600 - promotedLevelExperience*gridScale, 400, 3), blackTex);
		
		GUI.EndGroup();
	}
	
	void LevelUp(){
		currentRank = GetRank(experience);
		DataHolder.currentRank = currentRank;
		DataHolder.powerUps = GetPowerUps(currentRank);
		items = DataHolder.powerUps.ToArray();
		PopupGUI popup = this.gameObject.AddComponent<PopupGUI>();
		popup.message = "You've been promoted to " +currentRank + "!";
		popup.button = true;
		popup.parent = this;
		newRank = NextRank(newRank);
		promotedLevelExperience = GetExperience(newRank);
//		MoveDown(new Rect(0,0,targetWidth, targetHeight), .7f);
	}
	
	List<string> GetPowerUps(string rank){
		List<string> powers = new List<string>();
		switch (rank){
		case "Photojournalist":
			powers.Add("Jetpack");
			goto case "Full-Time Photographer";
		case "Full-Time Photographer":
			powers.Add("Water Shoes");
			goto case "Junior Photographer";
		case "Junior Photographer":
			powers.Add("Flashlight");
			break;
		}
		return powers;
	}
	
	string NextRank(string rank){
		string nextRank = rank;
		switch (rank){
		case "Intern":
			nextRank = "Junior Photographer";
			break;
		case "Junior Photographer":
			nextRank = "Full-Time Photographer";
			break;
		case "Full-Time Photographer":
			nextRank = "Photojournalist";
			break;
		case "Photojournalist":
			nextRank = "None";
			break;
		}
		return nextRank;
	}
	
	void PowerUpPanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f + 15, 300, 500, 500));
		GUI.Box(new Rect(0, 0, 500, 485), "");
		GUI.Label(new Rect(15, 15, 470, 60),"Items");
		
		// List Container
		GUI.Box(new Rect(50,90,400,350),"");
		GUILayout.BeginArea(new Rect(50,90,400,350));
		itemScroll = GUILayout.BeginScrollView(itemScroll, GUILayout.Width(400), GUILayout.Height(350));
		GUILayout.BeginVertical();
		foreach (string item in items){
			GUILayout.Label(item);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		GUI.EndGroup();
	}
}
