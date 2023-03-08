using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	public int damage;
	[Range(10,110)]
	public float range;
	public float fireRate;
	public float impactForce;
	public int reloads;
	public int ammoCapacity;
	public float reloadTime;


	public AudioManager AudioManager;
	//public GameObject impactEffect;

	Vector3 dir;
	private int reloadedAmmo;
	private bool isReloading = false;
	private float nextTimeToFire = 0f;
	private AudioSource noise;

	ParticleSystem muzzleFlash;
	public PlayerMovement PlayerMovement;
	AIMovement AIMovement;
	
	//public Animator animator;

	void Start()
	{
		
		AudioManager = GameObject.FindObjectOfType<AudioManager>();

		AIMovement = GetComponentInParent<AIMovement>();
		muzzleFlash = GetComponentInChildren<ParticleSystem>();
		noise = GetComponent<AudioSource>();
	       

	}

	void Update()
	{

		if (AIMovement.currentAmmo <= 0 && AIMovement.revolverAmmo >= 1)
		{
			StartCoroutine(Reload());
			return;
		}
        
		if (isReloading){
			return;
		}
        
		if(AIMovement.playerVisible && Time.time >= nextTimeToFire && AIMovement.currentAmmo >= 1)	{
			if(AIMovement.distToPlayer <= AIMovement.chaseRadius && AIMovement.currentAmmo > 0 && AIMovement.playerVisible){
			nextTimeToFire = Time.time + .5f / fireRate;
				Shoot();
		}
		}

	}

	IEnumerator Reload()
	{
		isReloading = true;
		Debug.Log("Reloading...");

		reloadedAmmo = ammoCapacity - AIMovement.currentAmmo;
		

		//animator.SetBool("Reloading", true);

		yield return new WaitForSeconds(reloadTime - .25f);

		
		//animator.SetBool("Reloading", false);

		yield return new WaitForSeconds(.25f);

		AIMovement.currentAmmo = ammoCapacity;
		isReloading = false;
		AIMovement.revolverAmmo -= reloadedAmmo;
	}

	void Shoot()
	{
		muzzleFlash.Play();
		noise.clip = AudioManager.gunShot;
		noise.Play();

			AIMovement.currentAmmo--;
		
		int layerMask = LayerMask.GetMask("Enemy");
		
		
		for(int i = 0; i < ammoCapacity; i++){
			dir = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-0f,0f),Random.Range(-1.0f,1.0f));
			Vector3 sprayDir = transform.TransformVector(dir);
		}
		
		
		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.position + dir, Color.red);
 
		if(Vector3.Distance(transform.position, PlayerMovement.transform.position) < range )
		{
			if(Physics.Raycast(transform.position, (PlayerMovement.transform.position - transform.position + dir), out hit, range))
			{
				if(hit.transform.position == PlayerMovement.transform.position)
				{
					Debug.DrawRay(transform.position, transform.forward, Color.red);
					PlayerMovement.health -= damage;
					// In Range and i can see you!
				}
				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce(-hit.normal * impactForce);
				}
			}
		}
		
	   
		//RaycastHit hit;
		//if (Time.timeScale == 1)
		//{
		//	if (Physics.Raycast(transform.position,PlayerMovement.transform.position, out hit, range, layerMask))
		//	{
		//		//Debug.Log(hit.transform.name);
		//		//Target target = PlayerMovement.GetComponent<Collider>();
		//		Debug.DrawRay(transform.position, transform.forward, Color.red);
				
		//		if (hit.collider.tag == "Player" != null)
		//		{
		//			PlayerMovement.health -= damage;
		//			//transform.tag.TakeDamage(damage);
		//		}
		//		if (hit.rigidbody != null)
		//		{
		//			hit.rigidbody.AddForce(-hit.normal * impactForce);
		//		}
		//		//GameObject ImpactOBJ = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
		//		//Destroy(ImpactOBJ, 2f);
		//	}
		}
	}

