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
    public Canvas Pause;
    public LightDetection LightDetection;
	public PlayerMovement PlayerMovement;
    public Slider slider;
    void Start()
    {
        GameOver.enabled = false;
        Pause.enabled = false;
    }
	

    void Update()
    {
        slider.value = LightDetection.s_fLightValue;

        if (Time.timeScale == 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {

            Pause.enabled = false;
            GameOver.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (PlayerMovement.revolver == false)
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
            Pause.enabled = true;

        }
    }
}
