using UnityEngine;
using System.Collections;

public class CropAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CropController cropController;
    private bool isAnimating = false;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cropController = GetComponent<CropController>();
    }
    
    private void Start()
    {
        StartCoroutine(IdleAnimation());
    }
    
    private IEnumerator IdleAnimation()
    {
        if (cropController == null) yield break;
        
        Sprite[] growthSprites = GetGrowthSprites();
        
        if (growthSprites != null && growthSprites.Length > 0)
        {
            float frameDelay = 0.3f;
            int currentFrame = growthSprites.Length - 1;
            
            while (!isAnimating)
            {
                if (currentFrame >= 0 && currentFrame < growthSprites.Length)
                {
                    spriteRenderer.sprite = growthSprites[currentFrame];
                }
                
                currentFrame = (currentFrame == growthSprites.Length - 1) ? growthSprites.Length - 2 : growthSprites.Length - 1;
                
                yield return new WaitForSeconds(frameDelay);
            }
        }
    }
    
    public void PlayHarvestAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(HarvestSequence());
        }
    }
    
    private IEnumerator HarvestSequence()
    {
        isAnimating = true;
        
        Sprite[] growthSprites = GetGrowthSprites();
        
        if (growthSprites != null && growthSprites.Length > 0)
        {
            float frameDelay = 0.05f;
            
            for (int i = growthSprites.Length - 1; i >= 0; i--)
            {
                spriteRenderer.sprite = growthSprites[i];
                yield return new WaitForSeconds(frameDelay);
            }
        }
        
        isAnimating = false;
    }
    
    private Sprite[] GetGrowthSprites()
    {
        if (cropController == null) return null;
        
        string cropName = cropController.GetCropTypeName().ToLower();
        Sprite[] sprites = new Sprite[6];
        
        for (int i = 0; i <= 5; i++)
        {
            string path = $"Sunnyside_World_Assets/Elements/Crops/{cropName}_0{i}";
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                sprites[i] = sprite;
            }
        }
        
        return sprites;
    }
}
