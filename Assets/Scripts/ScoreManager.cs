using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TMP_Text scoreText;
    
    private float survivalTime;
    private bool running;

    private int score;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    private void Start()
    {
        GameStartManager.Instance.OnGameStarted += StartRun;
        GameStartManager.Instance.OnGameEnded += EndRun;
    }

    private void OnEnable()
    {
        if (GameStartManager.Instance == null) return;
        
        GameStartManager.Instance.OnGameStarted += StartRun;
        GameStartManager.Instance.OnGameEnded += EndRun;
    }

    private void OnDisable()
    {
        if (GameStartManager.Instance == null) return;
        
        GameStartManager.Instance.OnGameStarted -= StartRun;
        GameStartManager.Instance.OnGameEnded -= EndRun;
    }
    
    private void Update()
    {
        if (!running) return;

        survivalTime += Time.deltaTime;
        UpdateUI();
    }
    
    void StartRun()
    {
        survivalTime = 0f;
        running = true;
    }

    void EndRun()
    {
        running = false;
    }
    
    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Time: " + Mathf.FloorToInt(survivalTime);
    }
}