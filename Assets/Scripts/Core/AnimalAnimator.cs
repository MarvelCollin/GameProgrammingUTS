using UnityEngine;
using System.Collections;

public class AnimalAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AnimalController animalController;
    private Coroutine idleCoroutine;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animalController = GetComponent<AnimalController>();
    }
    
    private void Start()
    {
        PlayIdleAnimation();
    }
    
    public void PlayIdleAnimation()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
        }
        idleCoroutine = StartCoroutine(IdleLoop());
    }
    
    private IEnumerator IdleLoop()
    {
        if (animalController == null) yield break;
        
        Sprite[] animalSprites = GetAnimalSprites();
        
        if (animalSprites != null && animalSprites.Length > 0)
        {
            float frameDelay = 0.15f;
            int currentFrame = 0;
            
            while (true)
            {
                spriteRenderer.sprite = animalSprites[currentFrame];
                currentFrame = (currentFrame + 1) % animalSprites.Length;
                yield return new WaitForSeconds(frameDelay);
            }
        }
    }
    
    public void PlayInteractionAnimation()
    {
        StartCoroutine(InteractionSequence());
    }
    
    private IEnumerator InteractionSequence()
    {
        Sprite[] animalSprites = GetAnimalSprites();
        
        if (animalSprites != null && animalSprites.Length > 0)
        {
            float frameDelay = 0.08f;
            
            for (int i = 0; i < animalSprites.Length * 2; i++)
            {
                spriteRenderer.sprite = animalSprites[i % animalSprites.Length];
                yield return new WaitForSeconds(frameDelay);
            }
        }
    }
    
    private Sprite[] GetAnimalSprites()
    {
        if (animalController == null) return null;
        
        string spritePath = animalController.GetAnimalSpritePath();
        
        if (!string.IsNullOrEmpty(spritePath))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
            return sprites != null && sprites.Length > 0 ? sprites : null;
        }
        
        return null;
    }
}
