using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private NPCType npcType = NPCType.Human;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private float interactionRange = 0.2f;
    [SerializeField] private bool autoLoadSprite = true;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform player;
    private WorldSpaceUI worldSpaceUI;
    
    private const string HUMAN_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Human/IDLE/base_idle_strip9";
    private const string GOBLIN_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9";
    private const string SKELETON_SPRITE_PATH = "Sunnyside_World_Assets/Characters/Skeleton/PNG/skeleton_idle_strip6";
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, -0.25f, 0));
        }
        
        transform.localScale = new Vector3(5f, 5f, 1f);
        
        if (autoLoadSprite)
        {
            LoadSpriteBasedOnType();
        }
    }
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    
    private void Update()
    {
        if (canInteract && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }
    
    private void Interact()
    {
        string interactionMessage = $"Talking with {npcName}";
        
        if (worldSpaceUI != null)
        {
            worldSpaceUI.ShowMessage(interactionMessage);
        }
        
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(interactionMessage);
        }
        
        Debug.Log(interactionMessage);
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
