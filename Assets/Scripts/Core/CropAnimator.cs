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
        
        Sprite[] growthSprites = SpriteFactory.GetCropGrowthSprites(cropController.GetCropType());
        
        if (growthSprites != null && growthSprites.Length > 0)
        {
            int lastFrame = growthSprites.Length - 1;
            int secondLastFrame = growthSprites.Length - 2;
            
            Sprite[] wobbleSprites = new Sprite[] { growthSprites[lastFrame], growthSprites[secondLastFrame] };
            IAnimationStrategy wobbleStrategy = new LoopAnimationStrategy(wobbleSprites, 0.3f);
            
            while (!isAnimating)
            {
                yield return StartCoroutine(wobbleStrategy.Play(spriteRenderer));
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
        
        Sprite[] growthSprites = SpriteFactory.GetCropGrowthSprites(cropController.GetCropType());
        
        if (growthSprites != null && growthSprites.Length > 0)
        {
            IAnimationStrategy harvestStrategy = new ReverseAnimationStrategy(growthSprites, GameConstants.Animation.FastFrameDelay);
            yield return StartCoroutine(harvestStrategy.Play(spriteRenderer));
        }
        
        isAnimating = false;
    }
    
    private Sprite[] GetGrowthSprites()
    {
        if (cropController == null) return null;
        return SpriteFactory.GetCropGrowthSprites(cropController.GetCropType());
    }
}
