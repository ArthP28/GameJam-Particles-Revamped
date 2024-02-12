using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHazard : MonoBehaviour
{
    [SerializeField] int _damage = 0;
    [SerializeField] bool OneHitKill = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Health>() != null)
        {
            if (OneHitKill)
            {
                _damage = collision.GetComponent<Health>().GetCurrentHealth();
            }
            collision.GetComponent<Health>().DealDamage(_damage);
        }
    }
}
