using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        //조건문if (rigidbody2D의 velocity의 Y값이 0과 같다면 PlayerStateMachine에게 상속받은 PlayerIdleState로 이동한다.
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        //조건문if (xInput 이동 입력이 0이 아닐 경우)
        if (xInput != 0)
        {
            //player의 SetVelocity의 moveSpeed를 80%의 속도로 이동되도록 한다.
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
        }
    }
}
