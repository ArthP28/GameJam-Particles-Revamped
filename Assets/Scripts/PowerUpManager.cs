using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] PickUp[] _powerups;
    [SerializeField] float minimumDuration = 20f;
    [SerializeField] float maximumDuration = 60f;
    PowerupSpawner[] allSpawnerZones; // All the areas where a powerup will spawn
    SurvivalBattleScript _game;
    // Start is called before the first frame update
    void Awake()
    {
        _game = FindObjectOfType<SurvivalBattleScript>();
        allSpawnerZones = GetComponentsInChildren<PowerupSpawner>();
    }

    private void Start()
    {
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnPowerUp() // Have the game generate a random powerup in a random part of the game arena.
    {
        while (!_game.Player1Won() && !_game.Player2Won())
        {
            float randomInterval = Random.Range(minimumDuration, maximumDuration); // Set up random time
            int randPowerUpIndex = Random.Range(0, _powerups.Length); // Set up the powerup that will spawn
            int randZoneIndex = Random.Range(0, allSpawnerZones.Length); // Pick a zone that will spawn the powerup

            PickUp randomPowerUp = _powerups[randPowerUpIndex];
            PowerupSpawner randomZone = allSpawnerZones[randZoneIndex];

            yield return new WaitForSeconds(randomInterval); // After a random amount of time has passed, spawn the powerup

            if(!_game.Player1Won() && !_game.Player2Won())
            {
                randomZone.SpawnPowerUp(randomPowerUp); // Once the powerup has spawned, restart the time and spawn another powerup again

                Debug.Log(randomPowerUp + "Spawned after " + (int)randomInterval + " seconds.");
            }
        }
    }
}
