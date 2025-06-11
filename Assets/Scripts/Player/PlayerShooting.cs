using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Rigidbody2D rb;

    [Header("Tuning")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;

    private InputSystem inputActions;
    private float nextFireTime = 0f;

    void Awake()
    {
        inputActions = new InputSystem();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        bool wantsToShoot =
          inputActions.Player.Shoot.ReadValue<float>() > 0.1f;

        if (wantsToShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector2 screenPos = inputActions.Player
          .Aim.ReadValue<Vector2>();

        Vector3 worldPos = Camera.main
          .ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        Vector2 origin = rb.position;
        Vector2 dir = (worldPos - (Vector3)origin).normalized;

        var b = Instantiate(
          bulletPrefab,
          origin,
          Quaternion.identity
        );

        b.GetComponent<Rigidbody2D>()
         .linearVelocity = dir * bulletSpeed;
    }

    void OnDestroy()
    {
        inputActions?.Dispose();
    }
}