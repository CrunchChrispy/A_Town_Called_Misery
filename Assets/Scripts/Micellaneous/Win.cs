using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Win : MonoBehaviour
{
	public PlayerMovement PlayerMovement;
	GameObject player;
	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
    
	void OnTriggerEnter(Collider collision){
		
		if(collision.gameObject == player){
			PlayerMovement.Win();
		
			//Destroy(collision.gameObject);
		}
		

		
	}
	// Update is called once per frame
	void Update()
	{
        
	}
}
