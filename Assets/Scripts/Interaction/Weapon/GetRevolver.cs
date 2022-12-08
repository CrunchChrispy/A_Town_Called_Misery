using UnityEngine;

public class GetRevolver : Interactable
{
	//public GameObject Revolver;
	public PlayerManager PlayerManager;
	private bool gotRevolver;
	

	private void Start()
	{
		PlayerManager.revolver = false;
		this.gameObject.SetActive(value: true);
		UpdateLight();
	}

	private void UpdateLight()
	{
		if (gotRevolver)
		{
			PlayerManager.revolver = true;
			this.gameObject.SetActive(value: false);
		}
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
