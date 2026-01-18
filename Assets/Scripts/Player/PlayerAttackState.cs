using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly PlayerController player;
    private readonly Rigidbody2D rb;
    private readonly PlayerAnimator animator;
    private readonly float duration;
    private float timer;

    public PlayerAttackState(PlayerController player, Rigidbody2D rb, PlayerAnimator animator, float duration)
    {
        this.player = player;
        this.rb = rb;
        this.animator = animator;
        this.duration = duration;
    }

    public void Enter()
    {
        Debug.Log("PlayerAttackState.Enter() - Starting attack animation");
        timer = duration;
        rb.linearVelocity = Vector2.zero;
        if (animator != null)
        {
            animator.PlayAttack();
        }
    }

    public void Exit()
    {
        if (animator != null)
        {
            animator.StopAttack();
        }
    }

    public void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        rb.linearVelocity = Vector2.zero;

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
}
