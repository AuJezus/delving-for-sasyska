using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float aggroRadius = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int contactDamage = 1;

    private int currentHealth;
    private enum State { Idle, Chasing }
    private State currentState = State.Idle;
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 moveDirection;

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
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        switch (currentState)
        {
            case State.Idle:
                if (dist <= aggroRadius)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
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
                break;
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
        if (col.collider.CompareTag("Player"))
        {
            var ph = col.collider.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(contactDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        // TODO: play hit VFX/SFX here

        if (currentHealth == 0) Die();
    }

    private void Die()
    {
        // TODO: play death animation, drop loot, etc.
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}