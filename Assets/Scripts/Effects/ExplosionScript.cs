using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] float _forceRadius = 10f; // The range in which objects will be forced back
    [SerializeField] float _force = 30f;

    // This script mainly covers the explosion's animation and cleaning it up
    ParticleSystem _mainParticles; // Main particle system of the parent

    AudioSource _source; // For Explosion Sound

    // Start is called before the first frame update
    void Awake()
    {
        _mainParticles = GetComponent<ParticleSystem>();
        _source = GetComponent<AudioSource>();
        _source.Play();
        if (_forceRadius > 0 && _force > 0)
        {
            ProduceForcePulse();
        }
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

    void ProduceForcePulse()
    {
        Collider2D[] _objectsToForce = Physics2D.OverlapCircleAll(transform.position, _forceRadius);
        foreach (Collider2D _obj in _objectsToForce)
        {
            if (_obj.GetComponent<Rigidbody2D>() && !_obj.GetComponent<Bullet>()) // Push back all enemies and the player
            {
                Rigidbody2D rb = _obj.GetComponent<Rigidbody2D>();
                StartCoroutine(ApplyKnockBack(rb, transform.position, _force));
            }
        }
    }

    IEnumerator ApplyKnockBack(Rigidbody2D obj_rb, Vector3 direction, float thrust)
    {
        if (obj_rb)
        {
            Vector3 force = (obj_rb.transform.position - direction).normalized * thrust * obj_rb.mass;
            obj_rb.AddForce(force, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
            obj_rb.velocity = Vector2.zero; // After _knockBackTime seconds, stop the knockback
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _forceRadius);
    }

}
