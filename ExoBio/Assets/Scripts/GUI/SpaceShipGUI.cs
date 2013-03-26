using UnityEngine;
using System.Collections;

public class SpaceShipGUI : GUIScreen {

	public GUISkin skin;
 	int experience, gainedExperience, nextLevelExperience, promotedLevelExperience;
	string currentRank, newRank;
	float gridScale;
	bool animateExperience = true;
	Timer experienceTimer;
	Texture2D blackTex;
	string[] items;
	
	void Start () {
		FadeIn();
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,Color.black);
		blackTex.Apply();
		currentRank = GetRank(DataHolder.currentScore);
		newRank = NextRank(currentRank);
		items = new string[]{"Super Shoes","Zoom Lens","Flash Light"};
		
		experience = DataHolder.totalScore;
		print (DataHolder.currentScore);
		print (experience);
		nextLevelExperience = GetExperience(newRank);
		promotedLevelExperience = nextLevelExperience;
		if (DataHolder.totalScore > nextLevelExperience){
			gridScale = 600/(DataHolder.totalScore*1.5f);
		}
		else{
			gridScale = 600/(nextLevelExperience*1.5f);			
		}
		skin.button.fontSize = 50;
		skin.label.fontSize = 50;
		skin.label.alignment = TextAnchor.MiddleCenter;
		
		experienceTimer = new Timer(3f);
		Persist(true);
		
		Screen.showCursor = true;
		Screen.lockCursor = false;
	}
	
	string GetRank(int score){
		string rank = "Intern";
		if (score > 100)
			rank = "Junior Photographer";
		if (score > 4000)
			rank = "Photojournalist";
		return rank;
	}
	
	int GetExperience(string rank){
		int experience = 0;
		switch (rank){
		case "Junior Photographer":	
			experience = 110;
			break;
		case "Photojournalist":
			experience = 1000;
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
			TransitionGUI.SwitchLevel("SceneTest-With Creatures");
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
			GUI.Box(new Rect(100, (600 - experience*gridScale) + experience*gridScale*(1-experienceTimer.Percent()), 300, experience*gridScale*experienceTimer.Percent()),"");
			if (experienceTimer.IsFinished()){
				animateExperience = false;
				if (experience >= nextLevelExperience){
					LevelUp();
				}
			}
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
		PopupGUI popup = this.gameObject.AddComponent<PopupGUI>();
		popup.message = "You've been promoted to " +newRank + "!";
		popup.button = true;
		popup.parent = this;
		currentRank = newRank;
		newRank = NextRank(newRank);
		DataHolder.currentScore = DataHolder.totalScore;
		promotedLevelExperience = GetExperience(newRank);
//		MoveDown(new Rect(0,0,targetWidth, targetHeight), .7f);
	}
	
	string NextRank(string rank){
		string nextRank = rank;
		switch (rank){
		case "Intern":
			nextRank = "Junior Photographer";
			break;
		case "Junior Photographer":
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
		GUI.SelectionGrid(new Rect(50,90,400,350), 0, items,1);
		GUI.EndGroup();
	}
}
