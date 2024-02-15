using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [SerializeField] string PowerUpName;
    [SerializeField] GameObject P1Bullet;
    [SerializeField] GameObject P2Bullet;
    [SerializeField] float timeToFire;
    [SerializeField] float powerUpDuration = 15f;

    private void Start()
    {
        StartCoroutine(PowerUpLife());
    }

    IEnumerator PowerUpLife()
    {
        yield return new WaitForSeconds(powerUpDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player1Movement>())
        {
            collision.GetComponent<Player1Weapon>().UpgradeWeapon(P1Bullet, timeToFire, PowerUpName);
            Debug.Log(name + " obtained");
            Destroy(gameObject);
        } else if (collision.GetComponent<Player2Movement>())
        {
            collision.GetComponent<Player2Weapon>().UpgradeWeapon(P2Bullet, timeToFire, PowerUpName);
            Debug.Log(name + " obtained");
            Destroy(gameObject);
        }
    }
}
