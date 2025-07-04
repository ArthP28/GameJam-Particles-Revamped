using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] PickUp[] _items;
    [SerializeField] bool respawnable = false;
    [SerializeField] float _timeBeforeRespawn = 30f;
    BoxCollider2D _crate;
    Vector2 respawnPosition;

    void Start()
    {
        respawnPosition = transform.position;
        _crate = GetComponentInChildren<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            bullet.Miss();
            bullet.DetermineBulletDestruction();
            Debug.Log("Crate Destroyed");
            DropRandomItem();
            if (respawnable)
            {
                _crate.gameObject.SetActive(false);
                StartCoroutine(Respawn());
            } else
            {
                Destroy(_crate.gameObject);
            }
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
        }
    }

    void DropRandomItem()
    {
        if (_items.Length > 0)
        {
            int randItemIndex = Random.Range(0, _items.Length); // Set up the powerup that will spawn
            PickUp randomItem = _items[randItemIndex];
            Instantiate(randomItem, new Vector3(transform.position.x, transform.position.y + 1f, 0f), Quaternion.identity);
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_timeBeforeRespawn);
        transform.position = respawnPosition;
        _crate.gameObject.SetActive(true);
    }
}
