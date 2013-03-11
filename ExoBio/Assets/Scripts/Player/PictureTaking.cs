using UnityEngine;
using System.Collections;
using System.IO;



public class PictureTaking : MonoBehaviour {
	public Camera cam;
	//public Renderer texture;
	private bool guiOn, takeShot;
	int indexLookingAt;
	public ArrayList textures;
	public Texture2D[] array;
	int resWidth, resHeight;
	private int count = 0;
	
	
	public GUITexture cameraMark;
	
	void Start(){
		resWidth=Screen.width;
		resHeight=Screen.height;
		takeShot=true;
		textures = new ArrayList();
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
		
		
		array = textures.ToArray() as Texture2D[];
		
		
		Vector3 cameraEnd = transform.position;
		cameraEnd+=transform.forward*100;
		
		Debug.DrawLine(transform.position,cameraEnd);
		
		 RaycastHit hit;
		//Determine color of indicator, 100.0f is our range for now
		if(Physics.Raycast(transform.position,transform.forward, out hit,100.0f)){
			//print("I raycasted at: "+hit.transform.name);
			
			if(hit.collider.tag=="Creature"){
				cameraMark.color= Color.red;
			
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
			
			
			if(Input.GetMouseButtonDown(0) ){
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
	
	
	IEnumerator TakePicture(){
		// create a texture to pass to encoding
		yield return new WaitForEndOfFrame();
		
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
		textures.Add(texture);
		
		
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
			}
			else{
				GUI.Box(new Rect (10,10,100,50), "Nothing Found");
			}
		}
	}
}
