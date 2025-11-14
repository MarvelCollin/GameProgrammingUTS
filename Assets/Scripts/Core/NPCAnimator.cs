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
            string spritePath = "";
            
            if (currentSpriteName.Contains("base_idle"))
            {
                spritePath = "Sunnyside_World_Assets/Characters/Human/IDLE/base_idle_strip9";
            }
            else if (currentSpriteName.Contains("spr_idle"))
            {
                spritePath = "Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9";
            }
            else if (currentSpriteName.Contains("skeleton_idle"))
            {
                spritePath = "Sunnyside_World_Assets/Characters/Skeleton/PNG/skeleton_idle_strip6";
            }
            
            if (!string.IsNullOrEmpty(spritePath))
            {
                idleSprites = Resources.LoadAll<Sprite>(spritePath);
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
}

public enum NPCDirection
{
    Up,
    Down,
    Left,
    Right
}
