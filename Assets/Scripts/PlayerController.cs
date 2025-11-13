using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerAnimator playerAnimator;
    
    private void Awake()
    {
        Debug.Log("[PlayerController] Awake - Initializing player controller");
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        
        if (rb == null)
        {
            Debug.LogError("[PlayerController] Rigidbody2D not found!");
        }
        else
        {
            Debug.Log("[PlayerController] Rigidbody2D found and assigned");
        }
        
        if (playerAnimator == null)
        {
            Debug.LogWarning("[PlayerController] PlayerAnimator not found!");
        }
        else
        {
            Debug.Log("[PlayerController] PlayerAnimator found and assigned");
        }
    }
    
    private void Start()
    {
        Debug.Log("[PlayerController] Start - Setting up physics");
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Debug.Log($"[PlayerController] Player position: {transform.position}");
        Debug.Log($"[PlayerController] Move speed: {moveSpeed}");
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f)
        {
            Debug.Log($"[PlayerController] Input received: {moveInput}");
        }
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        
        if (playerAnimator != null)
        {
            playerAnimator.UpdateMovement(moveInput);
        }
    }
}
