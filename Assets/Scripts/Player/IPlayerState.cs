using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Exit();
    void FixedUpdate();
    void UpdateMovement(Vector2 moveInput);
}
