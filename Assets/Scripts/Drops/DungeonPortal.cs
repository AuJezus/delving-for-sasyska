using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DungeonPortal : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private GameManager gameManager;

    private Collider2D portalCollider;
    private SpriteRenderer portalRenderer;

    void Awake()
    {
        portalCollider = GetComponent<Collider2D>();
        portalRenderer = GetComponent<SpriteRenderer>();

        HidePortal();

        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        ScoreManager.Instance.AddScore(scoreValue);
        gameManager.GenerateLevel();
        HidePortal();
    }

    public void HidePortal()
    {
        portalRenderer.enabled = false;
        portalCollider.enabled = false;
    }

    public void ShowPortal()
    {
        portalRenderer.enabled = true;
        portalCollider.enabled = true;
    }
}