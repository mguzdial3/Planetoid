using UnityEngine;
using System.Collections;

public class RadarGUI : GUIScreen {
	
	int width = 300, height = 300;
	Texture2D radarScreen, radarDot, radarCircle, radarCenter;
	Transform center, centerA, centerB;
	Transform[] detectables;
	float[] times;
	Timer radarTimer;
	float radarTime = 2f;
	float percent = 0;
	float angle = 0;
	float radarScale = .5f;
	bool toggle = true;
	
	void Start(){
		Screen.lockCursor = true;
		localBounds = new Rect(targetWidth-width-30, targetHeight-height-30, width,height);
		StartCoroutine(FadeIn());
		GameObject[] ds = GameObject.FindGameObjectsWithTag("Creature");
		detectables = new Transform[ds.Length];
		times = new float[ds.Length+1];
		center = GameObject.FindGameObjectWithTag("MainCamera").transform;
		centerA = center;
		centerB = center.GetChild(0);
		int ptr = 0;
		foreach (GameObject d in ds){
			detectables[ptr] = d.transform;
			ptr++;
		}

		radarTimer = new Timer(radarTime, true);
		radarTimer.Repeat();
		
		radarCenter = new Texture2D(1,1);
		radarCenter.SetPixel(0,0,new Color(1,1,1,1f));
		radarCenter.Apply();
		
		radarScreen = Resources.Load("radarscreen") as Texture2D;
		radarDot = Resources.Load("radardot") as Texture2D;
		radarCircle = Resources.Load("radarcircle") as Texture2D;
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Q)){
			if (toggle){
				center = centerB;
				toggle = false;
			}
			else{
				center = centerA;
				toggle = true;	
			}
		}
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0,width,height), radarScreen);
		percent = radarTimer.Percent();
		GUI.color = new Color(1,1,1,1-percent);
		GUI.DrawTexture(new Rect(150*(1-percent),150*(1-percent),300*percent,300*percent),radarCircle);
		angle = RadarAngle();
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(143, 143, 14, 14), radarCenter);
		times[0] = Time.time;
		int ptr = 1;
 		foreach (Transform t in detectables){
			
			Vector2 convertedDistance = GetRadarPosition(t);
			if (convertedDistance.magnitude < 150){
				convertedDistance = rotate(convertedDistance, angle);
				GUI.color = RadarBlink(ptr, convertedDistance);
				GUI.DrawTexture(new Rect((143 + convertedDistance.x), (143 - convertedDistance.y), 15, 15), radarDot);
			}
			ptr++;
			
		}
	}
	
	Vector3 GetRadarPosition(Transform t){
		return radarScale*new Vector2(t.position.x-center.position.x, t.position.z - center.position.z);
	}
	
	float RadarAngle(){
		Vector2 standard = new Vector2(0,1);
		Vector2 vec = new Vector2(center.forward.x, center.forward.z);
		float angle = Vector2.Angle(standard, vec)*Mathf.PI/180f;
		angle *= -Mathf.Sign(standard.x * vec.y - standard.y * vec.x);
		return angle;
	}
	
	Color RadarBlink(int ptr, Vector2 vec){
		times[ptr] -= Time.deltaTime/radarTime;
		if (Mathf.Pow(vec.magnitude - 150*percent, 2) < 25){
			times[ptr] = 1;
		}
		return new Color(1,1,1,times[ptr]);
	}
	
	Vector3 rotate(Vector2 vec, float angle){
		float x = vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle);
		float y = vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle);
		return new Vector2(x,y);
	}
}
