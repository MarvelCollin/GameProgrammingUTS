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
        triggerCollider.radius = 0.5f;
        
        transform.localScale = new Vector3(5f, 5f, 1f);
        
        LoadSprite();
    }
    
    private void LoadSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            string spritePath = GetCropSpritePath();
            if (!string.IsNullOrEmpty(spritePath))
            {
                Sprite sprite = Resources.Load<Sprite>(spritePath);
                if (sprite != null)
                {
                    spriteRenderer.sprite = sprite;
                }
            }
        }
    }
    
    private string GetCropSpritePath()
    {
        switch (cropType)
        {
            case CropType.Carrot:
                return "Sunnyside_World_Assets/Elements/Crops/carrot_05";
            case CropType.Potato:
                return "Sunnyside_World_Assets/Elements/Crops/potato_05";
            case CropType.Wheat:
                return "Sunnyside_World_Assets/Elements/Crops/wheat_05";
            case CropType.Pumpkin:
                return "Sunnyside_World_Assets/Elements/Crops/pumpkin_05";
            case CropType.Cabbage:
                return "Sunnyside_World_Assets/Elements/Crops/cabbage_05";
            case CropType.Beetroot:
                return "Sunnyside_World_Assets/Elements/Crops/beetroot_05";
            default:
                return "";
        }
    }
    
    public string GetCropTypeName()
    {
        return cropType.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                isCollected = true;
                player.CollectCrop();
                
                string message = $"Crop harvested: {player.GetCropCount()}";
                
                if (worldSpaceUI != null)
                {
                    worldSpaceUI.ShowMessage(message);
                }
                
                GameUIManager.Instance?.ShowMessage(message);
                Debug.Log(message);
                
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
