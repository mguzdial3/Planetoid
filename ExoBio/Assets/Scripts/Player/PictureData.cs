using UnityEngine;
using System.Collections;

//Just an info holder for pictures
public class PictureData{
	//Number of creatures in this photo
	public int numCreaturesInPicture;
	//Names of creatures in this photo
	public string[] namesOfCreatures;
	//Score for this photo
	public float score;
	//Whether or not there's a new creature that we haven't seen before in this picture
	public bool newCreature;
	
	
	
	//Constructor
	public PictureData(int _numCreaturesInPicture, string[] names, float _score, bool somebodyNew){
		
		numCreaturesInPicture=_numCreaturesInPicture;
		namesOfCreatures = names;
		score=_score;
		newCreature=somebodyNew;
	}
	
	public PictureData(BasicCreature[] creaturesInPicture, float _score, bool somebodyNew){
		if(creaturesInPicture!=null){
			numCreaturesInPicture = creaturesInPicture.Length-1;
			string[] names = new string[creaturesInPicture.Length];
			
			for(int i = 0; i<creaturesInPicture.Length; i++){
				names[i] = creaturesInPicture[i].transform.name;
			}
		}
		score = _score;
		
		newCreature= somebodyNew;
		
		
	}
	
	
}
