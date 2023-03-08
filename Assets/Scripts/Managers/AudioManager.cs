using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class AudioManager : MonoBehaviour
{
	private PlayerMovement PlayerMovement;
	public  AudioClip[] DirtFootsteps;
	public  AudioClip[] WoodFootsteps;
	public  AudioClip[] StoneFootsteps;
	public  AudioClip gunShot;
	public AudioClip gunCock;
	

	private  int audioClipIndex;
	private  int[] previousArray;
	private  int previousArrayIndex;
	private int FootstepArrayindex;
	
	public void Start(){
		if(SceneManager.GetActiveScene().buildIndex == 1){
			PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
		}
		
	}
	public void Update(){
		if(PlayerMovement == null){
			PlayerMovement = GameObject.FindObjectOfType<PlayerMovement>();
			Debug.Log("WHERE'S THE PLAYER's FOOTSTEPS BITCH");
		}
	}


	public  AudioClip GetRandomAudioClip(AudioClip[] audioClipArray) {
		
		audioClipArray = PlayerMovement.currentAudioArray;
		
		if (previousArray == null) {
			// Sets the length to half of the number of AudioClips
			// This will round downwards
			// So it works with odd numbers like for example 3
			previousArray = new int[audioClipArray.Length / 2];
		}
		if (previousArray.Length == 0) {
			// If the the array length is 0 it returns null
			return null;
		} else {
			// Psuedo random remembering previous clips to avoid repetition
			do {
				audioClipIndex = Random.Range(0, audioClipArray.Length);
			} while (PreviousArrayContainsAudioClipIndex());
			// Adds the selected array index to the array
			previousArray[previousArrayIndex] = audioClipIndex;
			// Wrap the index
			previousArrayIndex++;
			if (previousArrayIndex >= previousArray.Length) {
				previousArrayIndex = 0;
			}
		}

		// Returns the randomly selected clip
		return audioClipArray[audioClipIndex];
	}


	// Returns if the randomIndex is in the array
	private  bool PreviousArrayContainsAudioClipIndex() {
		for (int i = 0; i < previousArray.Length; i++) {
			if (previousArray[i] == audioClipIndex) {
				return true;
			}
		}
		return false;
	}

	
}
