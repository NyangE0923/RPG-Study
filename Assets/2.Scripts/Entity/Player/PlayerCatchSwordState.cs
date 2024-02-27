using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        //만약 플레이어의 x위치가 마우스의 x위치와 플레이어 방향이 오른쪽일 경우 플레이어의 방향을 전환한다.
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        //만약 플레이어의 x위치가 마우스의 x위치와 플레이어 방향이 왼쪽일 경우 플레이어의 방향을 전환한다.
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
