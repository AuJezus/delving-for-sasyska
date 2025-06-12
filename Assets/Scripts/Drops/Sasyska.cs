using UnityEngine;

public class Sasyska : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        ScoreManager.Instance.AddScore(scoreValue);
        Destroy(gameObject);
    }
}