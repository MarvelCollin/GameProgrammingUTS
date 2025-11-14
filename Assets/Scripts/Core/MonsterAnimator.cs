using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class MonsterAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void PlayAttackAnimation()
    {
        if (!isAttacking && animator != null)
        {
            StartCoroutine(AttackSequence());
        }
    }
    
    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        Sprite[] attackSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Goblin.Attack);
        
        if (attackSprites != null && attackSprites.Length > 0)
        {
            IAnimationStrategy attackStrategy = new OnceAnimationStrategy(attackSprites, GameConstants.Animation.FastFrameDelay);
            yield return StartCoroutine(attackStrategy.Play(spriteRenderer));
            
            Sprite[] idleSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Goblin.Idle);
            if (idleSprites != null && idleSprites.Length > 0)
            {
                spriteRenderer.sprite = idleSprites[0];
            }
        }
        
        isAttacking = false;
    }
    
    public void PlayIdleAnimation()
    {
        if (!isAttacking)
        {
            StartCoroutine(IdleLoop());
        }
    }
    
    private IEnumerator IdleLoop()
    {
        Sprite[] idleSprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Goblin.Idle);
        
        if (idleSprites != null && idleSprites.Length > 0)
        {
            while (!isAttacking)
            {
                IAnimationStrategy idleStrategy = new LoopAnimationStrategy(idleSprites, GameConstants.Animation.DefaultFrameDelay);
                yield return StartCoroutine(idleStrategy.Play(spriteRenderer));
            }
        }
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
