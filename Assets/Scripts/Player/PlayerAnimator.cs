using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer toolSpriteRenderer;
    private GameObject toolObject;
    
    private Vector2 lastMoveDirection;
    private bool isMoving;
    private bool isHurt = false;
    private bool isAttacking = false;
    private bool isDigging = false;
    private Coroutine hurtCoroutine;
    private Coroutine attackCoroutine;
    private Coroutine digCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2.down;
        
        spriteRenderer.sortingOrder = GameConstants.Physics.PlayerSortingOrder;
        
        CreateToolSpriteRenderer();
    }
    
    private void CreateToolSpriteRenderer()
    {
        toolObject = new GameObject("ToolSprite");
        toolObject.transform.SetParent(transform);
        toolObject.transform.localPosition = Vector3.zero;
        toolObject.transform.localScale = Vector3.one;
        
        toolSpriteRenderer = toolObject.AddComponent<SpriteRenderer>();
        toolSpriteRenderer.sortingOrder = GameConstants.Physics.PlayerSortingOrder + 1;
        toolSpriteRenderer.sprite = null;
        toolObject.SetActive(false);
    }
    
    public void UpdateMovement(Vector2 moveInput)
    {
        if (isHurt || isAttacking || isDigging) return;
        
        bool wasMoving = isMoving;
        isMoving = moveInput.magnitude > 0.1f;
        
        if (isMoving)
        {
            lastMoveDirection = moveInput.normalized;
            
            if (moveInput.x < -0.1f)
            {
                spriteRenderer.flipX = true;
                toolSpriteRenderer.flipX = true;
            }
            else if (moveInput.x > 0.1f)
            {
                spriteRenderer.flipX = false;
                toolSpriteRenderer.flipX = false;
            }
        }
        
        UpdateAnimationState();
    }
    
    private void UpdateAnimationState()
    {
        if (animator == null || isHurt || isAttacking || isDigging) return;
        
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
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackAnimation());
    }
    
    private IEnumerator AttackAnimation()
    {
        isAttacking = true;
        
        if (animator != null)
        {
            animator.enabled = false;
        }
        
        Sprite[] bodySprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.Attack);
        Sprite[] toolSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.AttackTool);
        
        if (bodySprites != null && bodySprites.Length > 0)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAttackSound();
            }
            toolObject.SetActive(true);
            yield return StartCoroutine(PlayLayeredAnimation(bodySprites, toolSprites, 0.05f));
            toolObject.SetActive(false);
        }
        
        if (animator != null)
        {
            animator.enabled = true;
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
        toolObject.SetActive(false);
        
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
    
    public void PlayDig()
    {
        if (digCoroutine != null)
        {
            StopCoroutine(digCoroutine);
        }
        digCoroutine = StartCoroutine(DigAnimation());
    }
    
    private IEnumerator DigAnimation()
    {
        isDigging = true;
        
        if (animator != null)
        {
            animator.enabled = false;
        }
        
        Sprite[] bodySprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.Dig);
        Sprite[] toolSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Human.DigTool);
        
        if (bodySprites != null && bodySprites.Length > 0)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDigSound();
            }
            toolObject.SetActive(true);
            yield return StartCoroutine(PlayLayeredAnimation(bodySprites, toolSprites, 0.05f));
            toolObject.SetActive(false);
        }
        
        if (animator != null)
        {
            animator.enabled = true;
        }
        
        isDigging = false;
    }
    
    public void StopDig()
    {
        if (digCoroutine != null)
        {
            StopCoroutine(digCoroutine);
            digCoroutine = null;
        }
        isDigging = false;
        toolObject.SetActive(false);
        
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
    
    private IEnumerator PlayLayeredAnimation(Sprite[] bodySprites, Sprite[] toolSprites, float frameDelay)
    {
        int frameCount = bodySprites.Length;
        
        for (int i = 0; i < frameCount; i++)
        {
            spriteRenderer.sprite = bodySprites[i];
            
            if (toolSprites != null && i < toolSprites.Length)
            {
                toolSpriteRenderer.sprite = toolSprites[i];
            }
            
            yield return new WaitForSeconds(frameDelay);
        }
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
    
    public bool IsDigging()
    {
        return isDigging;
    }
}
