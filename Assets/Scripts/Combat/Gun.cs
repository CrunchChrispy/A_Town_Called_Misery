using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    [Range(10,110)]
    public float range;
    public float fireRate;
    public float impactForce;
    public int reloads;
    public int ammoCapacity;
    public float reloadTime;


    public UIManager UIManager;
	public AudioManager AudioManager;
    public Camera fpsCam;
	// public GameObject impactEffect;

    
    private int reloadedAmmo;
    private bool isReloading = false;
	private float nextTimeToFire = 0f;
	private AudioSource noise;

	public ParticleSystem muzzleFlash;
	public Animator Flash;
    AIMovement AIMovement;
	public PlayerMovement PlayerMovement;

    //public Animator animator;

    void Start()
	{
		AudioManager = GameObject.FindObjectOfType<AudioManager>();
		UIManager = GameObject.FindObjectOfType<UIManager>();
		PlayerMovement.currentAmmo = 0;
		//if (PlayerMovement.currentAmmo <= 0)
		//		PlayerMovement.currentAmmo = ammoCapacity;
		UIManager.AmmoCount.text = PlayerMovement.currentAmmo + "/" + PlayerMovement.revolverAmmo;
		
		// muzzleFlash = GetComponentInChildren<ParticleSystem>();
	    noise = GetComponent<AudioSource>();

    }

    void Update()
	{
		
	    	
			UIManager.AmmoCount.text = PlayerMovement.currentAmmo + "/" + PlayerMovement.revolverAmmo;
			
			if (PlayerMovement.currentAmmo < ammoCapacity && PlayerMovement.revolverAmmo >= 1)
			{
				
				if (Input.GetButtonDown("Reload"))
				{
					StartCoroutine(Reload());
					return;
				}
			}
				

     
			if (isReloading){
					
				return;
					
			}
           
		if(PlayerMovement.revolver == true) {
        				
		if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && PlayerMovement.currentAmmo >= 1 && Time.timeScale == 1)
		{
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
		}
		}
		//UIManager.Pause.enabled == false
			
	}
	    


    IEnumerator Reload()
    {
        isReloading = true;
	    Debug.Log("Reloading...");
	    	
	    //reloadedAmmo = ammoCapacity - PlayerMovement.currentAmmo;
 
	    
	    yield return new WaitForSeconds(reloadTime - .25f);
		
	    int reloadedAmmo = 1;
	    PlayerMovement.currentAmmo += reloadedAmmo;
	    //animator.SetBool("Reloading", false);

	    yield return new WaitForSeconds(.25f);

	    //PlayerMovement.currentAmmo = ammoCapacity;
	    isReloading = false;
	    PlayerMovement.revolverAmmo -= reloadedAmmo;
	    		    
	    //if(PlayerMovement.currentAmmo == ammoCapacity || PlayerMovement.revolverAmmo >= 0){
		//    isReloading = false;
	    //}
	    //else{
	    
	    //for (int i = 0; i < PlayerMovement.currentAmmo; i++) {
	    	
	    	
		//    PlayerMovement.currentAmmo += 1;
		//    PlayerMovement.revolverAmmo -= reloadedAmmo;
		//    i++;
		//    yield return new WaitForSeconds(reloadTime - .25f);
		    
		    
		    
		//    yield return null;
	    //}
	    
	    //}
	    
	    // }

        //animator.SetBool("Reloading", false);

	    //yield return new WaitForSeconds(.25f);
        
	    


	    //PlayerMovement.currentAmmo = ammoCapacity;
        
	    
    }

    void Shoot()
	{
		StartCoroutine(GunSounds());
	    muzzleFlash.Play();
	    noise.clip = AudioManager.gunShot;
	    noise.Play();
		 
	    PlayerMovement.currentAmmo--;
	   
        RaycastHit hit;
        if (Time.timeScale == 1)
        {
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                //GameObject ImpactOBJ = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(ImpactOBJ, 2f);
            }
        }
    }
	IEnumerator GunSounds(){
		
		Flash.SetBool("flash", true);
		yield return new WaitForSeconds(.5f);
		Flash.SetBool("flash", false);
		
	//	noise.clip = AudioManager.gunCock;
	//	yield return new WaitForSeconds(reloadTime - .25f);
	}
}
