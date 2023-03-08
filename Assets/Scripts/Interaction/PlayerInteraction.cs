using System;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	public float interactionDistance;

	//public InspectObject InspectObject;

	public TextMeshProUGUI interactionText;

	private Camera cam;

	private void Start()
	{
		cam = Camera.main;
		Cursor.visible = false;
		interactionText = GameObject.Find("Interaction Text").GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		Ray ray = cam.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f));
		bool flag = false;
		if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance))
		{
			Interactable component = hitInfo.collider.GetComponent<Interactable>();
			if (component != null)
			{
				HandleInteraction(component);
				interactionText.text = component.GetDescription();
				flag = true;
			}
		}
		if (!flag)
		{
			interactionText.text = "";
		}
	}

	private void HandleInteraction(Interactable interactable)
	{
		
		switch (interactable.interactionType)
		{
			case Interactable.InteractionType.Minigame:
				break;
			case Interactable.InteractionType.Click:
				if (UnityEngine.Input.GetButtonDown("Interact"))
				{
					interactable.Interact();
				}
				break;
			default:
				throw new Exception("Unsupported type of interactable.");
		}
	}
}
