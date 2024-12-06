using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCoins : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private Vector2 sizeSpawner = new Vector2(10, 10);
    [SerializeField] private float spawnInterval = 3;

    private void Start()
    {
        InvokeRepeating(nameof(spawningCoins), 0f, spawnInterval);
    }

    private void spawningCoins()
    {
        float randomX = Random.Range(-sizeSpawner.x / 2, sizeSpawner.x / 2);
        float randomZ = Random.Range(-sizeSpawner.y / 2, sizeSpawner.y / 2);

        Vector3 spawnPosition = new Vector3(randomX, 0.1f, randomZ);

        Instantiate(coin, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(sizeSpawner.x, 0.1f, sizeSpawner.y));
    }
}
