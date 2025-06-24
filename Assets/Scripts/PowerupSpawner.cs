using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] float MinxBoundary;
    [SerializeField] float MaxxBoundary;
    [SerializeField] float MinyBoundary;
    [SerializeField] float MaxyBoundary;
    Vector3 RandPosition;


    public void SpawnPowerUp(PickUp _randomPowerUp)
    {
        RandomizePosition();
        Instantiate(_randomPowerUp, RandPosition, transform.rotation);
    }

    void RandomizePosition()
    {
        RandPosition = new Vector3(Random.Range(MinxBoundary, MaxxBoundary), Random.Range(MinyBoundary, MaxyBoundary));
        //Debug.Log("Current Spawn Location: " + transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(MinxBoundary, MinyBoundary), new Vector2(MaxxBoundary, MinyBoundary)); // Bottom left to Bottom right
        Gizmos.DrawLine(new Vector2(MaxxBoundary, MinyBoundary), new Vector2(MaxxBoundary, MaxyBoundary)); // Bottom right to Top right
        Gizmos.DrawLine(new Vector2(MaxxBoundary, MaxyBoundary), new Vector2(MinxBoundary, MaxyBoundary)); // Top right to Top left
        Gizmos.DrawLine(new Vector2(MinxBoundary, MaxyBoundary), new Vector2(MinxBoundary, MinyBoundary)); // Top left to Bottom left
    }
}
