using UnityEngine;
using System.Collections;

public class Flashlight : Powerup {
	Light myLight;
	
	//FOR TESTING ONLY
	void Start(){
		Setup();
	}
	
	public override void Setup(){
		myLight = gameObject.AddComponent<Light>();
		myLight.type = LightType.Spot;
		
		myLight.intensity = 0;
		
		myLight.spotAngle = 80;
		myLight.range = 50;
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.V)){
			if(myLight.intensity ==0){
				myLight.intensity = 5;
			}
			else{
				myLight.intensity = 0;
			}
		}
	}
	
	
}
