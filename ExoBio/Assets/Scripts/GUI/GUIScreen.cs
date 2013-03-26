using UnityEngine;
using System.Collections;

public abstract class GUIScreen : MonoBehaviour {
	
	bool fadeOut, fadeIn, scaling, wrapUp, wrapDown, moving;
	protected static float targetWidth = 1920;
	protected static float targetHeight = 1080;
	protected bool displayed = false;
	protected bool persist = false;
	protected static bool letterBoxing = false;
	protected Timer transitionTimer;
	float transitionTime = .3f;
	float alpha = 1f;
	protected int depth = 100;
	Matrix4x4 screenTransform;
	protected static Vector3 resolutionTransform;
	Vector3 scalingTransform, wrappingTransform, movingTransform, currentMove, moveGoal;
	Quaternion rotatingTransform;
	Rect bounds;
	protected static Rect screenBounds, letterBox1, letterBox2;
	protected static Texture2D letterBox;

	// Use this for initialization
	protected virtual void Awake (){
		scalingTransform = Vector3.one;
		wrappingTransform = Vector3.one;
		rotatingTransform = Quaternion.identity;
		transitionTimer = new Timer(transitionTime);
		screenTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, 1f));
		if (!letterBoxing){
			screenBounds = new Rect(0,0, GUIScreen.targetWidth, GUIScreen.targetHeight);
			GUIScreen.resolutionTransform = new Vector3(Screen.width/GUIScreen.targetWidth, Screen.height/GUIScreen.targetHeight, 1f);
		}
	}
	
	protected static void useLetterBox(){
		GUIScreen.letterBox = new Texture2D(1,1);
		GUIScreen.letterBox.SetPixel(0,0, Color.black);
		GUIScreen.letterBox.Apply();
		float ratio = Screen.width/GUIScreen.targetWidth;
		bool width = true;
		if (ratio > Screen.height/GUIScreen.targetHeight){
			width = false;
			ratio = Screen.height/GUIScreen.targetHeight;
		}
		GUIScreen.screenBounds = new Rect((Screen.width/ratio - GUIScreen.targetWidth)/2f,(Screen.height/ratio - GUIScreen.targetHeight)/2f,GUIScreen.targetWidth, GUIScreen.targetHeight);
		if (!width){
			GUIScreen.letterBox1 = new Rect(0,0,(Screen.width - GUIScreen.targetWidth*ratio)/2f, Screen.height);
			GUIScreen.letterBox2 = new Rect((Screen.width - GUIScreen.targetWidth*ratio)/2f + targetWidth*ratio,0,(Screen.width - GUIScreen.targetWidth*ratio)/2f, Screen.height);
		}
		else{
			GUIScreen.letterBox1 = new Rect(0,0,Screen.width, (Screen.height - GUIScreen.targetHeight*ratio)/2f+1);
			GUIScreen.letterBox2 = new Rect(0,(Screen.height - GUIScreen.targetHeight*ratio)/2f + GUIScreen.targetHeight*ratio,Screen.width, (Screen.height - GUIScreen.targetHeight*ratio)/2f);
		}
		GUIScreen.resolutionTransform = new Vector3(ratio, ratio, 1f);
		GUIScreen.letterBoxing = true;
	}
			

	void OnGUI(){
		if (GUIScreen.letterBoxing){
			GUI.DrawTexture(GUIScreen.letterBox1, GUIScreen.letterBox);
			GUI.DrawTexture(GUIScreen.letterBox2, GUIScreen.letterBox);
		}
		if (displayed){
			GUI.depth = depth;
			GUI.matrix = screenTransform;
			if (fadeOut){
				alpha = transitionTimer.Percent();
				if (scaling)
					scalingTransform = Vector3.one*(1f-alpha/10f);
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f-alpha);
				if (alpha >= 1){
					fadeOut = false;
					if (!persist)
						displayed = false;
				}
			}
			else if (fadeIn){
				alpha = transitionTimer.Percent();
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
				if (scaling)
					scalingTransform = Vector3.one*(alpha/10f + .9f);
				if (alpha >= 1){
					fadeIn = false;
				}
			}
			if (wrapDown){
				wrappingTransform = new Vector3(1f, 1-transitionTimer.Percent(), 1f);
				movingTransform = new Vector3(0f, (bounds.y+bounds.height/2f)*(transitionTimer.Percent()), 0f);
				if (transitionTimer.IsFinished()){
					wrapDown = false;
					if (!persist)
						displayed = false;
				}
			}
			else if (wrapUp){
				wrappingTransform = new Vector3(1f, transitionTimer.Percent(), 1f);
				movingTransform = new Vector3(0f, (bounds.y+bounds.height/2f)*(1-transitionTimer.Percent()), 0f);
				if (transitionTimer.IsFinished())
					wrapUp = false;
			}
			if (moving){
				movingTransform = Vector3.Lerp(currentMove, moveGoal, Mathf.Sin(Mathf.PI*transitionTimer.Percent()/2f));
//				movingTransform = Vector3.Lerp(currentMove, moveGoal, (Mathf.Pow(2f, transitionTimer.Percent()-1)-.5f)*2);
				if (transitionTimer.IsFinished()){
					currentMove = movingTransform;
					if (moveGoal != Vector3.zero){
						if (!persist){
							displayed = false;
							movingTransform = new Vector3(0,0,0);
						}
					}
					moving = false;
				}
			}
			screenTransform = Matrix4x4.TRS(new Vector3(movingTransform.x * resolutionTransform.x, movingTransform.y * resolutionTransform.y, 0f), rotatingTransform, new Vector3(scalingTransform.x * wrappingTransform.x * resolutionTransform.x, scalingTransform.y * wrappingTransform.y * resolutionTransform.y, 1f));
			GUI.BeginGroup(GUIScreen.screenBounds);
			DrawGUI();
			GUI.EndGroup();
		}
	}
	
	protected abstract void DrawGUI();
	
	public void Reset(){
		scalingTransform = Vector3.one;
		wrappingTransform = Vector3.one;
		movingTransform = Vector3.zero;
	}
	
	public void Persist(bool persist){
		this.persist = persist;
	}
	
	public void FadeOut(float transitionTime = .3f){
		scaling = false;
		fadeOut = true;
		this.transitionTime = transitionTime;
		transitionTimer = new Timer(transitionTime);
	}
	
	public void FadeIn(float transitionTime = .3f){
		Reset();
		scaling = false;
		fadeIn = true;
		displayed = true;
		this.transitionTime = transitionTime;
		transitionTimer = new Timer(transitionTime);
	}
	
	public void ScaleOut(float scaleTime = .3f){
		Reset();
		FadeOut (scaleTime);
		scaling = true;
	}
	
	public void ScaleIn(float scaleTime = .3f){
		Reset();
		FadeIn (scaleTime);
		scaling = true;
	}
	
	public void WrapUp(Rect bounds, float wrapTime = .3f){
		Reset();
		wrapUp = true;
		this.bounds = bounds;
		transitionTimer = new Timer(wrapTime);
		displayed = true;
	}

	public void WrapDown(Rect bounds, float wrapTime = .3f){
		Reset();
		wrapDown = true;
		this.bounds = bounds;
		transitionTimer = new Timer(wrapTime);
	}
	
	public void MoveRight(Rect bounds, float moveTime = 1f){
		if (!persist)
			Reset();
		if (!displayed){
			movingTransform += new Vector3(-(bounds.x + bounds.width), 0,0);
			currentMove = movingTransform;
			moveGoal = new Vector3(0, 0, 0);
			displayed = true;
		}
		else{
			currentMove = movingTransform;
			moveGoal = movingTransform + new Vector3(targetWidth - bounds.x, 0, 0);	
		}
		moving = true;
		transitionTimer = new Timer(moveTime);
	}

	public void MoveLeft(Rect bounds, float moveTime = 1f){
		if (!persist)
			Reset();
		if (!displayed){
			movingTransform += new Vector3(targetWidth - bounds.x, 0,0);
			currentMove = movingTransform;
			moveGoal = new Vector3(0, 0, 0);
			displayed = true;
		}
		else{
			currentMove = movingTransform;
			moveGoal = movingTransform + new Vector3(-(bounds.x + bounds.width), 0, 0);
		}
		moving = true;
		transitionTimer = new Timer(moveTime);
	}
	
	public void MoveDown(Rect bounds, float moveTime = 1f){
		if (!persist)
			Reset();
		if (!displayed){
			movingTransform += new Vector3(0, -(bounds.y + bounds.height),0);
			currentMove = movingTransform;
			moveGoal = new Vector3(0, 0, 0);
			displayed = true;
		}
		else{
			currentMove = movingTransform;
			moveGoal = movingTransform + new Vector3(0, targetHeight - bounds.y, 0);
		}
		moving = true;
		transitionTimer = new Timer(moveTime);
	}

	public void MoveUp(Rect bounds, float moveTime = 1f){
		if (!persist)
			Reset();
		if (!displayed){
			movingTransform += new Vector3(0, targetHeight - bounds.y,0);
			moveGoal = new Vector3(0, 0, 0);
			currentMove = movingTransform;
			displayed = true;
		}
		else{
			currentMove = movingTransform;
			moveGoal = movingTransform + new Vector3(0, -(bounds.y + bounds.height), 0);
		}
		moving = true;
		transitionTimer = new Timer(moveTime);
	}	
	
	public  void Disable(){
		GUI.enabled = false;	
	}
	
	public void Enable(){
		GUI.enabled = true;
	}
}