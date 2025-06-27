using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesSystem : MonoBehaviour
{
    [SerializeField] Health _health;
    [SerializeField] GameObject[] PlayerLives = new GameObject[3];

    int numLives;
    bool livesChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        numLives = PlayerLives.Length;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLives();
    }

    void CheckLives()
    {
        if (_health.GetCurrentHealth() <= 0 && !livesChanged) // When a player dies, remove the last life in the list and subtract the number of lives by 1.
        {
            PlayerLives[numLives - 1].SetActive(false);
            numLives--;
            livesChanged = true;
            if(numLives > 0)
            {
                if (_health.GetComponent<PlayerNew>())
                {
                    StartCoroutine(_health.GetComponent<PlayerNew>().WaitAndRespawn());
                }
            }
        }
        if (_health.GetCurrentHealth() > 0 && livesChanged)
        {
            livesChanged = false;
        }
    }

    public int GetCurrentLives()
    {
        return numLives;
    }

}
