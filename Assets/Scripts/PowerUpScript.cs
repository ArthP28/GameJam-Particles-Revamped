using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [SerializeField] string PowerUpName;
    [SerializeField] Bullet P1Bullet;
    [SerializeField] Bullet P2Bullet;
    [SerializeField] float timeToFire;
    [SerializeField] int powerUpDuration = 15;

    CinemachineTargetGroup _cameraTargetGroup;

    private void Awake()
    {
        _cameraTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
        _cameraTargetGroup.AddMember(transform, 1, 0);
    }

    private void Start()
    {
        StartCoroutine(PowerUpLife());
    }

    private void OnDestroy()
    {
        _cameraTargetGroup.RemoveMember(transform);
    }

    IEnumerator PowerUpLife()
    {
        yield return new WaitForSeconds(powerUpDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>())
        {
            PlayerInput playerPickingUp = collision.GetComponent<PlayerInput>();
            if (playerPickingUp.playerNum == PlayerNumber.Player1) // Player 1
            {
                collision.GetComponent<Weapon>().UpgradeWeapon(P1Bullet, timeToFire, powerUpDuration, PowerUpName);
                Debug.Log("Player 1 obtains " + name);
                Destroy(gameObject);
            } else if (playerPickingUp.playerNum == PlayerNumber.Player2) // Player 2
            {
                collision.GetComponent<Weapon>().UpgradeWeapon(P2Bullet, timeToFire, powerUpDuration, PowerUpName);
                Debug.Log("Player 2 obtains " + name);
                Destroy(gameObject);
            }
        }
    }
}
