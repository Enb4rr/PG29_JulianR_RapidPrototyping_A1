using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [Header("Game Goal")]
    [SerializeField] private int targetScore = 1000;
    
    [Header("Streak")]
    [SerializeField] private float multiplierIncrease = 0.2f;
    [SerializeField] private float maxMultiplier = 5f;

    private float multiplier = 1f;

    [Header("Components")]
    [SerializeField] private TMP_Text scoreText;

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
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted += ResetScore;
    }

    private void OnEnable()
    {
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted += ResetScore;
    }

    private void OnDisable()
    {
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted -= ResetScore;
    }

    public void AddScore(int amount)
    {
        int finalScore = Mathf.RoundToInt(amount * multiplier);
        score += finalScore;

        IncreaseMultiplier();
        UpdateUI();

        if (score >= targetScore)
            WinGame();
    }
    
    void ResetScore()
    {
        score = 0;
        UpdateUI();
    }
    
    void IncreaseMultiplier()
    {
        multiplier += multiplierIncrease;
        multiplier = Mathf.Min(multiplier, maxMultiplier);
    }

    public void ResetMultiplier()
    {
        multiplier = 1f;
    }
    
    void WinGame()
    {
        GameStartManager.Instance.OnPlayerWon();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text =
                "Score: " + score +
                "   x" + multiplier.ToString("0.0");
    }
}