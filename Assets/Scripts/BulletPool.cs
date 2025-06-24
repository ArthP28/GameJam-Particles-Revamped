using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    /* This is a more efficient way of storing a player's projectiles than simply creating and destroying. 
     * With some exceptions, all of the player's bullets are stored in this pool ready to be used again upon firing. */
    ObjectPool<Bullet> _bulletPool;
    [SerializeField] Bullet _bullet; // The bullet that will be stored in this pool. The player's gun will use this upon firing.
    Bullet reserveBullet; // The original bullet is stored in this variable while a powerup is still in effect.
    [SerializeField] Transform firePoint; // Source of laser being fired

    bool powerUpActivated = false;
    bool powerUpReplaced = false;
    string tagOfCurrentBullet = "Default";

    // Start is called before the first frame update
    void Awake()
    {
        reserveBullet = _bullet;
        CreateBulletPool();
    }


    void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            // Create new bullet object
            return Instantiate(_bullet, firePoint.position, firePoint.rotation);
        }, _bullet =>
        {
            // Set the bullet's position and extract it from the Pool
            _bullet.transform.position = firePoint.position;
            _bullet.transform.rotation = firePoint.rotation;
            _bullet.gameObject.SetActive(true);
        }, _bullet =>
        {
            // Instead of destroying it, simple deactivate the bullet until it can be used again
            _bullet.gameObject.SetActive(false);
            Debug.Log("Bullets of pool after deletion: " + _bulletPool.CountAll);
        }, _bullet =>
        {
            // Destroy object if there are too many objects held within the pool
            Destroy(_bullet.gameObject);
        }, false, 15, 30);
        // Collection Check: Checks if the object really did return to the pool (Return should not be called to an already returned object)
        // Default Capacity: How many objects can the pool hold?
        // Max Size: How much larger can the pool be to accommodate extra objects
    }

    public ObjectPool<Bullet> GetPool()
    {
        return _bulletPool;
    }

    public void SwitchBullet(Bullet newBullet) // Swap the current bullet with the powerup
    {
        _bulletPool.Clear(); // Make sure to clean out the pool of any inactive objects first (Any active bullets being released into the pool are handled separately)
        if (powerUpActivated)
        {
            powerUpReplaced = true;
        } else
        {
            powerUpActivated = true;
        }
        _bullet = newBullet;
    }

    public void RevertToDefaultBullet() // Upon the powerup's deactivation, switch the bullet back to the original.
    {
        _bulletPool.Clear(); // Make sure to clean out the pool of any inactive objects first (Any active bullets being released into the pool are handled separately)
        powerUpReplaced = false;
        powerUpActivated = false;
        _bullet = reserveBullet;
        tagOfCurrentBullet = "Default";
    }

    // Getters/Setters

    public void SetCurrentPowerUpTag(string newTag)
    {
        tagOfCurrentBullet = newTag;
    }

    public Bullet GetCurrentBullet()
    {
        return _bullet;
    }

    public Bullet GetReserveBullet()
    {
        return reserveBullet;
    }

    public bool powerUpEnabled()
    {
        return powerUpActivated;
    }

    public string getTagOfCurrentBullet()
    {
        return tagOfCurrentBullet;
    }
}
