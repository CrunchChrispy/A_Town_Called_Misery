using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;
    public float impactForce;
    public int reloads;
    public int maxAmmo;
    public float reloadTime;


    public UIManager UIManager;
    public Camera fpsCam;
    //public GameObject impactEffect;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    ParticleSystem muzzleFlash;
    AIMovement AIMovement;
    PlayerMovement PlayerMovement;






    //public Animator animator;

    void Start()
    {
        currentAmmo = 0;
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
        UIManager.AmmoCount.text = currentAmmo + "/" + maxAmmo;

        AIMovement = GetComponent<AIMovement>();
        PlayerMovement = GetComponent<PlayerMovement>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();

    }

    void Update()
    {
        UIManager.AmmoCount.text = currentAmmo + "/" + maxAmmo;

        if (isReloading)
            return;

        if (currentAmmo <= 0 && reloads >= 1)
        {
            StartCoroutine(Reload());
            return;
        }


        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo >= 1)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        //animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        //animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
        reloads -= 1;
    }

    void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;
        RaycastHit hit;
        if (Time.timeScale == 1)
        {
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                //GameObject ImpactOBJ = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(ImpactOBJ, 2f);
            }
        }
    }
}
