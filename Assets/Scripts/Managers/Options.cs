using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
	public Toggle FullscreenTog, vsnycTog, FPSTog;
	public List<ResSettings> resolutions = new List<ResSettings>();
	public Canvas FPSCounter;
	public TextMeshProUGUI resolutionLabel;
	private int selectedRes;
	// Start is called before the first frame update
	public AudioMixer theMixer;
	public TextMeshProUGUI mastLabel, musicLabel, sfxLabel;
	public Slider mastSlider, musicSlider, sfxSlider;
    void Start()
    {
	    FullscreenTog.isOn = Screen.fullScreen;
		
	    FPSTog.isOn = FPSCounter.enabled;
	    if(QualitySettings.vSyncCount == 0){
	    	vsnycTog.isOn = false;
	    }
	    else{
	    	vsnycTog.isOn = true;
	    }
	    
	    mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
	    musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
	    sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
	    updateResLabel();
	    
    }
    
	public void leftButton(){
		
		selectedRes--;
		if(selectedRes < 0){
			selectedRes = 0;
		}
		updateResLabel();	
	}
    
	public void rightButton(){
		selectedRes++;

		if(selectedRes > resolutions.Count - 1){
			
			selectedRes = resolutions.Count - 1;
		}
		
		updateResLabel();	

	}
	
	public void NextMenu(){
		
	}
	

	public void updateResLabel(){
		resolutionLabel.text = resolutions[selectedRes].HorizontalRes.ToString() + "x" + resolutions[selectedRes].VerticalRes.ToString();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void ApplyGraphics(){
		
		Screen.fullScreen = FullscreenTog.isOn;
		FPSCounter.enabled = FPSTog.isOn;
		
		if(vsnycTog.isOn){
			QualitySettings.vSyncCount = 1;
		}
		else{
			QualitySettings.vSyncCount = 0;
		}
		
		Screen.SetResolution(resolutions[selectedRes].HorizontalRes, resolutions[selectedRes].VerticalRes, FullscreenTog.isOn);
	}
	
	public void SetMasterVol(){
		mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
		theMixer.SetFloat("Master", mastSlider.value);
		PlayerPrefs.SetFloat("Master", mastSlider.value);
	}
	public void SetMusicVol(){
		musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
		theMixer.SetFloat("Music", musicSlider.value);
		PlayerPrefs.SetFloat("Music", musicSlider.value);
	}
	public void SetSFXVol(){
		sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
		theMixer.SetFloat("SFX", sfxSlider.value);
		PlayerPrefs.SetFloat("SFX", sfxSlider.value);
	}
}
[System.Serializable]
public class ResSettings{
	public int VerticalRes, HorizontalRes;
}
