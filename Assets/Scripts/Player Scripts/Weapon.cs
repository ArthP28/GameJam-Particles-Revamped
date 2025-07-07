using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

[RequireComponent(typeof(BulletPool))]
public class Weapon : MonoBehaviour
{
    // Serializeables
    [SerializeField] AudioClip shootingSound; // Laser Sound
    [SerializeField] AudioClip gainPowerSound;
    [SerializeField] AudioClip losePowerSound;
    [SerializeField] float timeBeforeNextFire = 0.07f; // The next bullet is fired after a certain amount of time has passed
    
    Coroutine PowerUpRoutine; // The IEnumerator function that will play when the player obtains a powerup
    Coroutine DisplayMessageRoutine;
    AudioSource m_AudioSource; // Required for Sound to work
    float reserveTime; // The original fire time is preserved in this variable while the powerup is still in effect

    bool alreadyFired = false;
    bool isActivated = true;

    BulletPool _bulletPool; // All bullets are handled in this object pool

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        _bulletPool = GetComponent<BulletPool>();
        reserveTime = timeBeforeNextFire;
    }
    public void Fire()
    {
        if (!alreadyFired) // If the bullet is already fired, then the player cannot shoot another one until time has passed
        {
            StartCoroutine(Shoot()); // Begin shooting as soon as the fire button is pressed
        }
    }

    public void UpgradeWeapon(Bullet newBullet, float newTime, int powerDuration, string PowerName) // The player obtains the new weapon until either the powerup wears off or the player dies
    {
        m_AudioSource.PlayOneShot(gainPowerSound);
        // The bullet and its firing time changes to the new powerup
        _bulletPool.SwitchBullet(newBullet);
        timeBeforeNextFire = newTime;

        // If a powerup is already in effect, restart the PowerUp Coroutine.
        // This allows for the player's powerup to be kept without losing it too early
        if (PowerUpRoutine != null) 
        {
            StopCoroutine(PowerUpRoutine);
        }
        PowerUpRoutine = StartCoroutine(PowerUpDuration(powerDuration));

        _bulletPool.SetCurrentPowerUpTag(PowerName);
    }

    IEnumerator PowerUpDuration(int powerUpTimeLimit)
    {
        yield return new WaitForSeconds(powerUpTimeLimit);
        LoseUpgrade();
    }

    public void LoseUpgrade() // The player does not have the powerup anymore
    {
        if (_bulletPool.GetCurrentBullet() != _bulletPool.GetReserveBullet())
        {
            GetComponent<PlayerNew>().RemovePowerUpTimer();
            GetComponent<PlayerNew>().DeleteMessage();
            m_AudioSource.PlayOneShot(losePowerSound);
            _bulletPool.RevertToDefaultBullet();
            timeBeforeNextFire = reserveTime;
        }
    }

    IEnumerator Shoot() // Fire a bullet from the player's gun
    {
        Bullet _bullet = _bulletPool.GetPool().Get(); // Instead of creating a new bullet, pick one up from the pool
        _bullet.SetPool(_bulletPool); // Make sure to set the bullet's pool so it knows where to return

        // Plays Bullet Sound
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(shootingSound);

        // Player must wait a certain amount of time before firing again
        alreadyFired = true;
        yield return new WaitForSeconds(timeBeforeNextFire);
        alreadyFired = false;


    }

    public bool getActivated()
    {
        return isActivated;
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
    }
}
