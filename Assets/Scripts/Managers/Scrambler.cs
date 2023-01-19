using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Scrambler
{
	public static void Shuffle<T> (this T[] array, int shuffleAccuracy){
		for(int i = 0; i < shuffleAccuracy; i++){
			int randomIndex = Random.Range (0, array.Length);
			
			T temp = array [randomIndex];
			array [randomIndex] = array [0];
			array [0] = temp;
		}
	}
}
