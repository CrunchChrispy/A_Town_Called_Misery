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
    public PlayerManager PlayerManager;
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
        if (PlayerManager.revolver == false)
        {
            PlayerManager.Weapon.SetActive(false);
            AmmoCount.enabled = false;
        }
        else
        {
            PlayerManager.Weapon.SetActive(true);
            AmmoCount.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            Pause.enabled = true;

        }
    }
}
