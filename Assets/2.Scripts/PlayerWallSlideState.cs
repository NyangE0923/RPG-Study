using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        //Input.GetKeyDown 스페이스바를 누르면 PlayerWallJumpState로 이동하고 반환한다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        //바라보는 방향과 다른 방향으로 입력이 발생하면 idle 상태로 이동 
        if (xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState);

        //yInput = Vertical(Y축의 이동)이 미만으로 입력될경우 즉 아래로 이동하고자 한다면 기본값으로 내려가지고
        //그게 아니라면(입력하지 않는다면) 70%의 속도로 내려간다.
        if(yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);


        if (player.IsGroundDetected() || !player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
