using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private InputSystem inputActions;
    private Vector2 moveInput;
    private bool facingRight = false; 

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
        moveInput = inputActions.Player.Move.ReadValue<Vector2>().normalized; 

        animator.SetFloat("Speed", moveInput.magnitude);
        
        if ((moveInput.x > 0 && !facingRight ) || (moveInput.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            spriteRenderer.flipX = facingRight;
        }

    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    
    void OnDestroy()
    {
        inputActions?.Dispose();
    }
}
