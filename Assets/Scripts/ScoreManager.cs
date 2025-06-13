using System;
using UnityEngine;

public sealed class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindFirstObjectByType<ScoreManager>();
            if (_instance != null) return _instance;
            var go = new GameObject(nameof(ScoreManager));
            _instance = go.AddComponent<ScoreManager>();
            return _instance;
        }
    }

    public event Action<int, int> OnScoreChanged;

    private int currentScore;
    private int highScore;
    private const string HighScoreKey = "HighScore";

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        currentScore = 0;
        // seed subscribers
        OnScoreChanged?.Invoke(currentScore, highScore);
    }

    void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }

    public void AddScore(int amount)
    {
        if (amount <= 0) return;
        currentScore += amount;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }
        OnScoreChanged?.Invoke(currentScore, highScore);
    }

    public int CurrentScore => currentScore;
    public int HighScore => highScore;
}