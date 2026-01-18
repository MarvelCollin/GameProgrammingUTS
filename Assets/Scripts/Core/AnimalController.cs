using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class AnimalController : MonoBehaviour
{
    [Header("Animal Settings")]
    [SerializeField] private AnimalType animalType = AnimalType.Cow;
    
    private SpriteRenderer spriteRenderer;
    private string animalSound = "MOO";
    private CircleCollider2D triggerCollider;
    private WorldSpaceUI worldSpaceUI;
    private AnimalAnimator animalAnimator;

    private float constantSoundInterval = 5f;
    private float constantSoundTimer;

    private float roamRadius = 1.5f;
    private float roamSpeed = 0.5f;
    private Vector3 spawnPosition;
    private Vector3 targetPosition;
    private float roamTimer;
    private float roamInterval = 3f;

    private float emoteDuration = 2f;
    private float emoteTimer;
    private bool showingEmote;
    private string currentEmote = "";

    private bool isBeingAttacked;
    private float attackCooldown = 0.5f;
    private float attackTimer;

    private bool isDead;
    private float respawnTime = 10f;
    private float respawnTimer;
    
    private void Awake()
    {
        if (gameObject.CompareTag(GameConstants.Tags.Player))
        {
            Destroy(this);
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<CircleCollider2D>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        animalAnimator = GetComponent<AnimalAnimator>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, 0.5f, 0));
        }
        
        if (animalAnimator == null)
        {
            animalAnimator = gameObject.AddComponent<AnimalAnimator>();
        }
        
        triggerCollider.isTrigger = true;
        triggerCollider.radius = GameConstants.Physics.DefaultTriggerRadius / GameConstants.Physics.DefaultScale;
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        SetAnimalSound();
        LoadSprite();

        spawnPosition = transform.position;
        targetPosition = spawnPosition;
        constantSoundTimer = Random.Range(0f, constantSoundInterval);
        roamTimer = Random.Range(0f, roamInterval);
    }
    
    private void LoadSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            Sprite[] sprites = SpriteFactory.GetAnimalSprites(animalType);
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0];
            }
        }
    }

    private void Update()
    {
        if (isDead)
        {
            HandleRespawn();
            return;
        }

        HandleConstantSound();
        HandleEmoteTimer();
        HandleAttackCooldown();
        CheckForNearbyAttack();
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
        transform.position = spawnPosition;
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
        if (isBeingAttacked || isDead) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag(GameConstants.Tags.Player);
        if (playerObj == null) return;

        PlayerController player = playerObj.GetComponent<PlayerController>();
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, playerObj.transform.position);
        float attackRange = 1.0f;

        if (distance <= attackRange && player.IsAttacking())
        {
            ShowEmote("</3");
            PlayAnimalSound();
            Die();
        }
    }

    private void HandleConstantSound()
    {
        constantSoundTimer -= Time.deltaTime;
        if (constantSoundTimer <= 0)
        {
            constantSoundTimer = constantSoundInterval + Random.Range(-1f, 1f);
            PlayAnimalSound();
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
                currentEmote = "";
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

    private void PlayAnimalSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAnimalSound(animalType.ToString());
        }
    }

    private void ShowEmote(string emote)
    {
        currentEmote = emote;
        showingEmote = true;
        emoteTimer = emoteDuration;
        
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, emote);
    }
    
    private void SetAnimalSound()
    {
        switch (animalType)
        {
            case AnimalType.Cow:
                animalSound = "MOO";
                break;
            case AnimalType.Chicken:
                animalSound = "CLUCK CLUCK";
                break;
            case AnimalType.Pig:
                animalSound = "OINK";
                break;
            case AnimalType.Sheep:
                animalSound = "BAA";
                break;
            case AnimalType.Duck:
                animalSound = "QUACK";
                break;
            case AnimalType.Bird:
                animalSound = "CHIRP";
                break;
        }
    }
    
    public string GetAnimalSpritePath()
    {
        switch (animalType)
        {
            case AnimalType.Cow:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_cow_strip4";
            case AnimalType.Chicken:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_chicken_01_strip4";
            case AnimalType.Pig:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_pig_01_strip4";
            case AnimalType.Sheep:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_sheep_01_strip4";
            case AnimalType.Duck:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_duck_01_strip4";
            case AnimalType.Bird:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_bird_01_strip4";
            default:
                return "";
        }
    }
    
    public AnimalType GetAnimalType()
    {
        return animalType;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float worldRadius = (triggerCollider != null ? triggerCollider.radius : GameConstants.Physics.DefaultTriggerRadius / GameConstants.Physics.DefaultScale) * transform.localScale.x;
        Gizmos.DrawWireSphere(transform.position, worldRadius);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.IsAttacking())
                {
                    ShowEmote("</3");
                    PlayAnimalSound();
                    Die();
                }
                else if (!showingEmote)
                {
                    ShowEmote("<3");
                    PlayAnimalSound();
                    
                    if (animalAnimator != null)
                    {
                        animalAnimator.PlayInteractionAnimation();
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.IsAttacking())
            {
                ShowEmote("</3");
                PlayAnimalSound();
                Die();
            }
        }
    }
}

public enum AnimalType
{
    Cow,
    Chicken,
    Pig,
    Sheep,
    Duck,
    Bird
}
