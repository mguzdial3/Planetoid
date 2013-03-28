using UnityEngine;
using System.Collections;

public class CameraGUI : GUIScreen {
	
	public static CameraGUI camera;
	Texture2D flashTex;
	bool flashing = false;
	
	void Start () {
		CameraGUI.camera = this;
		flashTex = new Texture2D(1,1);
		flashTex.SetPixel(0,0,new Color(1,1,1,.5f));
		flashTex.Apply();
	}
	
	public static void Snap(){
		CameraGUI.camera.Flash();	
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0, targetWidth, targetHeight), flashTex);
	}
	
	public void Flash(){
		FadeIn(.05f);
		flashing = true;
	}
	
	void Update(){
		if (displayed && flashing && transitionTimer.IsFinished()){
			FadeOut(.3f);
			flashing = false;
		}
	}
	
	
}
