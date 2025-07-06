using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] PickUp[] _items;
    [SerializeField] bool respawnable = false;
    [SerializeField] float _timeBeforeRespawn = 30f;
    [SerializeField] ParticleSystem _debris;
    BoxCollider2D _crate;
    Vector2 respawnPosition;

    void Start()
    {
        respawnPosition = transform.position;
        _crate = GetComponentInChildren<BoxCollider2D>();
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet.GetComponent<Bomb>())
            {
                bullet.GetComponent<Bomb>().Explode();
            } else
            {
                bullet.Miss();
                bullet.DetermineBulletDestruction();
            }
            DestroyCrate();
        } else if (collision.gameObject.GetComponent<BombExplosion>())
        {
            DestroyCrate();
        }
    }
    */

    public void DestroyCrate()
    {
        Debug.Log("Crate Destroyed");
        Instantiate(_debris, _crate.transform.position, Quaternion.identity);
        DropRandomItem();
        if (respawnable)
        {
            StartCoroutine(Respawn());
            _crate.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DropRandomItem()
    {
        if (_items.Length > 0)
        {
            int randItemIndex = Random.Range(0, _items.Length); // Set up the powerup that will spawn
            PickUp randomItem = _items[randItemIndex];
            if (randomItem)
            {
                Instantiate(randomItem, new Vector3(_crate.transform.position.x, _crate.transform.position.y + 1f, 0f), Quaternion.identity);
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_timeBeforeRespawn);
        _crate.gameObject.SetActive(true);
        _crate.transform.position = respawnPosition;
        _crate.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _crate.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        _crate.GetComponent<Rigidbody2D>().rotation = 0f;
    }
}
