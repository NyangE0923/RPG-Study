using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Idle상태로 진입했을때 모든 속도를 0으로 변경한다.
        //rb.velocity = new Vector2(0, 0);
        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        //xInput의 값이 0이 아니거나 isBusy가 false일때 moveState로 상태를 변환한다.
        if(xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
