using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class NPCController : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private NPCType npcType = NPCType.Human;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private float interactionRange = 0.2f;
    [SerializeField] private bool autoLoadSprite = true;
    [SerializeField] private bool isFriendlyNPC = true;
    
    private SpriteRenderer spriteRenderer;
    private NPCAnimator npcAnimator;
    private Transform player;
    private WorldSpaceUI worldSpaceUI;
    private CircleCollider2D triggerCollider;

    private float roamRadius = 1.5f;
    private float roamSpeed = 0.5f;
    private Vector3 spawnPosition;
    private Vector3 targetPosition;
    private float roamTimer;
    private float roamInterval = 3f;

    private float emoteDuration = 2f;
    private float emoteTimer;
    private bool showingEmote;

    private bool isBeingAttacked;
    private float attackCooldown = 0.5f;
    private float attackTimer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        npcAnimator = GetComponent<NPCAnimator>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        triggerCollider = GetComponent<CircleCollider2D>();

        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        triggerCollider.isTrigger = true;
        triggerCollider.radius = GameConstants.Physics.DefaultTriggerRadius / GameConstants.Physics.DefaultScale;
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, 0.5f, 0));
        }

        if (npcAnimator == null)
        {
            npcAnimator = gameObject.AddComponent<NPCAnimator>();
        }
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        if (autoLoadSprite)
        {
            LoadSpriteBasedOnType();
        }

        spawnPosition = transform.position;
        targetPosition = spawnPosition;
        roamTimer = Random.Range(0f, roamInterval);
    }
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(GameConstants.Tags.Player);
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (npcType == NPCType.Goblin)
        {
            isFriendlyNPC = true;
        }
    }
    
    private void Update()
    {
        if (isFriendlyNPC)
        {
            HandleEmoteTimer();
            HandleAttackCooldown();
            CheckForNearbyAttack();
        }
        else if (canInteract && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    private void CheckForNearbyAttack()
    {
        if (isBeingAttacked || player == null) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float attackRange = 1.0f;

        if (distance <= attackRange && playerController.IsAttacking())
        {
            isBeingAttacked = true;
            attackTimer = attackCooldown;
            ShowEmote("</3");

            if (npcAnimator != null)
            {
                npcAnimator.PlayInteractionAnimation();
            }
        }
    }

    private void HandleEmoteTimer()
    {
        if (showingEmote)
        {
            emoteTimer -= Time.deltaTime;
            if (emoteTimer <= 0)
            {
                showingEmote = false;
            }
        }
    }

    private void HandleAttackCooldown()
    {
        if (isBeingAttacked)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isBeingAttacked = false;
            }
        }
    }

    private void ShowEmote(string emote)
    {
        showingEmote = true;
        emoteTimer = emoteDuration;
        
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, emote);
    }
    
    private void Interact()
    {
        string interactionMessage = $"Talking with {npcName}";
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, interactionMessage);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFriendlyNPC) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (playerController.IsAttacking() && !isBeingAttacked)
                {
                    isBeingAttacked = true;
                    attackTimer = attackCooldown;
                    ShowEmote("</3");

                    if (npcAnimator != null)
                    {
                        npcAnimator.PlayInteractionAnimation();
                    }
                }
                else if (!playerController.IsAttacking() && !showingEmote)
                {
                    ShowEmote("<3");

                    if (npcAnimator != null)
                    {
                        npcAnimator.PlayInteractionAnimation();
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isFriendlyNPC) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsAttacking() && !isBeingAttacked)
            {
                isBeingAttacked = true;
                attackTimer = attackCooldown;
                ShowEmote("</3");
            }
        }
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
