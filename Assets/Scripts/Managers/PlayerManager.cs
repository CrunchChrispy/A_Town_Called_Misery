using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public bool spotted;
	//public AIMovement AIMovement;
	public AIMovement[] Enemies;
	//public GameObject[] Enemies;
	//public bool[] isChasing;
    void Start()
	{


    }

    void Update()
	{
		Enemies = FindObjectsOfType<AIMovement>();

		Debug.Log(Enemies.Length + " Enemies Remaining");
		
		
		
        
    }
}
