using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnWidth = 8f;

    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    private void SpawnAsteroid()
    {
        if (player == null) return;

        float randomX = Random.Range(-spawnWidth, spawnWidth);
        Vector3 spawnPos = transform.position + Vector3.right * randomX;

        Asteroid asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        Vector2 dir = (player.position - spawnPos).normalized;
        asteroid.Launch(dir);
    }
}