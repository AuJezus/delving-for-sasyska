using UnityEngine;

public sealed class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // try to find an existing one
            _instance = FindFirstObjectByType<ScoreManager>();
            if (_instance != null) return _instance;
            // create new GameObject if none exists
            var go = new GameObject(nameof(ScoreManager));
            _instance = go.AddComponent<ScoreManager>();
            return _instance;
        }
    }

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
    }

    public int CurrentScore => currentScore;
    public int HighScore => highScore;
}