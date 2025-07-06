using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float _timeBeforeFall = 3f;
    [SerializeField] float _timeBeforeRespawn = 5f;
    Rigidbody2D _rb;
    AudioSource _platformHitSound;
    Coroutine _fallCoRoutine;
    Vector3 _spawnPosition;
    bool fallActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInChildren<Rigidbody2D>();
        _platformHitSound = GetComponent<AudioSource>();
        _spawnPosition = transform.position;
        _rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fallActive)
        {
            _fallCoRoutine = StartCoroutine(FallPlatform());
        }
    }

    IEnumerator FallPlatform()
    {
        Debug.Log("Coroutine start");
        fallActive = true;
        _platformHitSound.Play();
        yield return new WaitForSeconds(_timeBeforeFall);
        _rb.isKinematic = false;
        yield return new WaitForSeconds(_timeBeforeRespawn);
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        transform.position = _spawnPosition;
        transform.rotation = Quaternion.identity;
        fallActive = false;
    }
}
