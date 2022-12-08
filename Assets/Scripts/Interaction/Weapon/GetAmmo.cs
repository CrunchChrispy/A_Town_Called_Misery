using UnityEngine;

public class GetAmmo : Interactable
{

	public bool gotAmmo;

	public bool ammo;
	public Gun Gun;
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
			Gun.reloads += 1;
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

