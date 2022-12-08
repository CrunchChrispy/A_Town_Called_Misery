using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Weapon;
    public bool revolver;
    public int revolverAmmo;

    // Start is called before the first frame update
    void Start()
    {
        revolver = false;
        Weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
