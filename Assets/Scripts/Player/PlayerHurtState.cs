using UnityEngine;

public class PlayerHurtState : IPlayerState
{
    private readonly PlayerController player;
    private readonly Rigidbody2D rb;
    private readonly PlayerAnimator animator;
    private readonly float duration;
    private float timer;

    public PlayerHurtState(PlayerController player, Rigidbody2D rb, PlayerAnimator animator, float duration)
    {
        this.player = player;
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

        Vector3 clampedPosition = player.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, player.MinX, player.MaxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, player.MinY, player.MaxY);
        player.transform.position = clampedPosition;
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
