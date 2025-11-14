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
        
        Sprite[] attackSprites = Resources.LoadAll<Sprite>("Sunnyside_World_Assets/Characters/Goblin/PNG/spr_attack_strip10");
        
        if (attackSprites != null && attackSprites.Length > 0)
        {
            float frameDelay = 0.05f;
            
            foreach (Sprite sprite in attackSprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(frameDelay);
            }
            
            Sprite[] idleSprites = Resources.LoadAll<Sprite>("Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9");
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
        Sprite[] idleSprites = Resources.LoadAll<Sprite>("Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9");
        
        if (idleSprites != null && idleSprites.Length > 0)
        {
            float frameDelay = 0.1f;
            int currentFrame = 0;
            
            while (!isAttacking)
            {
                spriteRenderer.sprite = idleSprites[currentFrame];
                currentFrame = (currentFrame + 1) % idleSprites.Length;
                yield return new WaitForSeconds(frameDelay);
            }
        }
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
