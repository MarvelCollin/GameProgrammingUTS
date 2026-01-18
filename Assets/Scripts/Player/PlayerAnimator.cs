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
    private bool isAttacking = false;
    private Coroutine hurtCoroutine;
    private Coroutine attackCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2.down;
        
        spriteRenderer.sortingOrder = GameConstants.Physics.PlayerSortingOrder;
    }
    
    public void UpdateMovement(Vector2 moveInput)
    {
        if (isHurt || isAttacking) return;
        
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
        if (animator == null || isHurt || isAttacking) return;
        
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
    
    public void PlayAttack()
    {
        Debug.Log("PlayerAnimator.PlayAttack() called");
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackAnimation());
    }
    
    private IEnumerator AttackAnimation()
    {
        isAttacking = true;
        Debug.Log("AttackAnimation coroutine started");
        
        if (animator != null)
        {
            animator.enabled = false;
            Debug.Log("Animator disabled for attack");
        }
        
        Sprite[] attackSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.Attack);
        Debug.Log($"Attack sprites loaded: {(attackSprites != null ? attackSprites.Length.ToString() : "NULL")} sprites");
        
        if (attackSprites != null && attackSprites.Length > 0)
        {
            IAnimationStrategy attackStrategy = new OnceAnimationStrategy(attackSprites, 0.05f);
            yield return StartCoroutine(attackStrategy.Play(spriteRenderer));
            Debug.Log("Attack animation completed");
        }
        else
        {
            Debug.LogError("Failed to load attack sprites!");
        }
        
        if (animator != null)
        {
            animator.enabled = true;
            Debug.Log("Animator re-enabled after attack");
        }
        
        isAttacking = false;
    }
    
    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        isAttacking = false;
        
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
}

