using System;
using UnityEngine;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameWon += Show;
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted += Hide;
    }

    private void OnEnable()
    {
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameWon += Show;
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted += Hide;
    }

    private void OnDisable()
    {
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameWon -= Show;
        if (GameStartManager.Instance != null) GameStartManager.Instance.OnGameStarted -= Hide;
    }

    void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        GameStartManager.Instance.OnPlayerLost();
    }
}