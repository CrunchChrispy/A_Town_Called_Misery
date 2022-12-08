using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public enum InteractionType
	{
		Click,
		Hold,
		Minigame
	}

	private float holdTime;

	public InteractionType interactionType;

	public abstract string GetDescription();

	public abstract void Interact();

	public void IncreaseHoldTime()
	{
		holdTime += Time.deltaTime;
	}

	public void ResetHoldTime()
	{
		holdTime = 0f;
	}

	public float GetHoldTime()
	{
		return holdTime;
	}
}
