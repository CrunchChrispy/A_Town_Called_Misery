using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController Player;
	
	[HideInInspector]
	public UIManager UIManager;
	[HideInInspector]
	public AudioManager AudioManager;
	[HideInInspector]
	public Transform cam;
	[HideInInspector]
	public Transform camRoot;
	public static Vector3 playerPos;
    
	public float health = 3;
	
	Vector3 velocity;
	[Header ("Movement")]
	public float run = 6;
	public float walk = 4f;
	public float crouch = 2;
	
	Vector2 currentInput;
	Vector3 moveDirection;
	[HideInInspector]
	public bool isWalking;	
	[HideInInspector]
	public bool isRunning;
	[HideInInspector]
	public bool isCrouching;
	float standHeight;
	float crouchHeight;
	float standingCenter;
	float crouchingCenter;
	
	[Range(0,200)]
	public float mouseSensitivity = 100f;

	[Range(0f, 0.5f)]
	[SerializeField] private float crouchSpeed = 0.5f;
	
	private float speed;
	float groundDistance;
	

	public float gravity = 30f;
	public float jumpHeight = 8f;
	
    

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


    
	[HideInInspector]
	public bool isGrounded;
	
	int audioClipIndex;
	int[] previousArray;
	int previousArrayIndex;
	[HideInInspector]
	public	AudioClip[] currentAudioArray;
    

	
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
	// Start is called before the first frame update


	void Awake()
    {
        Player = GetComponent<CharacterController>();
	    noise = GetComponent<AudioSource>();
	    
	    isCrouching = false;
	    isWalking = false;
	    
	    groundDistance = Player.height / 2 + .1f;
	    
	    standHeight = Player.height;
	    standingCenter = Player.center.y;
	    crouchHeight = Player.height / 2;
	    crouchingCenter = crouchHeight / 2;
	    
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
		Debug.Log("why not");
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
		
		float desiredHeight = isCrouching ? crouchHeight : standHeight;
		float desiredCenter = isCrouching ? crouchingCenter : standingCenter;

		if (Player.height != desiredHeight)
		{

	    	
			AdjustHeight(desiredHeight, desiredCenter);
		    
			Vector3 camPos = cam.transform.position;
			camPos.y = camRoot.transform.position.y;
			cam.transform.position = camPos;
		   
		}
		
	
		groundDistance = Player.height / 2 + .3f;
	
		RaycastHit hit;
		LayerMask groundMask = LayerMask.GetMask("Ground");
		
		isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundDistance , groundMask);
		
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
		Debug.Log("Grounded");
		
		
		if(Input.GetButton("Crouch") && isGrounded){
			isCrouching = true;
			Debug.Log("why");
		}
		else{
			isCrouching = false;
		}
			    
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
			//isRunning = false;
		}

			
		Move();
		
		Debug.Log(health);
		if(health == 0){
			Death();
		}
		

	    //WeaponEquiped();

	}
    
    //void WeaponEquiped()
    //{

    //}
    
	private void Move()
	{
		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;	
		
		
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
				
		}
		Player.Move(move * walk * Time.deltaTime);
		velocity.y += gravity * Time.deltaTime;
		Player.Move(velocity * Time.deltaTime);


	    if(isRunning){
	    	Player.Move(move * run * Time.deltaTime);
		    velocity.y += gravity * Time.deltaTime;
		    Player.Move(velocity * Time.deltaTime);
	    }
	    
		if(isCrouching){
			isRunning = false;
		    Player.Move(move * crouch * Time.deltaTime);
		    velocity.y += gravity * Time.deltaTime;
		    Player.Move(velocity * Time.deltaTime);
	    }
	    
	}
    


	
	IEnumerator playAudioSequentially()
	{
		yield return null;
		
		while(isGrounded = true){
			//1.Loop through each AudioClip
			if(isWalking || isCrouching){
				
				for (int i = 0; i < currentAudioArray.Length; i++){
					
					noise.clip = AudioManager.GetRandomAudioClip(currentAudioArray);
						noise.Play();
			
						if(!isWalking){
							noise.Stop();
							yield return null;
						}
			
						if(isRunning){
							yield return new WaitForSeconds(noise.clip.length);
						}
				
						else if(isWalking || isCrouching){
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

    
    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }
    
	void AdjustHeight(float height, float center)
	{
					    
		Player.height = Mathf.Lerp(Player.height, height, crouchSpeed);
		Player.center = Vector3.Lerp(Player.center, new Vector3(0, center, 0), crouchSpeed);
	 
	}
    public void Death()
    {
        Debug.Log("Ded");
        Time.timeScale = 0;
        UIManager.GameOver.enabled = true;
        
    }

}




