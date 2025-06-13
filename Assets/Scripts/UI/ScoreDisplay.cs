using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    void OnEnable()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateUI;

        UpdateUI(
          ScoreManager.Instance.CurrentScore,
          ScoreManager.Instance.HighScore
        );
    }

    void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int high)
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = currentScoreText.text.Replace(":score", current.ToString());
        }

        highScoreText.text = highScoreText.text.Replace(":score", high.ToString());
    }
}