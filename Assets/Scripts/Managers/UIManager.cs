using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public Text AmmoCount; 
	public TextMeshProUGUI InteractionText;
    
    public Canvas HUD;
    public Canvas GameOver;

	private GameObject Damage1;
	private GameObject Damage2;
	private GameObject Damage3;
	
	public LightTracker LightTracker;
	public PlayerMovement PlayerMovement;
	public WeaponSwitching WeaponSwitching;
	public Slider slider;
	
	private int health;
	private int initialHealth;
	
	
	private Canvas MainMenu;
	private Canvas PauseMenu;
	private Canvas OptionsMenu;
	public Canvas Win;

	
	
	public void StartLevel(int level)
	{      
		SceneManager.LoadScene(level); 

	}
	public void ReloadLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	
	}
	public void ResumeGame()
	{
		Time.timeScale = 1;
	}
	public void QuitGame()
	{
		Application.Quit();
	}
	public void OpenOptions(){
		OptionsMenu.enabled = true;
		
		if(SceneManager.GetActiveScene().buildIndex == 0){
			MainMenu.enabled = false;
		}
		else{
			PauseMenu.enabled = false;
		}

	}
	public void CloseOptions(){
		OptionsMenu.enabled = false;
		if(SceneManager.GetActiveScene().buildIndex == 0){
			MainMenu.enabled = true;
		}
		else{
			PauseMenu.enabled = true;
		}
	}
	
	
    void Start()
	{
		
		if(FindObjectsOfType<DDOL>().Length > 1){
			Destroy(gameObject);
		}
		Scene scene = SceneManager.GetActiveScene();
    	
		GameOver = GameObject.Find("Game Over").GetComponent<Canvas>();	    
		PauseMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();  
		OptionsMenu = GameObject.Find("Options Menu").GetComponent<Canvas>();
		HUD = GameObject.Find("HUD").GetComponent<Canvas>();
		Win = GameObject.Find("Win").GetComponent<Canvas>();
		

		Damage1 = GameObject.Find("Damage 1");
		Damage2 = GameObject.Find("Damage 2");
		Damage3 = GameObject.Find("Damage 3");
		

		Damage1.SetActive(false);
		Damage2.SetActive(false);
		Damage3.SetActive(false);
		

		OptionsMenu.enabled = false;			
		GameOver.enabled = false;
		PauseMenu.enabled = false;
		Win.enabled = false;

	}
	void OntriggerEnter(){
		
	}
	

    void Update()
	{
		
		Scene scene = SceneManager.GetActiveScene();
		
		if (scene.buildIndex == 0)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			PauseMenu.enabled = false;
			GameOver.enabled = false;
			HUD.enabled = false;
			
			if(MainMenu == null){
				MainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
			}		
		}
		
		
		if(scene.buildIndex == 1){
			
		
			if(LightTracker == null){
				LightTracker = GameObject.Find("LightTracker").GetComponent<LightTracker>();
				Debug.Log("WHERE'S THE LIGHT BITCH");
			}
			
			if(PlayerMovement == null){
				PlayerMovement = GameObject.FindObjectOfType<PlayerMovement>();
				health = PlayerMovement.health;
				initialHealth = PlayerMovement.health;
				Debug.Log("WHERE'S THE PLAYER BITCH");
			}
			
			if(WeaponSwitching == null){
				WeaponSwitching = GameObject.FindObjectOfType<WeaponSwitching>();
				Debug.Log("WHERE'S THE WEAPONS BITCH");
			}
			
			
			//slider.value = LightDetection.s_fLightValue;
			slider.value = LightTracker.Light;
			//health = PlayerMovement.health;
        


			if (Time.timeScale == 0)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.Confined;
			}
			else if(PlayerMovement.health >= 1){
				
				HUD.enabled = true;
				PauseMenu.enabled = false;
				GameOver.enabled = false;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (PlayerMovement.revolver == false || WeaponSwitching.selectedWeapon != 0)
			{
				PlayerMovement.Revolver.SetActive(false);
				AmmoCount.enabled = false;
			}
			else
			{
				PlayerMovement.Revolver.SetActive(true);
				AmmoCount.enabled = true;
			}

			if (Input.GetButtonDown("Start"))
			{
				Time.timeScale = 0;
				PauseMenu.enabled = true;
				HUD.enabled = false;

			}
			if(PlayerMovement.health >= initialHealth){
				
				Damage3.SetActive(false);
				Damage2.SetActive(false);
				Damage1.SetActive(false);
				GameOver.enabled = false;
			}
			
				    
			if(PlayerMovement.health <= health - 1){
	    	
				Damage1.SetActive(true);
	    	
				if(PlayerMovement.health <= health - 2){
	    		
					Damage2.SetActive(true);
	    		
					if(PlayerMovement.health <= health - 3){
	    		
						Damage3.SetActive(true);
	    		
	    		
					}
					else{
						Damage3.SetActive(false);
					}
				}
				else{
					Damage2.SetActive(false);
				}
			}
			else{
				Damage1.SetActive(false);
			}
		}
    	

    }
}
