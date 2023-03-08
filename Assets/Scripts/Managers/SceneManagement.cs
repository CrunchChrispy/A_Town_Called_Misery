using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
	private Canvas MainMenu;
	private Canvas PauseMenu;
	private Canvas OptionsMenu;
	
	
	void Start(){
		
		Scene scene = SceneManager.GetActiveScene();
		
		if(scene.buildIndex == 1){
			
			OptionsMenu.enabled = false;
		}
		

		OptionsMenu = GameObject.Find("Options Menu").GetComponent<Canvas>();
		OptionsMenu.enabled = false;

		PauseMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();
		PauseMenu.enabled = false;
		
		if(scene.buildIndex == 0){
			MainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
		}
		else{
			MainMenu = null;
		}
	}
	
	
	public void StartLevel(int level)
    {      
        SceneManager.LoadScene(level);      
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
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
		MainMenu.enabled = false;
	}
	public void CloseOptions(){
		OptionsMenu.enabled = false;
		MainMenu.enabled = true;
	}
	

		

		

		
		
	


    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Main_Menu")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
