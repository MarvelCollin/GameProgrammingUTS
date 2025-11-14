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
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        if (autoLoadSprite)
        {
            LoadSpriteBasedOnType();
        }
    }
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(GameConstants.Tags.Player);
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
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, interactionMessage);
        Debug.Log(interactionMessage);
    }
    
    private void LoadSpriteBasedOnType()
    {
        Sprite[] sprites = SpriteFactory.GetNPCSprites(npcType);
        
        if (sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
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
