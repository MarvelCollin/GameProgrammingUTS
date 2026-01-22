using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class CropController : MonoBehaviour
{
    [Header("Crop Settings")]
    [SerializeField] private CropType cropType = CropType.Carrot;
    
    private SpriteRenderer spriteRenderer;
    private WorldSpaceUI worldSpaceUI;
    private CircleCollider2D triggerCollider;
    private bool isCollected = false;
    private bool isPlanted = true;
    private CropAnimator cropAnimator;

    private float respawnTime = 10f;
    private float respawnTimer;
    private Vector3 spawnPosition;
    private Color normalColor;
    private Color plantedColor = new Color(0.6f, 0.6f, 0.6f, 0.8f);
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<CircleCollider2D>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        cropAnimator = GetComponent<CropAnimator>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, 0.5f, 0));
        }
        
        if (cropAnimator == null)
        {
            cropAnimator = gameObject.AddComponent<CropAnimator>();
        }
        
        triggerCollider.isTrigger = true;
        triggerCollider.radius = GameConstants.Physics.DefaultTriggerRadius / GameConstants.Physics.DefaultScale;
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        LoadSprite();

        spawnPosition = transform.position;
        normalColor = spriteRenderer.color;
        SetPlantedVisual();
    }
    
    private void LoadSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            Sprite sprite = SpriteFactory.GetCropSprite(cropType);
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
            }
        }
    }

    private void Update()
    {
        if (isCollected)
        {
            HandleRespawn();
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
        transform.position = spawnPosition;
        isCollected = false;
        isPlanted = true;
        triggerCollider.enabled = true;
        spriteRenderer.enabled = true;
        SetPlantedVisual();
    }

    private void SetPlantedVisual()
    {
        if (isPlanted)
        {
            spriteRenderer.color = plantedColor;
        }
        else
        {
            spriteRenderer.color = normalColor;
        }
    }
    
    private string GetCropSpritePath()
    {
        return ResourcePaths.Crops.GetFullyGrownCrop(cropType.ToString());
    }
    
    public string GetCropTypeName()
    {
        return cropType.ToString();
    }
    
    public CropType GetCropType()
    {
        return cropType;
    }

    public void SetCropType(CropType type)
    {
        cropType = type;
        LoadSprite();
    }

    public void DigUp()
    {
        if (!isPlanted || isCollected) return;

        isPlanted = false;
        spriteRenderer.color = normalColor;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("dig");
        }
        
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, "Ready to harvest!");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.IsDigging() && isPlanted)
                {
                    DigUp();
                    return;
                }

                if (!isPlanted)
                {
                    CollectCrop(player);
                }
                else
                {
                    MessageBroadcaster.Instance.SendMessageToObject(gameObject, "Use dig to harvest!");
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.IsDigging() && isPlanted)
                {
                    DigUp();
                }
                else if (!isPlanted && !player.IsDigging())
                {
                    CollectCrop(player);
                }
            }
        }
    }

    private void CollectCrop(PlayerController player)
    {
        isCollected = true;
        player.CollectCrop();
        
        if (CropDataManager.Instance != null)
        {
            CropDataManager.Instance.AddCrop(cropType);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHarvestSound();
        }
        
        string message = $"{cropType} harvested!";
        
        MessageBroadcaster.Instance.SendMessageToObject(gameObject, message);
        
        if (cropAnimator != null)
        {
            cropAnimator.PlayHarvestAnimation();
        }

        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;
        respawnTimer = respawnTime;
    }
}

public enum CropType
{
    Carrot,
    Potato,
    Wheat,
    Pumpkin,
    Cabbage,
    Beetroot
}
