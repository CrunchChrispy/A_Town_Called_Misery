using UnityEngine;

public class LightSwitch : Interactable
{
	public GameObject m_Light;

	public bool isOn;

	private void Start()
	{
		UpdateLight();
	}

	private void UpdateLight()
	{
		m_Light.SetActive(isOn);
	}

	public override string GetDescription()
	{
		if (isOn)
		{
			return "Press [E] to turn <color=red>off</color>.";
		}
		return "Press [E] to turn <color=green>on</color>.";
	}

	public override void Interact()
	{
		isOn = !isOn;
		UpdateLight();
	}
}

