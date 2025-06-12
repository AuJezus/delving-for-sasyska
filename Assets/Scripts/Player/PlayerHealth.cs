using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int maxHealth = 5;

    private int currentHealth;

    [Header("Events")]
    public UnityEvent onHurt;
    public UnityEvent onHeal;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        onHurt?.Invoke();

        if (currentHealth == 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth == maxHealth) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHeal?.Invoke();
    }

    private void Die()
    {
        onDeath?.Invoke();
        Debug.Log("Player has died.");
    }


    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}