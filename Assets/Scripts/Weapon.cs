using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    AudioSource m_AudioSource;
    [SerializeField] Transform firePoint;
    [SerializeField] int damage = 20;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] AudioClip shootingSound;
    //[SerializeField] GameObject bullet;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        //Instantiate(bullet, firePoint.position, firePoint.rotation);
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(shootingSound);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right);


        if (hit)
        {
            // Take Damage to Object Collided
            Debug.Log(hit.transform.name);

            //Instantiate(laser, hit.point, Quaternion.identity); // For Impact Effect

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;


    }
}
