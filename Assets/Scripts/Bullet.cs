using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Time in seconds before auto‐destruct")]
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}