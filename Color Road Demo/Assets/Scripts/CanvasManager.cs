using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] CanvasGroup gameOverPanel;
    [SerializeField] CanvasGroup startText;
    [SerializeField] TextMeshProUGUI coinCountText;

    private void Start()
    {
        ScoreManager.instance.OnScoreUpdated += UpdateScore;
        UpdateScore(0);
        UpdateCoinCount(Coin.GetCoinCount());
    }

    void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void DisplayGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        Sequence fadeSeq = DOTween.Sequence();
        fadeSeq.SetDelay(1f).Append(gameOverPanel.DOFade(1f, 1f));
        fadeSeq.Play();
    }

    public void FadeAwayStartText()
    {
        startText.DOFade(0f, 1f);
    }

    public void UpdateCoinCount(int newCount)
    {
        coinCountText.text = $"Coins: {newCount}";
    }
}
