using UnityEngine;
using System.Collections;

public interface IAnimationStrategy
{
    IEnumerator Play(SpriteRenderer spriteRenderer);
}
