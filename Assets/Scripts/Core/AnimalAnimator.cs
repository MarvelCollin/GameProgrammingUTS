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
            IAnimationStrategy idleStrategy = new LoopAnimationStrategy(animalSprites, GameConstants.Animation.SlowFrameDelay);
            yield return StartCoroutine(idleStrategy.Play(spriteRenderer));
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
            for (int cycle = 0; cycle < 2; cycle++)
            {
                IAnimationStrategy interactionStrategy = new OnceAnimationStrategy(animalSprites, 0.08f);
                yield return StartCoroutine(interactionStrategy.Play(spriteRenderer));
            }
        }
    }
    
    private Sprite[] GetAnimalSprites()
    {
        if (animalController == null) return null;
        return SpriteFactory.GetAnimalSprites(animalController.GetAnimalType());
    }
}
