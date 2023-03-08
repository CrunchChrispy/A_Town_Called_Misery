using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetRevolver : Interactable
{
	public PlayerMovement PlayerMovement;
	private bool gotRevolver;
	private AudioSource pickup;
	
	private void Start()
	{
		PlayerMovement.revolver = false;
		this.gameObject.SetActive(value: true);
		pickup = GetComponent<AudioSource>();	
		PlayerMovement = GameObject.FindObjectOfType<PlayerMovement>();
		UpdateLight();
	}

	private void UpdateLight()
	{
		if (gotRevolver)
		{
			StartCoroutine(PickUpWeapon());
		}
	}
	IEnumerator PickUpWeapon(){
		

		pickup.Play();
		PlayerMovement.revolver = true;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs)
			r.enabled = false;
			
		yield return new WaitForSeconds(pickup.clip.length);

		Destroy(this.gameObject);
	}

	public override string GetDescription()
	{
		if (gotRevolver)
		{
			return "";
		}
		return "Press [E] to equip Colt 45.";
	}

	public override void Interact()
	{
		gotRevolver = !gotRevolver;
		UpdateLight();
	}
}
