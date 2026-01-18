using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPCAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private bool playIdleAnimation = true;
    [SerializeField] private NPCDirection facingDirection = NPCDirection.Down;
    [SerializeField] private float animationSpeed = 0.1f;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite[] idleSprites;
    private int currentSpriteIndex = 0;
    private float animationTimer = 0f;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        SetFacingDirection(facingDirection);
        LoadIdleSprites();
    }
    
    private void Update()
    {
        if (playIdleAnimation && idleSprites != null && idleSprites.Length > 1)
        {
            animationTimer += Time.deltaTime;
            
            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % idleSprites.Length;
                spriteRenderer.sprite = idleSprites[currentSpriteIndex];
            }
        }
    }
    
    private void LoadIdleSprites()
    {
        if (spriteRenderer.sprite != null)
        {
            string currentSpriteName = spriteRenderer.sprite.name;
            string spritePath = string.Empty;
            
            if (currentSpriteName.Contains("base_idle"))
            {
                spritePath = ResourcePaths.Characters.Human.Idle;
            }
            else if (currentSpriteName.Contains("spr_idle"))
            {
                spritePath = ResourcePaths.Characters.Goblin.Idle;
            }
            else if (currentSpriteName.Contains("skeleton_idle"))
            {
                spritePath = ResourcePaths.Characters.Skeleton.Idle;
            }
            
            if (!string.IsNullOrEmpty(spritePath))
            {
                idleSprites = SpriteFactory.LoadSpriteStrip(spritePath);
            }
        }
    }
    
    public void SetFacingDirection(NPCDirection direction)
    {
        facingDirection = direction;
        
        if (animator != null)
        {
            switch (direction)
            {
                case NPCDirection.Left:
                    spriteRenderer.flipX = true;
                    break;
                case NPCDirection.Right:
                    spriteRenderer.flipX = false;
                    break;
            }
        }
    }
    
    public void PlayAnimation(string animationName)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.Play(animationName);
        }
    }

    public void PlayInteractionAnimation()
    {
        currentSpriteIndex = 0;
        animationTimer = 0f;
    }
}

public enum NPCDirection
{
    Up,
    Down,
    Left,
    Right
}
