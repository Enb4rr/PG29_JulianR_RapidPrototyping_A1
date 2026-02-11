using System.Collections;
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
    
    [Header("Difficulty")]
    [SerializeField] private float difficultyIncreaseRate = 0.05f;
    [SerializeField] private float minSpawnInterval = 0.5f;

    [SerializeField] private float maxSpeedMultiplierLimit = 2.5f;

    private float difficultyTimer = 0f;
    private float currentSpawnInterval;
    
    private Transform player;
    private Coroutine spawnRoutine;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;

        currentSpawnInterval = spawnInterval;

        GameStartManager.Instance.OnGameStarted += () =>
        {
            ResetDifficulty();
            spawnRoutine = StartCoroutine(SpawnLoop());
        };
    }
    
    private void Update()
    {
        difficultyTimer += Time.deltaTime;

        currentSpawnInterval = Mathf.Max(
            minSpawnInterval,
            spawnInterval - difficultyTimer * difficultyIncreaseRate
        );
    }
    
    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
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
        float difficultySpeedBoost = 1f + difficultyTimer * 0.05f;
        difficultySpeedBoost = Mathf.Min(difficultySpeedBoost, maxSpeedMultiplierLimit);

        float speedMultiplier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier) * difficultySpeedBoost;
        
        asteroid.Launch(dir, speedMultiplier);
    }
    
    private void ResetDifficulty()
    {
        difficultyTimer = 0f;
        currentSpawnInterval = spawnInterval;
    }
}