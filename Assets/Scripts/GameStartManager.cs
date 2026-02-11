using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager Instance { get; private set; }

    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float reloadDelay = 5f;

    public event Action OnGameStarted;
    public event Action OnGameEnded;

    private bool gameStarted = false;
    private bool isReloading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Called by Start button
    public void OnStartButtonPressed()
    {
        if (gameStarted) return;

        gameStarted = true;

        OnGameStarted?.Invoke();
    }

    // Call this when player loses
    public void OnPlayerLost()
    {
        if (isReloading) return;

        OnGameEnded?.Invoke();
        StartCoroutine(ReloadStartSceneAfterDelay());
    }

    private IEnumerator ReloadStartSceneAfterDelay()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadDelay);

        gameStarted = false;
        isReloading = false;

        SceneManager.LoadScene(gameSceneName);
    }
}