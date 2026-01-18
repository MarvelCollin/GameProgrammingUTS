using UnityEngine;

public class SkeletonAnimator
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] idleSprites;
    private Sprite[] attackSprites;
    private Sprite[] hurtSprites;
    private Sprite[] deathSprites;
    private Sprite[] walkSprites;

    private Sprite[] currentSprites;
    private int currentFrame;
    private float animationTimer;
    private float animationSpeed = 0.1f;
    private bool isLooping = true;
    private bool animationComplete;

    private string currentAnimation = "";

    public SkeletonAnimator(SpriteRenderer renderer)
    {
        spriteRenderer = renderer;
        LoadSprites();
    }

    private void LoadSprites()
    {
        idleSprites = SpriteFactory.GetSprites(ResourcePaths.Characters.Skeleton.Idle);
        attackSprites = SpriteFactory.GetSprites(ResourcePaths.Characters.Skeleton.Attack);
        hurtSprites = SpriteFactory.GetSprites(ResourcePaths.Characters.Skeleton.Hurt);
        deathSprites = SpriteFactory.GetSprites(ResourcePaths.Characters.Skeleton.Death);
        walkSprites = SpriteFactory.GetSprites(ResourcePaths.Characters.Skeleton.Walk);
    }

    public void Update()
    {
        if (currentSprites == null || currentSprites.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++;

            if (currentFrame >= currentSprites.Length)
            {
                if (isLooping)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = currentSprites.Length - 1;
                    animationComplete = true;
                }
            }

            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }

    public void PlayIdle()
    {
        if (currentAnimation == "idle") return;
        currentAnimation = "idle";
        SetAnimation(idleSprites, true);
    }

    public void PlayAttack()
    {
        if (currentAnimation == "attack") return;
        currentAnimation = "attack";
        SetAnimation(attackSprites, false);
    }

    public void PlayHurt()
    {
        currentAnimation = "hurt";
        SetAnimation(hurtSprites, false);
    }

    public void PlayDeath()
    {
        currentAnimation = "death";
        SetAnimation(deathSprites, false);
    }

    public void PlayWalk()
    {
        if (currentAnimation == "walk") return;
        currentAnimation = "walk";
        SetAnimation(walkSprites, true);
    }

    private void SetAnimation(Sprite[] sprites, bool loop)
    {
        currentSprites = sprites;
        currentFrame = 0;
        animationTimer = 0f;
        isLooping = loop;
        animationComplete = false;

        if (currentSprites != null && currentSprites.Length > 0)
        {
            spriteRenderer.sprite = currentSprites[0];
        }
    }

    public bool IsAnimationComplete()
    {
        return animationComplete;
    }
}
