using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    [SerializeField] protected int powerUpDuration = 15;

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
}
