using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHolder : MonoBehaviour {
	public static Dictionary<Texture2D, int> pictures;
	public static int currentScore, totalScore;
	public static bool created;
		
	void Start(){
		if (created)
			Destroy(this);
		else{
			DataHolder.created = true;
			print ("Made Data");
			DataHolder.pictures = new Dictionary<Texture2D, int>();
			DataHolder.totalScore = 0;
			DataHolder.currentScore = 0;
			DontDestroyOnLoad(this.gameObject);
		}
	}
	
	public static void AddPictures(ArrayList textures, int[] scores){
		int i = 0;
		foreach(Object o in textures){
			DataHolder.pictures.Add(o as Texture2D,scores[i]);
			DataHolder.totalScore += scores[i];
			i++;
		}
	}
}
