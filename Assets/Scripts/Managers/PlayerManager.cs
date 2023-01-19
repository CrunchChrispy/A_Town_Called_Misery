using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Gun Gun;
    [Header("Colt 45 Single Action Army Revolver")]
    public bool revolver;
    public int revolverAmmo;
    public GameObject Revolver;

 /*   [Header("Winchester Rifle")]
    public bool rifle;
    public int rifleAmmo;
    public GameObject Rifle;
	*/
    // Start is called before the first frame update
    void Start()
    {
        revolver = false;
        Revolver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
