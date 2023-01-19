using UnityEngine;

public class GetAmmo : Interactable
{
	[HideInInspector]
	public bool gotAmmo;
	[HideInInspector]
	public bool ammo;
	public PlayerMovement PlayerMovement;
	private void Start()
	{
		ammo = false;
		this.gameObject.SetActive(value: true);
		UpdateLight();
	}

	private void UpdateLight()
	{
		if (gotAmmo)
		{
			ammo = true;
			PlayerMovement.revolverAmmo += 12;
			Destroy(this.gameObject);
		}
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

