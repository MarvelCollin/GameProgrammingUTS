using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 lastMoveDirection;
    private bool isMoving;
    
    private void Awake()
    {
        Debug.Log("[PlayerAnimator] Awake - Initializing animator");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2.down;
        
        if (animator == null)
        {
            Debug.LogError("[PlayerAnimator] Animator component not found!");
        }
        else
        {
            Debug.Log($"[PlayerAnimator] Animator found. Controller: {animator.runtimeAnimatorController?.name}");
        }
        
        if (spriteRenderer == null)
        {
            Debug.LogError("[PlayerAnimator] SpriteRenderer component not found!");
        }
        else
        {
            Debug.Log($"[PlayerAnimator] SpriteRenderer found. Current sprite: {spriteRenderer.sprite?.name}");
        }
    }
    
    public void UpdateMovement(Vector2 moveInput)
    {
        bool wasMoving = isMoving;
        isMoving = moveInput.magnitude > 0.1f;
        
        if (isMoving)
        {
            lastMoveDirection = moveInput.normalized;
            
            if (moveInput.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveInput.x > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
        }
        
        if (wasMoving != isMoving)
        {
            Debug.Log($"[PlayerAnimator] Movement state changed: {(isMoving ? "Moving" : "Idle")}");
        }
        
        UpdateAnimationState();
    }
    
    private void UpdateAnimationState()
    {
        if (animator == null) return;
        
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
    }
}
