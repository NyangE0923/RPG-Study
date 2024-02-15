using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    //부모 클래스로부터 메소드를 오버라이드(재정의)한다.
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

        //Player 클래스의 SetVelocity 메서드에 있는 xInput변수 * moveSpeed와 rigidbody2D의 velocity의 y축을 상속받는다.
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        //xInput의 값이 0일때 idleState로 상태를 변환한다.
        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
