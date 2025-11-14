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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2.down;
        
        spriteRenderer.sortingOrder = 10;
        
        Debug.Log($"[PlayerAnimator] Sorting order set to: {spriteRenderer.sortingOrder}");
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
