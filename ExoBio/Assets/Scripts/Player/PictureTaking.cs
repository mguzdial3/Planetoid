using UnityEngine;
using System.Collections;
using System.IO;



public class PictureTaking : MonoBehaviour {
	public Camera cam;
	//public Renderer texture;
	private bool guiOn, takeShot;
	int indexLookingAt;
	public ArrayList textures;
	public int[] scores;
	int resWidth, resHeight;
	private int count = 0, maxNumberOfShots = 20;
	
	
	//Sound for camera to make
	public AudioClip cameraNoise;
	
	public GUITexture cameraMark;
	
	public bool rabbitPictureHave;
	
	void Start(){
		resWidth=Screen.width;
		resHeight=Screen.height;
		takeShot=true;
		textures = new ArrayList();
		
		scores = new int[maxNumberOfShots];
		//Store in playerprefs for later
		/**
		for(int i =0; i<3; i++){
			Texture2D curr = Resources.Load("-"+i) as Texture2D;
				
			if(curr!=null){
				textures.Add(curr);
			}
		}
		*/
	}
	
	void Update(){
		
		
		//array = textures.ToArray() as Texture2D[];
		
		
		Vector3 cameraEnd = transform.position;
		cameraEnd+=transform.forward*100;
		
		Debug.DrawLine(transform.position,cameraEnd);
		
		 RaycastHit hit;
		//Determine color of indicator, 100.0f is our range for now
		if(Physics.Raycast(transform.position,transform.forward, out hit,100.0f)){
			//print("I raycasted at: "+hit.transform.name);
			
			if(hit.collider.tag=="Creature"){
				cameraMark.color= Color.green;
			
			}
			else{
				cameraMark.color = Color.white;
			}
		}
		else{
			cameraMark.color = Color.white;
		}
		
		
		
		if(!guiOn){
			float zoomAmnt = Input.GetAxis("CamVertical");
			
			if(zoomAmnt!=0){
				cam.fieldOfView-=Time.deltaTime*zoomAmnt*5.0f;
			}
			
			
			if(Input.GetMouseButtonDown(0) && count<maxNumberOfShots){
				//Figure out the score
				RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10.0f, transform.forward, 100.0f);
				
				audio.PlayOneShot(cameraNoise);
				int scoreForDis=determineScore(hits);
				
				scores[count] = scoreForDis;
				
				StartCoroutine(TakePicture());
				
				//StartCoroutine(ScreenshotEncode());
			}
			
		
		
		}
		else{
			//If gui is on
			if(Input.GetKeyDown(KeyCode.A)){
				if(indexLookingAt>0){
					indexLookingAt--;
				}
			}
			else if(Input.GetKeyDown(KeyCode.D)){
				if(indexLookingAt<textures.Count-1){
					indexLookingAt++;
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.E)){
			guiOn=!guiOn;
			indexLookingAt=textures.Count-1;
			
			/**
			ForceUpdate.Force();
			//Test this
			for(int i =0; i<20; i++){
				Texture2D curr = Resources.Load("-"+i) as Texture2D;
				
				if(curr!=null){
					textures.Add(curr);
				}
			}
			*/
		}
	}
	
	
	
	
	void LateUpdate(){
		if(!takeShot){
			
			TakePicture();
			
			
			
		}
	}
	
	int determineScore(RaycastHit[] hits){
		int scoreToReturn =0;
		Vector2 middleOfScreen = new Vector2(Screen.width/2,Screen.height/2);
		
		ArrayList creaturesInPic = new ArrayList();
		
		foreach(RaycastHit hitt in hits){
			if(hitt.collider.tag=="Creature"){
				BasicCreature bc = hitt.collider.GetComponent<BasicCreature>();
				
				//If it has Basic Creature attached, it's a for real creature and not just a limb or something
				if(bc!=null){
					if(!creaturesInPic.Contains(bc)){
						
						
						//You get a 1000 points for a new picture of a creature
						if(!rabbitPictureHave){
							//TODO; Need to check if we already have a picture of the thing
							scoreToReturn+=1000;
			
							rabbitPictureHave=true;
						}
						else{
							//You automatically get 100 points for each creature in the picture, scaled by distance to center of frame
							//and what it's current state is (so you get 0 for running away creatures, 100 or less for standing creatures and so on
							Vector2 screenPos = camera.WorldToScreenPoint(bc.transform.position);
							
							
							float distance = Vector3.Distance(screenPos,middleOfScreen);
							
							float distance3D = (transform.position-bc.transform.position).magnitude;
							
							if(distance==0){
								scoreToReturn+=(int)+(100-distance3D)+(int)(bc.getPoints()*100);
							}
							else{
								scoreToReturn+=(int)+(100-distance3D)+(int)(bc.getPoints()*100*(1.0f/distance));
							}
							
							
							
							
							
						
						}
						creaturesInPic.Add(bc);
					}
				}
			}
		}
		
		
		return scoreToReturn;
	}
	
	
	IEnumerator TakePicture(){
		// create a texture to pass to encoding
		yield return new WaitForEndOfFrame();
		
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
		textures.Add(texture);
		
		count++;
	}
	
	/**
	IEnumerator ScreenshotEncode()
    {
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
 
        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
		textures.Add(texture);
        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
 
        byte[] bytes = texture.EncodeToPNG();
 
        // save our test image (could also upload to WWW)
        System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/-" + count + ".png", bytes);
        count++;
 
        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject( texture );
 
        //Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
    }
	*/
	
	void OnGUI(){
		if(guiOn){
			
			
			//print("Got to here");
			//Texture2D tex = Resources.Load("Screenshot") as Texture2D;
			Texture2D tex = textures.ToArray()[indexLookingAt] as Texture2D;
			
			
			if(tex!=null){
				GUI.Box (new Rect (10,10,Screen.width-20,Screen.height-20), tex);
				
				GUI.TextArea( new Rect(0,0,Screen.width/4, 30), "Score: "+scores[indexLookingAt]);
			}
			else{
				GUI.Box(new Rect (10,10,100,50), "Nothing Found");
			}
		}
	}
}
