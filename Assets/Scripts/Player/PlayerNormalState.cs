using UnityEngine;

public class PlayerNormalState : IPlayerState
{
    private readonly PlayerController player;
    private readonly Rigidbody2D rb;
    private readonly PlayerAnimator animator;
    private readonly float moveSpeed;
    private Vector2 currentMoveInput;

    public PlayerNormalState(PlayerController player, Rigidbody2D rb, PlayerAnimator animator, float moveSpeed)
    {
        this.player = player;
        this.rb = rb;
        this.animator = animator;
        this.moveSpeed = moveSpeed;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
        Vector2 velocity = currentMoveInput * moveSpeed;
        rb.linearVelocity = velocity;

        Vector3 clampedPosition = player.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, player.MinX, player.MaxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, player.MinY, player.MaxY);
        player.transform.position = clampedPosition;

        if (animator != null)
        {
            animator.UpdateMovement(currentMoveInput);
        }
    }

    public void UpdateMovement(Vector2 moveInput)
    {
        currentMoveInput = moveInput;
    }
}
