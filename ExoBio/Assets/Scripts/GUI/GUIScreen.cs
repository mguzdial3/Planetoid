using UnityEngine;
using System.Collections;

public abstract class GUIScreen : MonoBehaviour {
	
	bool fadeOut, fadeIn, scaling, wrapUp, wrapDown, moving;
	protected float targetWidth = 1920;
	protected float targetHeight = 1080;
	protected bool displayed = false;
	protected bool persist = false;
	protected Timer transitionTimer;
	float transitionTime = .3f;
	float alpha = 1f;
	protected int depth = 100;
	Matrix4x4 screenTransform;
	Vector3 scalingTransform, resolutionTransform, wrappingTransform, movingTransform, currentMove, moveGoal;
	Quaternion rotatingTransform;
	Rect bounds;

	// Use this for initialization
	protected virtual void Awake (){
		scalingTransform = Vector3.one;
		wrappingTransform = Vector3.one;
		rotatingTransform = Quaternion.identity;
		transitionTimer = new Timer(transitionTime);
		screenTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, 1f));
		resolutionTransform = new Vector3(Screen.width/targetWidth, Screen.height/targetHeight, 1f);
//		resolutionTransform = Vector3.one;
	}

	void OnGUI(){
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
			DrawGUI();
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