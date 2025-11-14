using UnityEngine;

public class PlayerHurtState : IPlayerState
{
    private readonly Rigidbody2D rb;
    private readonly PlayerAnimator animator;
    private readonly float duration;
    private float timer;

    public PlayerHurtState(Rigidbody2D rb, PlayerAnimator animator, float duration)
    {
        this.rb = rb;
        this.animator = animator;
        this.duration = duration;
    }

    public void Enter()
    {
        timer = duration;
        if (animator != null)
        {
            animator.PlayHurt();
        }
    }

    public void Exit()
    {
        if (animator != null)
        {
            animator.StopHurt();
        }
    }

    public void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
    }

    public void UpdateMovement(Vector2 moveInput)
    {
    }

    public bool IsComplete()
    {
        return timer <= 0f;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        rb.linearVelocity = direction * force;
    }
}
