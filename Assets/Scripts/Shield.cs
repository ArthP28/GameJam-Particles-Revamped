using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int duration = 15;
    [SerializeField] PlayerNumber numOfOwner;
    [SerializeField] AudioClip activatingSound;
    [SerializeField] AudioClip deactivatingSound;

    AudioSource m_AudioSource; // Required for Sound to work
    Coroutine shieldCoroutine;

    private void Start()
    {
        ActivateShield();
    }

    public void ActivateShield()
    {
        if (GetComponentInParent<AudioSource>())
        {
            m_AudioSource = GetComponentInParent<AudioSource>();
            m_AudioSource.PlayOneShot(activatingSound);
        }
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldLifeTime());
    }

    IEnumerator ShieldLifeTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GetComponentInParent<PlayerNew>())
        {
            GetComponentInParent<PlayerNew>().RemoveEffectTimer();
            GetComponentInParent<PlayerNew>().DeleteMessage();
        }
        m_AudioSource.PlayOneShot(deactivatingSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if((numOfOwner == PlayerNumber.Player1 && bullet.bulletOfPlayersWeapon == PlayerNumber.Player2) ||
                (numOfOwner == PlayerNumber.Player2 && bullet.bulletOfPlayersWeapon == PlayerNumber.Player1))
            {
                if (bullet.GetComponent<Bomb>())
                {
                    bullet.GetComponent<Bomb>().Explode();
                }
                else
                {
                    bullet.Miss();
                }
                bullet.DetermineBulletDestruction();
            }
        }
    }
}
