using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint; // Source of laser being fired
    [SerializeField] int damage = 10; // Amount of damage dealt to targets/enemies
    [SerializeField] float range = 16; // How far the laser goes
    [SerializeField] LineRenderer lineRenderer; // Actual Laser
    [Tooltip("Element 0: Default Impact - Used when player shoots a wall\n" +
        "Element 1: Damage Impact - Explosion is colored to the player's laser when hitting something that takes in damage")]
    [SerializeField] GameObject[] impactEffects = new GameObject[2];

    AudioSource m_AudioSource; // Required for Sound to work
    [SerializeField] AudioClip shootingSound; // Laser Sound
    //[SerializeField] GameObject bullet; // Used for bullet effect (unused)

    [SerializeField] float timeBeforeNextFire = 0.07f;
    bool alreadyFired = false;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetButtonDown("Fire1") && !alreadyFired)
        {
            StartCoroutine(Shoot()); // Begin shooting as soon as the fire button is pressed
        }
    }*/

    public void Fire()
    {
        if (!alreadyFired)
        {
            StartCoroutine(Shoot()); // Begin shooting as soon as the fire button is pressed
        }
    }

    IEnumerator Shoot()
    {
        //Instantiate(bullet, firePoint.position, firePoint.rotation); // Used for bullet effect (unused)

        // Plays Bullet Sound
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(shootingSound);

        alreadyFired = true;

        // A Laser raycast is fired out from the player's firepoint
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range);

        if (hit) // When the laser hits something
        {
            Debug.Log(hit.transform.name);

            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Object takes damage if it has health
                targetHealth.DealDamage(damage);
                Instantiate(impactEffects[1], hit.point, Quaternion.identity); // Colored Explosion - Player hit
            }
            else
            {
                Instantiate(impactEffects[0], hit.point, Quaternion.identity); // Default Explosion - Miss
            }

            // These two lines of code determines how far the laser travelled before hitting the object.
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else // When it misses any object
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * range);
        }

        // Laser appears for a fraction of a second before disappearing again
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.035f);

        lineRenderer.enabled = false;

        yield return new WaitForSeconds(timeBeforeNextFire);
        alreadyFired = false;


    }
}
