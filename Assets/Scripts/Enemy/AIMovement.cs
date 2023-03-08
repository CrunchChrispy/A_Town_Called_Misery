using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMovement: MonoBehaviour
{

	public Gun Gun;
	public PlayerManager PlayerManager;
    public Transform[] moveSpots;
    private int randomSpot;

    //Wait Time at waypoint for patrolling
    private float waitTime;
    public float startWaitTime = 1f;

    NavMeshAgent nav;

    //AI Strafe
    public float distToPlayer = 5.0f;

    private float randomStrafeStartTime;
    private float waitStrafeTime;
    public float t_minStrafe;
    public float t_maxStrafe;

    public Transform strafeLeft;
    public Transform strafeRight;
	private int randomStrafeDir;


	[Range (5, 30)]
	public float chaseRadius = 20f;
	[HideInInspector]
	public bool isChasing;
    public float facePlayerFactor = 20f;
	public PlayerMovement PlayerMovement;
	public LightTracker LightTracker;
	//public AudioSource Player;
	
    //AI Sight and Memory
    private bool aiMemorizesPlayer = false;
    public float memoryStartTime = 10;
    private float increasingMemoryTime;

    //AI Hearing
    Vector3 noisePosition;
	private bool aiHeardPlayer = false;
	[Range (10, 60)]
	public float noiseTravelDistance = 100f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime = 3f;
	[HideInInspector]
	public bool playerVisible;
	[Range (2, 7)]
	public float meleeRadius = 5f;
	
	[Header ("Animation")]
	public Animator Enemy;
	
	[Header ("Colt 45 Single Action Army Revolver")]
	public int revolverAmmo;	
	public int currentAmmo;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }

    void Start()
    {
        waitTime = startWaitTime;
	    randomSpot = Random.Range(0, moveSpots.Length);
	    PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
	    PlayerMovement = GameObject.FindObjectOfType<PlayerMovement>();
	    LightTracker = GameObject.FindObjectOfType<LightTracker>();

    }

    void Update()
	{
		
		float distance = Vector3.Distance(PlayerMovement.playerPos, transform.position);
		


        if (playerVisible == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
        {
            Patrol();
            NoiseCheck();
            StopCoroutine(AiMemory());
        }
        else if (aiHeardPlayer == true && playerVisible == false && aiMemorizesPlayer == false)
        {
            canSpin = true;
            GoToNoisePosition();
        }
		else if (playerVisible == true)
		{
			aiMemorizesPlayer = true;
			Debug.Log("CHASE DAMNIT!");
			ChasePlayer();

			FacePlayer();
		}
		else if (aiMemorizesPlayer == true && playerVisible == false)
		{
			ChasePlayer();
			StartCoroutine(AiMemory());

		}

    }
    
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, noiseTravelDistance);
		
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.transform.position, chaseRadius);
		
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(this.transform.position, meleeRadius);
		
	} 
    

    void NoiseCheck()
    {
        float distance = Vector3.Distance(PlayerMovement.playerPos, transform.position);

        if(distance <= noiseTravelDistance)
        {
        	if(PlayerMovement.isRunning){
	        	noisePosition = PlayerMovement.playerPos;

	        	aiHeardPlayer = true;
        	}
        	
	        if (Input.GetButton("Fire1") && PlayerMovement.revolver == true && PlayerMovement.currentAmmo >= 1 && Time.timeScale == 1)
            {
                noisePosition = PlayerMovement.playerPos;

                aiHeardPlayer = true;
            }
            else
            {
                aiHeardPlayer = false;
                canSpin = false;
            }
        }
    }

    void GoToNoisePosition()
    {
        nav.SetDestination(noisePosition);

        if(Vector3.Distance(PlayerMovement.playerPos, transform.position) <= 5f && canSpin == true)
        {
            isSpinningTime += Time.deltaTime;

            transform.Rotate(Vector3.up * spinSpeed, Space.World);
            if(isSpinningTime >= spinTime)
            {
                canSpin = false;
                aiHeardPlayer = false;
                isSpinningTime = 0;
            }
        }
    }

    IEnumerator AiMemory()
    {
        increasingMemoryTime = 0;
        while(increasingMemoryTime < memoryStartTime)
        {
            increasingMemoryTime += Time.deltaTime;
            aiMemorizesPlayer = true;
            yield return null;
        }
        aiHeardPlayer = false;
        aiMemorizesPlayer = false;
    }

    void Patrol()
    {
	    nav.SetDestination(moveSpots[randomSpot].position);
	    PlayerManager.spotted = false;
	    Enemy.SetBool("isWalking", true);

        if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
        {
        	
        	
            if(waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

	            waitTime = startWaitTime;
	            
            }
            else { waitTime -= Time.deltaTime;

	            Enemy.SetBool("isWalking", false);
            }
        }
    }

	public void ChasePlayer()
    {
	    float distance = Vector3.Distance(PlayerMovement.playerPos, transform.position);
        
	    

	    if(distance <= chaseRadius && distance > distToPlayer && aiMemorizesPlayer == true)
        {
		    nav.SetDestination(PlayerMovement.playerPos);
		    PlayerManager.spotted = true;
        }
        else if (nav.isActiveAndEnabled && distance <= distToPlayer)
        {
            randomStrafeDir = Random.Range(0,2);
            randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);

            if(waitStrafeTime <= 0)
            {
                if(randomStrafeDir == 0)
                {
                    nav.SetDestination(strafeLeft.position);
                }
               else if (randomStrafeDir == 1)
                {
                    nav.SetDestination(strafeRight.position);
                }
                waitStrafeTime = randomStrafeStartTime;
            }
            else
            {
                waitStrafeTime -= Time.deltaTime;
            }
        }
    }
    

    public void InSight()
	{

		
		if(LightTracker.Light >= LightTracker.VisibilityThreshold){
		playerVisible = true;
		}
			
		if(distToPlayer <= meleeRadius){
			playerVisible = true;
		}

		//else{
		//	playerVisible = false;
		//}

}
    public void OutOfSight()
	{
			playerVisible = false;
		

    }

    void FacePlayer()
    {
        Vector3 direction = (PlayerMovement.playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
    }

}

