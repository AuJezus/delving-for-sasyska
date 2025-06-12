using UnityEngine;

public class DungeonPortal : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    [SerializeField] private GameManager gameManager;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        ScoreManager.Instance.AddScore(scoreValue);

        gameManager.GenerateLevel();
        gameObject.SetActive(false);
    }
}