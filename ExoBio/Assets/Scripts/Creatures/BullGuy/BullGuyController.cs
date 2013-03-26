using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullGuyController : CreatureController {

	void Start(){
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		maxEnergy = 10000;
		behaviors.Add("First Person Controller", new CreatureAction(STANDING, 0.4f, ThrowIntoAir));
	}
	
	//Hang Out With
	private void ThrowIntoAir(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		
		
		print("Distance: "+distance);
		
		
		if(distance>2){
			movementController.MoveTowards(buddy.transform.position,0.05f);
		}
		else{
			print("Ever getting to here");
			TimedForce hasForce = buddy.GetComponent<TimedForce>();
			
			if(hasForce==null){
				
				TimedForce tf = buddy.AddComponent<TimedForce>();
				tf.forceDirection = transform.up+ transform.forward;
				
				tf.forceMagnitude=1;
			}
			
		}
	}
}