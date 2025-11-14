using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPCAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private bool playIdleAnimation = true;
    [SerializeField] private NPCDirection facingDirection = NPCDirection.Down;
    [SerializeField] private float animationSpeed = 0.1f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite[] idleSprites;
    private int currentSpriteIndex = 0;
    private float animationTimer = 0f;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (showDebugLogs)
        {
            Debug.Log("========== NPC ANIMATOR AWAKE ==========");
            Debug.Log($"[NPCAnimator] Initializing NPC Animator");
            Debug.Log($"[NPCAnimator] Play Idle Animation: {playIdleAnimation}");
            Debug.Log($"[NPCAnimator] Facing Direction: {facingDirection}");
            
            if (animator == null)
            {
                Debug.LogError("[NPCAnimator] Animator component not found!");
            }
            else
            {
                Debug.Log($"[NPCAnimator] Animator found. Controller: {animator.runtimeAnimatorController?.name ?? "None"}");
                if (animator.runtimeAnimatorController == null)
                {
                    Debug.LogWarning("[NPCAnimator] No Animator Controller assigned!");
                }
            }
            
            if (spriteRenderer == null)
            {
                Debug.LogError("[NPCAnimator] SpriteRenderer component not found!");
            }
            else
            {
                Debug.Log($"[NPCAnimator] SpriteRenderer found");
                Debug.Log($"[NPCAnimator] Current sprite: {spriteRenderer.sprite?.name ?? "None"}");
                if (spriteRenderer.sprite == null)
                {
                    Debug.LogWarning("[NPCAnimator] No sprite assigned to SpriteRenderer!");
                }
            }
            
            Debug.Log("========================================");
        }
    }
    
    private void Start()
    {
        SetFacingDirection(facingDirection);
        LoadIdleSprites();
        
        if (playIdleAnimation)
        {
            if (idleSprites != null && idleSprites.Length > 0)
            {
                if (showDebugLogs)
                {
                    Debug.Log($"[NPCAnimator] Starting sprite animation with {idleSprites.Length} frames");
                    Debug.Log($"[NPCAnimator] Animation speed: {animationSpeed}s per frame");
                }
            }
            else
            {
                if (showDebugLogs)
                {
                    Debug.LogWarning($"[NPCAnimator] No idle sprites loaded - animation disabled");
                }
            }
        }
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
                
                if (showDebugLogs)
                {
                    Debug.Log($"[NPCAnimator] Animation frame: {currentSpriteIndex}/{idleSprites.Length - 1} - {idleSprites[currentSpriteIndex].name}");
                }
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
                
                if (showDebugLogs)
                {
                    if (idleSprites != null && idleSprites.Length > 0)
                    {
                        Debug.Log($"[NPCAnimator] Loaded {idleSprites.Length} idle sprites from: {spritePath}");
                        foreach (var sprite in idleSprites)
                        {
                            Debug.Log($"[NPCAnimator]   - {sprite.name}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"[NPCAnimator] Failed to load sprites from: {spritePath}");
                    }
                }
            }
            else if (showDebugLogs)
            {
                Debug.LogWarning($"[NPCAnimator] Could not determine sprite path from: {currentSpriteName}");
            }
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning($"[NPCAnimator] No sprite assigned to SpriteRenderer");
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
        
        if (showDebugLogs)
        {
            Debug.Log($"[NPCAnimator] Facing direction set to: {direction}");
        }
    }
    
    public void PlayAnimation(string animationName)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.Play(animationName);
            
            if (showDebugLogs)
            {
                Debug.Log($"[NPCAnimator] Playing animation: {animationName}");
            }
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning($"[NPCAnimator] Cannot play animation '{animationName}' - Animator or Controller is missing");
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
