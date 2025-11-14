using UnityEngine;
using System.Collections;

public class LoopAnimationStrategy : IAnimationStrategy
{
    private readonly Sprite[] sprites;
    private readonly float frameDelay;

    public LoopAnimationStrategy(Sprite[] sprites, float frameDelay)
    {
        this.sprites = sprites;
        this.frameDelay = frameDelay;
    }

    public IEnumerator Play(SpriteRenderer spriteRenderer)
    {
        if (sprites == null || sprites.Length == 0) yield break;

        int currentFrame = 0;
        while (true)
        {
            spriteRenderer.sprite = sprites[currentFrame];
            currentFrame = (currentFrame + 1) % sprites.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }
}
