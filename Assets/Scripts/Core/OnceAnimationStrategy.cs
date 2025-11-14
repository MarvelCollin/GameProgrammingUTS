using UnityEngine;
using System.Collections;

public class OnceAnimationStrategy : IAnimationStrategy
{
    private readonly Sprite[] sprites;
    private readonly float frameDelay;

    public OnceAnimationStrategy(Sprite[] sprites, float frameDelay)
    {
        this.sprites = sprites;
        this.frameDelay = frameDelay;
    }

    public IEnumerator Play(SpriteRenderer spriteRenderer)
    {
        if (sprites == null || sprites.Length == 0) yield break;

        foreach (Sprite sprite in sprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(frameDelay);
        }
    }
}
