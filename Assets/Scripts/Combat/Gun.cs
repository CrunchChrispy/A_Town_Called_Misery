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

    public int maxAmmo;
    private int currentAmmo;
    public float reloadTime;
    private bool isReloading = false;

    public Text text;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    AIMovement AIMovement;
    PlayerMovement PlayerMovement;

    //public Animator animator;

    void Start()
    {
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
        text.text = currentAmmo + "/" + maxAmmo;
        currentAmmo = maxAmmo;
        AIMovement = GetComponent<AIMovement>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        text.text = currentAmmo + "/" + maxAmmo;

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
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
        }

    void Shoot()
    {
       muzzleFlash.Play();
       currentAmmo--;
       RaycastHit hit;

       if( Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
                if(target != null && gameObject.tag == "Enemy" || gameObject.tag == "Player")
            {
                target.TakeDamage(damage);
            }
                if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject ImpactOBJ = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactOBJ, 2f);
        }
    }
}
