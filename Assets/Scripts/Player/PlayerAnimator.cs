using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 lastMoveDirection;
    private bool isMoving;
    private bool isHurt = false;
    private Coroutine hurtCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2.down;
        
        spriteRenderer.sortingOrder = GameConstants.Physics.PlayerSortingOrder;
    }
    
    public void UpdateMovement(Vector2 moveInput)
    {
        if (isHurt) return;
        
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
        if (animator == null || isHurt) return;
        
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
    }
    
    public void PlayHurt()
    {
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine = StartCoroutine(HurtAnimation());
    }
    
    private IEnumerator HurtAnimation()
    {
        isHurt = true;
        
        Sprite[] hurtSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.Hurt);
        
        if (hurtSprites != null && hurtSprites.Length > 0)
        {
            IAnimationStrategy hurtStrategy = new OnceAnimationStrategy(hurtSprites, 0.0625f);
            yield return StartCoroutine(hurtStrategy.Play(spriteRenderer));
        }
        
        isHurt = false;
    }
    
    public void StopHurt()
    {
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
            hurtCoroutine = null;
        }
        isHurt = false;
    }
}
