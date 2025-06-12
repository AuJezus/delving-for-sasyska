using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Scene select")]
    [SerializeField] public string startMenuScene;

    [System.Serializable]
    public class HealthChangedEvent : UnityEvent<int, int> { }

    public HealthChangedEvent onHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0) Die();
    }

    private void Die()
    {
        SceneManager.LoadScene(startMenuScene);
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}