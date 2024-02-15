using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _powerups;
    [SerializeField] float MinxBoundary;
    [SerializeField] float MaxxBoundary;
    [SerializeField] float MinyBoundary;
    [SerializeField] float MaxyBoundary;
    [SerializeField] float minimumDuration = 20f;
    [SerializeField] float maximumDuration = 60f;
    // Start is called before the first frame update
    void Start()
    {
        RandomizePosition();
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnPowerUp()
    {
        while(!FindObjectOfType<SurvivalBattleScript>().Player1Won() || !FindObjectOfType<SurvivalBattleScript>().Player2Won())
        {
            Debug.Log("Waiting");
            float randomInterval = Random.Range(minimumDuration, maximumDuration);
            yield return new WaitForSeconds(randomInterval);
            Instantiate(_powerups[0], transform.position, transform.rotation);
            RandomizePosition();
        }
        
    }

    void RandomizePosition()
    {
        transform.position = new Vector3(Random.Range(MinxBoundary, MaxxBoundary), Random.Range(MinyBoundary, MaxyBoundary));
        Debug.Log("Current Spawn Location: " + transform.position);
    }
}
