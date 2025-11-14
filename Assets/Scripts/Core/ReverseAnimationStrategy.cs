using UnityEngine;
using System.Collections;

public class ReverseAnimationStrategy : IAnimationStrategy
{
    private readonly Sprite[] sprites;
    private readonly float frameDelay;

    public ReverseAnimationStrategy(Sprite[] sprites, float frameDelay)
    {
        this.sprites = sprites;
        this.frameDelay = frameDelay;
    }

    public IEnumerator Play(SpriteRenderer spriteRenderer)
    {
        if (sprites == null || sprites.Length == 0) yield break;

        for (int i = sprites.Length - 1; i >= 0; i--)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameDelay);
        }
    }
}
