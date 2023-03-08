using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetAmmo : Interactable
{
	[HideInInspector]
	public bool gotAmmo;
	[HideInInspector]
	public bool ammo;
	public PlayerMovement PlayerMovement;
	private AudioSource pickup;
	
	private void Start()
	{
		ammo = false;
		this.gameObject.SetActive(value: true);
		pickup = GetComponent<AudioSource>();
		UpdateLight();
	}

	private void UpdateLight()
	{
		if (gotAmmo)
		{
			StartCoroutine(PickUpAmmo());

		}
	}
	
	IEnumerator PickUpAmmo(){
		

		pickup.Play();
		ammo = true;
		PlayerMovement.revolverAmmo += 6;
		PlayerMovement.revolver = true;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs)
			r.enabled = false;
			
		yield return new WaitForSeconds(pickup.clip.length);
		Destroy(this.gameObject);
	}

	public override string GetDescription()
	{
		if (gotAmmo)
		{
			return "";
		}
		return "Press [E] to collect Ammo.";
	}

	public override void Interact()
	{
		gotAmmo = !gotAmmo;
		UpdateLight();
	}
}

