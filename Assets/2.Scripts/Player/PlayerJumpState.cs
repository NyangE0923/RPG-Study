using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //조건문if (rigidbody2D의 velocity의 Y값이 0미만이라면 PlayerStateMachine에게 상속받은 PlayerAirState로 이동한다.
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
