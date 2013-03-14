using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notification : GUIScreen {
	
//	public FamilyMember owner;
	public string title, content;
	public string transitionType = "Wrap";
	public delegate void buttonAction();
	public Dictionary<string, buttonAction> buttons, nextButtons;
	public bool newMessage = false;
	static Notification notifier = null;
	public string nextTitle, nextContent;
	int width = 400;
	int height = 100;
	int buttonHeight = 30;
	int buttonWidth = 50;
	GUIStyle header;
	Timer timeout;
	float timeoutTime = 0;
	Rect windowBounds;
	Sprite test;
	SpriteAnimation spriteAnimation;
	
	void Start (){
		header = GUIStyle.none;
		header.fontSize = 32;
		GUI.contentColor = Color.white;
		Notification.notifier = this;
		buttons = new Dictionary<string, buttonAction>();
		windowBounds = new Rect(targetWidth/2 - width/2, targetHeight - height - 50, width, height);
//		test = new Sprite("testsprite", 32, 32);
//		spriteAnimation = test.animations[0];
		timeout = new Timer(timeoutTime);
		if (timeoutTime <= 0){
			timeout.Stop();	
		}
		WrapUp(windowBounds);
	}
	
	public void Timeout(float time){
		timeoutTime = time;
		timeout = new Timer(timeoutTime);
		print (time);
	}
	
	public static void Notify(string title, string content, Dictionary<string, buttonAction> buttons, float timeout = 0f){
		if (Notification.notifier == null)
		{
			Notification.notifier = GameObject.FindGameObjectWithTag("GUIHandler").AddComponent<Notification>();
			Notification.notifier.title = title;
			Notification.notifier.content = content;
			Notification.notifier.buttons = buttons;
			Notification.notifier.Timeout(timeout);
			Notification.notifier.Transition("Wrap", true);			
		}
		else if (Notification.notifier.displayed){
			Notification.notifier.Transition("Wrap", false);
			Notification.notifier.newMessage = true;
			Notification.notifier.nextButtons = buttons;
			Notification.notifier.nextTitle = title;
			Notification.notifier.nextContent = content;			
		}
	}
	
	protected override void DrawGUI (){
		GUI.Window(1, windowBounds, NotificationWindow, title);
	}
	
	void NotificationWindow(int id){
		int buttonNumber = 0;
		float widthModifier = 0;
//		if (spriteAnimation.IsStopped()){
//			spriteAnimation = test.animations[1];
//		}
//		GUI.DrawTexture(new Rect(0,0,test.spriteWidth, test.spriteHeight),spriteAnimation.PlayAnimation(.3f));
		GUI.Label(new Rect(79, 15, windowBounds.width - 79, windowBounds.height - 15), content);
		foreach (KeyValuePair<string, buttonAction> button in buttons){
			if (buttons.Count%2 == 1)
				widthModifier = buttonNumber-(buttons.Count/2);
			else{
				widthModifier = buttonNumber-(buttons.Count/2) + .5f;
			}

			if (GUI.Button(new Rect(width/2f - (buttonWidth/2f) + (buttonWidth+10)*(widthModifier),
									height - buttonHeight - 10,
									buttonWidth, buttonHeight), button.Key)){
				button.Value();
			}
			buttonNumber++;
		}
//		GUI.DragWindow();
	}
	
	public void Transition(string type, bool TransitionIn){
		switch (type){
		
		case "Wrap":
			if (TransitionIn){
				WrapUp(windowBounds);
			}
			else
				WrapDown(windowBounds);
			break;
		case "Fade":
			if (TransitionIn)
				FadeIn();
			else
				FadeOut ();
			break;
		}
	}
	
	void Update(){
		if (newMessage){
			if (!displayed){
				Notification n = this.gameObject.AddComponent<Notification>();
				n.WrapUp(n.windowBounds);
				n.title = nextTitle;
				n.content = nextContent;
				n.buttons = nextButtons;
				Close();
				Notification.notifier = n;
			}
		}
		if (!displayed){
			Close();
		}
		
		if (timeout.IsFinished() && timeoutTime > 0){
			if (!displayed){
				Close();
			}
			if (transitionTimer.IsStopped())
				WrapDown(windowBounds);
		}
		if (Input.GetKeyDown(KeyCode.Space))
			WrapDown(windowBounds);
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			MoveLeft(windowBounds,.3f);
		if (Input.GetKeyDown(KeyCode.DownArrow))
			MoveDown(windowBounds,.3f);
		
	}
	
	void Close(){
		Destroy(this);
		Notification.notifier = null;
	}
	
	void testButton(){
		WrapDown(windowBounds);
	}
	
	void testButton2(){
		MoveRight(windowBounds,2);
	}
}