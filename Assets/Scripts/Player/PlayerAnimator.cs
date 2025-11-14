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
        
        spriteRenderer.sortingOrder = 10;
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
        
        Sprite[] hurtSprites = Resources.LoadAll<Sprite>("Sunnyside_World_Assets/Characters/Human/HURT/base_hurt_strip8");
        
        if (hurtSprites != null && hurtSprites.Length > 0)
        {
            float frameDelay = 0.0625f;
            
            foreach (Sprite sprite in hurtSprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(frameDelay);
            }
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
