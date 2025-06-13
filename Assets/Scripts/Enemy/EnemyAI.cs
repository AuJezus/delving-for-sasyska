using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float aggroRadius = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int contactDamage = 1;

    [Header("Contact Damage")]
    [Tooltip("Seconds between repeated contact damage")]
    [SerializeField] private float contactDamageCooldown = 1f;

    [Header("Drop")]
    [SerializeField] private GameObject sasyskaPrefab;

    private int currentHealth;
    private enum State { Idle, Chasing }
    private State currentState = State.Idle;
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 moveDirection;

    // contact‐damage tracking
    private bool touchingPlayer;
    private PlayerHealth playerHealthRef;
    private float nextContactDamageTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("EnemyAI: No GameObject tagged 'Player'.");
    }

    void Update()
    {
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (currentState == State.Idle && dist <= aggroRadius)
                currentState = State.Chasing;
            else if (currentState == State.Chasing)
            {
                if (dist > aggroRadius)
                {
                    currentState = State.Idle;
                    moveDirection = Vector2.zero;
                }
                else
                {
                    moveDirection = (player.position - transform.position)
                                    .normalized;
                }
            }
        }

        // Handle repeated contact damage
        if (touchingPlayer && playerHealthRef != null &&
            Time.time >= nextContactDamageTime)
        {
            playerHealthRef.TakeDamage(contactDamage);
            nextContactDamageTime = Time.time + contactDamageCooldown;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Chasing)
            rb.MovePosition(rb.position + moveDirection
                            * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        touchingPlayer = true;
        playerHealthRef = col.collider.GetComponent<PlayerHealth>();

        if (playerHealthRef != null)
        {
            playerHealthRef.TakeDamage(contactDamage);
            nextContactDamageTime = Time.time + contactDamageCooldown;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        touchingPlayer = false;
        playerHealthRef = null;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (currentHealth == 0) Die();
    }

    private void Die()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1)
        {
            DungeonPortal portal = FindFirstObjectByType<DungeonPortal>();

            portal.ShowPortal();
            portal.transform.position = transform.position;
        }
        else if (sasyskaPrefab != null)
        {
            Instantiate(sasyskaPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}