using UnityEngine;

public class Lights0ff : MonoBehaviour
{
	public GameObject light1;

	public GameObject light2;

	private void OnTriggerExit(Collider collision)
	{
		light1.SetActive(value: false);
		light2.SetActive(value: false);
	}
}
