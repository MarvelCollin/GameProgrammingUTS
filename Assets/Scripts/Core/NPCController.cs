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

    private bool isDead;
    private float respawnTime = 10f;
    private float respawnTimer;
    private Vector3 spawnPositionNPC;

    private Rigidbody2D rb;

    public void SetNPCType(NPCType type)
    {
        npcType = type;
        if (npcType == NPCType.Goblin)
        {
            isFriendlyNPC = true;
        }
        LoadSpriteBasedOnType();
    }
    
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
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, 0.5f, 0));
        }

        if (npcAnimator == null)
        {
            npcAnimator = gameObject.AddComponent<NPCAnimator>();
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        if (autoLoadSprite)
        {
            LoadSpriteBasedOnType();
        }

        spawnPosition = transform.position;
        spawnPositionNPC = transform.position;
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
        if (isDead)
        {
            HandleRespawn();
            return;
        }

        if (isFriendlyNPC)
        {
            HandleEmoteTimer();
            HandleAttackCooldown();
            CheckForNearbyAttack();
            HandleRoaming();
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

    private void HandleRoaming()
    {
        roamTimer -= Time.deltaTime;
        
        if (roamTimer <= 0)
        {
            roamTimer = roamInterval + Random.Range(-1f, 1f);
            Vector2 randomOffset = Random.insideUnitCircle * roamRadius;
            targetPosition = spawnPosition + new Vector3(randomOffset.x, randomOffset.y, 0);
        }

        Vector3 direction = (targetPosition - transform.position);
        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(direction.x, direction.y) * roamSpeed;
            }
            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void HandleRespawn()
    {
        respawnTimer -= Time.deltaTime;
        if (respawnTimer <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPositionNPC;
        isDead = false;
        triggerCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void Die()
    {
        isDead = true;
        respawnTimer = respawnTime;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    private void CheckForNearbyAttack()
    {
        if (isBeingAttacked || isDead || player == null) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float attackRange = 1.0f;

        if (distance <= attackRange && playerController.IsAttacking())
        {
            ShowEmote("</3");
            Die();
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
        if (!isFriendlyNPC || isDead) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (playerController.IsAttacking())
                {
                    ShowEmote("</3");
                    Die();
                }
                else if (!showingEmote)
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
        if (!isFriendlyNPC || isDead) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsAttacking())
            {
                ShowEmote("</3");
                Die();
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
