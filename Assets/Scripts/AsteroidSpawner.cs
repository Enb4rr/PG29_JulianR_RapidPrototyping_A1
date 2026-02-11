using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnWidth = 12f;

    [Header("Randomization")]
    [SerializeField] private float minScale = 0.6f;
    [SerializeField] private float maxScale = 1.6f;

    [SerializeField] private float minSpeedMultiplier = 0.7f;
    [SerializeField] private float maxSpeedMultiplier = 1.4f;

    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;

        GameStartManager.Instance.OnGameStarted += () =>
            InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    private void SpawnAsteroid()
    {
        if (player == null) return;

        float randomX = Random.Range(-spawnWidth, spawnWidth);
        Vector3 spawnPos = transform.position + Vector3.right * randomX;

        // Random rotation
        float rotationZ = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);

        Asteroid asteroid = Instantiate(asteroidPrefab, spawnPos, rotation);

        // Random scale
        float scale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * scale;

        Vector2 dir = (player.position - spawnPos).normalized;

        // Random speed
        float speedMultiplier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
        asteroid.Launch(dir, speedMultiplier);
    }
}