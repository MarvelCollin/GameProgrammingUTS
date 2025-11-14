using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private NPCType npcType = NPCType.Human;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private bool autoLoadSprite = true;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform player;
    
    private const string HUMAN_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Human/IDLE/base_idle_strip9";
    private const string GOBLIN_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9";
    private const string SKELETON_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Skeleton/PNG/skeleton_idle_strip6";
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        transform.localScale = new Vector3(5f, 5f, 1f);
        
        if (autoLoadSprite)
        {
            LoadSpriteBasedOnType();
        }
        
        if (showDebugLogs)
        {
            Debug.Log($"========== NPC AWAKE: {npcName} ==========");
            Debug.Log($"[NPCController] NPC Name: {npcName}");
            Debug.Log($"[NPCController] NPC Type: {npcType}");
            Debug.Log($"[NPCController] Can Interact: {canInteract}");
            Debug.Log($"[NPCController] Interaction Range: {interactionRange}");
            Debug.Log($"[NPCController] Auto Load Sprite: {autoLoadSprite}");
            
            if (spriteRenderer == null)
            {
                Debug.LogError($"[NPCController] {npcName} - SpriteRenderer not found!");
            }
            else
            {
                Debug.Log($"[NPCController] {npcName} - SpriteRenderer found. Sprite: {spriteRenderer.sprite?.name ?? "None"}");
            }
            
            if (animator == null)
            {
                Debug.LogError($"[NPCController] {npcName} - Animator not found!");
            }
            else
            {
                Debug.Log($"[NPCController] {npcName} - Animator found. Controller: {animator.runtimeAnimatorController?.name ?? "None"}");
            }
            
            Debug.Log($"==========================================");
        }
    }
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            if (showDebugLogs)
            {
                Debug.Log($"[NPCController] {npcName} - Player found at: {player.position}");
            }
        }
        else
        {
            if (showDebugLogs)
            {
                Debug.LogWarning($"[NPCController] {npcName} - Player not found! Interaction disabled.");
            }
        }
        
        if (showDebugLogs)
        {
            Debug.Log($"[NPCController] {npcName} - Positioned at: {transform.position}");
            Debug.Log($"[NPCController] {npcName} - Ready for interactions: {canInteract && player != null}");
        }
    }
    
    private void Update()
    {
        if (canInteract && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            if (showDebugLogs && distance <= interactionRange)
            {
                Debug.Log($"[NPCController] {npcName} - Player in range! Distance: {distance:F2} (Press E to interact)");
            }
            
            if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }
    
    private void Interact()
    {
        Debug.Log($"========================================");
        Debug.Log($"[NPCController] INTERACTION: {npcName}");
        Debug.Log($"[NPCController] Type: {npcType}");
        Debug.Log($"[NPCController] Position: {transform.position}");
        Debug.Log($"========================================");
    }
    
    private void LoadSpriteBasedOnType()
    {
        string spritePath = "";
        
        switch (npcType)
        {
            case NPCType.Human:
                spritePath = HUMAN_SPRITE_PATH;
                break;
            case NPCType.Goblin:
                spritePath = GOBLIN_SPRITE_PATH;
                break;
            case NPCType.Skeleton:
                spritePath = SKELETON_SPRITE_PATH;
                break;
        }
        
        if (!string.IsNullOrEmpty(spritePath))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
            
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0];
                
                if (showDebugLogs)
                {
                    Debug.Log($"[NPCController] {npcName} - Loaded sprite: {sprites[0].name} from {spritePath}");
                    Debug.Log($"[NPCController] {npcName} - Total sprites available: {sprites.Length}");
                }
            }
            else
            {
                Debug.LogError($"[NPCController] {npcName} - Failed to load sprites from: {spritePath}");
                Debug.LogError($"[NPCController] Make sure sprites are in Resources folder or use addressables");
            }
        }
    }
    
    public void SetNPCName(string name)
    {
        npcName = name;
    }
    
    public string GetNPCName()
    {
        return npcName;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (canInteract)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}

public enum NPCType
{
    Human,
    Goblin,
    Skeleton
}
