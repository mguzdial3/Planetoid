using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour {
	//Prefabs for various things
	public GameObject rabbit;
	public Transform[] rabbitPositions;
	
	public GameObject friendly;
	public Transform[] friendlyPositions;
	
	public GameObject bull;
	public Transform[] bullPositions;
	
	public GameObject watermelonBear;
	public Transform[] watermelonBearPositions;
	
	public GameObject waterElephant;
	public Transform[] waterElephantPositions;
	
	public GameObject flying;
	public Transform[] flyingPositions;
	
	public GameObject lovesPlayer;
	public Transform[] lovesPlayerPositions;
	
	//Max number you can have 
	private int numRabbits, numFriendlies, numBulls, numWatermelons, numElephants, numFlying, numLoves;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
