using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	CharacterController Player;
	public Camera playerCamera;
	
	//[HideInInspector]
	public UIManager UIManager;
	//[HideInInspector]
	public AudioManager AudioManager;
	//[HideInInspector]
	public Transform cam;
	//[HideInInspector]
	public Transform camRoot;
	public static Vector3 playerPos;
    
	public int health = 3;	

	//Vector3 velocity;
	
	[Header ("Movement")]
	public float run = 6;
	public float walk = 4f;
	public float crouch = 2;
	public float jumpForce = 15;
	
	Vector3 moveVelocity;
	//Vector3 turnVelocity;
	
	Vector2 currentInput;
	Vector3 moveDirection;
	//[HideInInspector]
	public bool isWalking;	
	
	[Range(0,200)]
	public float mouseSensitivity = 100f;
	
	private float speed;
	float groundDistance;
	

	public float gravity = 9.81f;

	
	//Audio
	AudioSource noise;
	int CurrentfootstepClip;
	[HideInInspector]
	public bool onWood;
	[HideInInspector]
	public bool onDirt;
	[HideInInspector]
	public bool onStone;
	[HideInInspector]
	public bool onMetal;
	
	int audioClipIndex;
	int[] previousArray;
	int previousArrayIndex;
	[HideInInspector]
	public	AudioClip[] currentAudioArray;
	
	//[HideInInspector]
	public bool isGrounded;

	
	[Header ("Field of View")]
	public float fov = 60f;
	public float zoomFOV = 30f;
	public float runFOV = 80f;
	public bool holdToZoom = false;
	//[HideInInspector]
	//public bool enableZoom = true;
	public KeyCode zoomKey = KeyCode.Mouse1;
	public float TimeToFov = 5f;

	// Internal Variables
	private bool isZoomed = false;
	
	
	[Header ("Running")]
	public KeyCode sprintKey = KeyCode.LeftShift;
	public float sprintDuration = 5f;
	public float sprintCooldown = .5f;

	public bool unlimitedSprint = false;
	//[HideInInspector]
	public bool isRunning;

	//// Sprint Bar
	//public bool useSprintBar = true;
	//public bool hideBarWhenFull = true;
	//public Image sprintBarBG;

	//public Image sprintBar;

	//public float sprintBarWidthPercent = .3f;

	//public float sprintBarHeightPercent = .015f;

	//// Internal Variables
	//private CanvasGroup sprintBarCG;
	//private bool isSprinting = false;
	//private float sprintRemaining;
	//private float sprintBarWidth;
	//private float sprintBarHeight;
	//private bool isSprintCooldown = false;
	//private float sprintCooldownReset;
	
	[Header ("Crouch")]
	public bool holdToCrouch = true;
	public KeyCode crouchKey = KeyCode.LeftControl;
	public float speedReduction = .5f;
	//[HideInInspector]
	public bool isCrouching;

	Vector3 originalScale;
	float standHeight;
	float crouchHeight;
	
	
	[Header ("Head Bob")]
	public Transform joint;
	public float bobSpeed = 10f;
	public Vector3 bobAmount = new Vector3(.1f, .05f, 0f);

	Vector3 jointOriginalPos;
	float timer = 0;
    

	
	[Header("Colt 45 Single Action Army Revolver")]
	public Gun Gun;
	public GameObject Revolver;
	public bool revolver;
	public int revolverAmmo;	
	public int currentAmmo;

 /*   [Header("Winchester Rifle")]
	public bool rifle;
	public int rifleAmmo;
	public GameObject Rifle;
	*/


	
	void Start()
    {
        Player = GetComponent<CharacterController>();
	    noise = GetComponent<AudioSource>();
	    
	    UIManager = UIManager = GameObject.FindObjectOfType<UIManager>();
	    AudioManager = AudioManager = GameObject.FindObjectOfType<AudioManager>();
	    
	    isCrouching = false;
	    isWalking = false;
	    
	    groundDistance = Player.height / 2 + .1f;
	    
	    
	    standHeight = Player.height;
	    crouchHeight = Player.height / 2;
	    
	    health = 3;
	    
	    playerCamera.fieldOfView = fov;
	    originalScale = transform.localScale;
	    jointOriginalPos = joint.localPosition;
	    
	    revolver = false;
	    Revolver.SetActive(false);
	    
	    StartCoroutine(TrackPlayer());
	    
	    StartCoroutine(playAudioSequentially());

    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Death();
        }
	}
    
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Debug.Log("why not");
		if (hit.gameObject.CompareTag("Wood"))
		{
			onWood = true;
			onDirt = false;
			onStone = false;
			onMetal = false;			
			currentAudioArray = AudioManager.WoodFootsteps;

		}
		if (hit.gameObject.CompareTag("Dirt"))
		{
			onWood = false;
			onDirt = true;
			onStone = false;
			onMetal = false;		
			currentAudioArray = AudioManager.DirtFootsteps;
		}
		if (hit.gameObject.CompareTag("Stone"))
		{
			onWood = false;
			onDirt = false;
			onStone = true;
			onMetal = false;
			currentAudioArray = AudioManager.StoneFootsteps;
		}
		if (hit.gameObject.CompareTag("Metal"))
		{
			onWood = false;
			onDirt = false;
			onStone = false;
			onMetal = true;
		}
			
		
	}

    
    void Update()
	{

	
		groundDistance = Player.height / 2 + .3f;
	
		RaycastHit hit;
		LayerMask groundMask = LayerMask.GetMask("Ground");
		
		isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundDistance , groundMask);
		
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
		Debug.Log("Grounded");
		
			    
		if(Input.GetButton("Horizontal") || Input.GetButton("Vertical") && isGrounded){
			
			if(isCrouching != true){
				
				isWalking = true;
				isRunning = false;

				
				if(Input.GetButton("Run")){
					isRunning = true;

					
				}
			}

			
		}
		else{
			isWalking = false;	  

		}
		
		
		Zoom();
		
		HeadBob();
		
		Move();
		
		if(Input.GetKeyDown(crouchKey) && !holdToCrouch)
			{
				Crouch();
			}
			
			
		if(health == 0){
			Death();
		}
		
		


	}
	

	private void Zoom(){
		// Changes isZoomed when key is pressed
		// Behavior for toogle zoom
		if(Input.GetKeyDown(zoomKey) && !holdToZoom && !isRunning)
		{
			if (!isZoomed)
			{
				isZoomed = true;
			}
			else
			{
				isZoomed = false;
			}
		}

		// Changes isZoomed when key is pressed
		// Behavior for hold to zoom
		if(holdToZoom && !isRunning)
		{
			if(Input.GetKeyDown(zoomKey))
			{
				isZoomed = true;
			}
			else if(Input.GetKeyUp(zoomKey))
			{
				isZoomed = false;
			}
		}

		// Lerps camera.fieldOfView to allow for a smooth transistion
		if(isZoomed)
		{
			playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, TimeToFov * Time.deltaTime);
		}
		else if(!isZoomed && !isRunning)
		{
			playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, TimeToFov * Time.deltaTime);
		}
	
	}
	
	
	
	//private void Run(){
		
	//	if(isRunning)
	//	{
	//		isZoomed = false;
	//		playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, runFOV, TimeToFov * Time.deltaTime);
	//		// Drain sprint remaining while sprinting
	//		if(!unlimitedSprint)
	//		{

	//			sprintRemaining -= 1 * Time.deltaTime;
	//			if (sprintRemaining <= 0)
	//			{
	//				isSprinting = false;
	//				isSprintCooldown = true;
	//				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, TimeToFov * Time.deltaTime);


	//			}
	//		}
	//	}
	//	else
	//	{
	//		// Regain sprint while not sprinting
	//		sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
	//	}

	//	// Handles sprint cooldown 
	//	// When sprint remaining == 0 stops sprint ability until hitting cooldown
	//	if(isSprintCooldown)
	//	{
	//		sprintCooldown -= 1 * Time.deltaTime;
	//		if (sprintCooldown <= 0)
	//		{
	//			isSprintCooldown = false;
	//		}
	//	}
	//	else
	//	{
	//		sprintCooldown = sprintCooldownReset;
	//	}

	//	// Handles sprintBar 
	//	if(useSprintBar && !unlimitedSprint)
	//	{
	//		float sprintRemainingPercent = sprintRemaining / sprintDuration;
	//		sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
	//	}
	//}
	
	
	
    
	private void Move()
	{
		//if (isGrounded && moveVelocity.y < 0)
		//{
		//	moveVelocity.y = -2f;
		//}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;	
		
		
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
				
		}

		if(isRunning){
			playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, runFOV, TimeToFov * Time.deltaTime);
	    	Player.Move(move * run * Time.deltaTime);
		    moveVelocity.y += gravity * Time.deltaTime;
		    Player.Move(moveVelocity * Time.deltaTime);
	    }
	    
		if(isCrouching){
			playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, TimeToFov * Time.deltaTime);
			isRunning = false;
		    Player.Move(move * crouch * Time.deltaTime);
		    moveVelocity.y += gravity * Time.deltaTime;
		    Player.Move(moveVelocity * Time.deltaTime);
		}
		else{
			playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, TimeToFov * Time.deltaTime);
			Player.Move(move * walk * Time.deltaTime);
			moveVelocity.y += gravity * Time.deltaTime;
			Player.Move(moveVelocity * Time.deltaTime);
		}
	    
		moveVelocity.y += gravity * Time.deltaTime;
		Player.Move(moveVelocity * Time.deltaTime);
  
	}
	
	
	void Jump(){
		// Adds force to the player rigidbody to jump
		if (isGrounded)
		{
			moveVelocity.y = jumpForce;
		}

		// When crouched and using toggle system, will uncrouch for a jump
		if(isCrouching && !holdToCrouch)
		{
			Crouch();
		}
	}
	
	
	private void Crouch(){

		if(isCrouching)
		{
			Player.height = Mathf.Lerp(crouchHeight, standHeight, 1);
			
			Vector3 camPos = cam.transform.position;
			camPos.y = camRoot.transform.position.y;
			cam.transform.position = camPos;
			isCrouching = false;
				
		}

		else
		{	
			Player.height = Mathf.Lerp(standHeight, crouchHeight, 1);
			
			Vector3 camPos = cam.transform.position;
			camPos.y = camRoot.transform.position.y;
			cam.transform.position = camPos;
			isCrouching = true;
			
		}
	}
	
	
	
	void HeadBob()
	{
		if(isWalking)
		{
			// Calculates HeadBob speed during sprint
			if(isRunning)
			{
				timer += Time.deltaTime * (bobSpeed + run);
			}
			// Calculates HeadBob speed during crouched movement
			else if (isCrouching)
			{
				timer += Time.deltaTime * (bobSpeed * speedReduction);
			}
			// Calculates HeadBob speed during walking
			else
			{
				timer += Time.deltaTime * bobSpeed;
			}
			// Applies HeadBob movement
			joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
		}
		else
		{
			// Resets when play stops moving
			timer = 0;
			joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
		}
	}


	
	IEnumerator playAudioSequentially()
	{
		yield return null;
		
		while(isGrounded = true){

			if(isWalking || isCrouching){
				
				for (int i = 0; i < currentAudioArray.Length; i++){
					
					noise.clip = AudioManager.GetRandomAudioClip(currentAudioArray);
					noise.Play();
					
			
						if(!isWalking){
							noise.Stop();
							yield return null;
						}
			
					if(isRunning){
						if(onStone){noise.volume = .125f;}
						if(onWood){noise.volume = .1f;}
						if(onDirt){noise.volume = .08f;}
							
							yield return new WaitForSeconds(noise.clip.length);

						}
				
						else if(isWalking || isCrouching){
							noise.volume = .02f;
							yield return new WaitForSeconds(noise.clip.length + .1f);
							
						}

			
						while (noise.isPlaying)
						{
	
							yield return null;
						}
	
					}

		}
			yield return null;

	  }

		yield return null;
	}
	
	
	
	public void Death()
	{
		Debug.Log("Ded");
		Time.timeScale = 0;
		UIManager.GameOver.enabled = true;
        
	}
	public void Win()
	{
		Debug.Log("Win");
		Time.timeScale = 0;
		UIManager.Win.enabled = true;
        
	}
	

    
    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }


}





