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
    private CropAnimator cropAnimator;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<CircleCollider2D>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        cropAnimator = GetComponent<CropAnimator>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, -0.4f, 0));
        }
        
        if (cropAnimator == null)
        {
            cropAnimator = gameObject.AddComponent<CropAnimator>();
        }
        
        triggerCollider.isTrigger = true;
        triggerCollider.radius = GameConstants.Physics.DefaultTriggerRadius;
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
        LoadSprite();
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag(GameConstants.Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                isCollected = true;
                player.CollectCrop();
                
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayHarvestSound();
                }
                
                string message = $"Crop harvested: {player.GetCropCount()}";
                
                MessageBroadcaster.Instance.SendMessageToObject(gameObject, message);
                
                if (cropAnimator != null)
                {
                    cropAnimator.PlayHarvestAnimation();
                }
                
                Destroy(gameObject, 0.5f);
            }
        }
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
