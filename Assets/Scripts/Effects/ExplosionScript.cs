using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // This script mainly covers the explosion's animation and cleaning it up
    ParticleSystem _mainParticles; // Main particle system of the parent

    AudioSource _source; // For Explosion Sound

    // Start is called before the first frame update
    void Awake()
    {
        _mainParticles = GetComponent<ParticleSystem>();
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // This explosion object is destroyed as soon as it stops playing to save memory
        if (_mainParticles.isStopped)
        {
            Destroy(gameObject);
        }
    }


}
