using UnityEngine;
using System.Collections;

public abstract class GUIScreen : MonoBehaviour {
	
	bool fadeOut, fadeIn, scaling;
	protected bool displayed;
	protected Timer transitionTimer;
	float fadeTime = .3f;
	float alpha = 1f;
	protected int depth = 100;
	Matrix4x4 screenTransform;

	// Use this for initialization
	protected virtual void Start (){
		transitionTimer = new Timer(fadeTime);
		screenTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, 1f));
	}

	void OnGUI(){
		if (displayed){
			GUI.depth = depth;
			GUI.matrix = screenTransform;
			if (fadeOut){
				alpha = transitionTimer.Percent();
				if (scaling)
					screenTransform = Matrix4x4.Scale(Vector3.one*(1f-alpha/10f));
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f-alpha);
				if (alpha >= 1){
					fadeOut = false;
					displayed = false;
				}
			}
			else if (fadeIn){
				alpha = transitionTimer.Percent();
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
				if (scaling)
					screenTransform = Matrix4x4.Scale(Vector3.one*(alpha/10f + .9f));
				if (alpha >= 1){
					fadeIn = false;
				}
			}
			DrawGUI();
		}
	}
	
	protected abstract void DrawGUI();
	
	public void FadeOut(float fadeTime = .3f){
		scaling = false;
		fadeOut = true;
		this.fadeTime = fadeTime;
		transitionTimer = new Timer(fadeTime);
	}
	
	public void FadeIn(float fadeTime = .3f){
		scaling = false;
		fadeIn = true;
		displayed = true;
		this.fadeTime = fadeTime;
		transitionTimer = new Timer(fadeTime);
	}
	
	public void ScaleOut(float scaleTime = .3f){
		FadeOut (scaleTime);
		scaling = true;
	}
	
	public void ScaleIn(float scaleTime = .3f){
		FadeIn (scaleTime);
		scaling = true;
	}
}