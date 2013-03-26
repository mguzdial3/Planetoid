using UnityEngine;
using System.Collections;

public class Timer {
	float startTime, timerInterval, percentDone;
	bool stopped, repeating;
	
	public Timer(float timerInterval){
		this.timerInterval = timerInterval; //time between timer going off (in seconds)
		this.Restart();
		repeating = false;
	}
	
	public bool IsFinished(){
		if (!stopped){
			if (Time.realtimeSinceStartup > startTime + timerInterval){
				if (repeating)
					Restart(); //automatically restart timer when it's found to be over
				else {
					Stop();
				}
				return true;
			}
			else{
				return false;
			}
		}
		else{
			return false;	
		}
	}
	
	public void Repeat(){
		repeating = true;	
	}
	
	public float Percent(){
		percentDone = Mathf.Clamp01((Time.realtimeSinceStartup - startTime)/timerInterval);
		return percentDone;
	}	
	
	public void Restart(){
		startTime = Time.realtimeSinceStartup;
		Start();
	}
	
	public bool IsStopped(){
		return stopped;	
	}

	public void Stop(){
		stopped = true;
	}

	public void Start(){
		stopped = false;
	}
}
