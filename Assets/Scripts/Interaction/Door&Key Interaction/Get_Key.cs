using UnityEngine;

public class Get_Key : Interactable
{
	public GameObject Key;

	public bool gotKey;

	public bool key;

	private void Start()
	{
		key = false;
		Key.SetActive(value: true);
		UpdateLight();
	}

	private void UpdateLight()
	{
		if (gotKey)
		{
			key = true;
			Key.SetActive(value: false);
		}
	}

	public override string GetDescription()
	{
		if (gotKey)
		{
			return "";
		}
		return "Press [E] to collect key.";
	}

	public override void Interact()
	{
		gotKey = !gotKey;
		UpdateLight();
	}
}
