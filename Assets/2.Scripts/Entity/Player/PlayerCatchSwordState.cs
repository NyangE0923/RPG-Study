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

        //���� �÷��̾��� x��ġ�� ���콺�� x��ġ�� �÷��̾� ������ �������� ��� �÷��̾��� ������ ��ȯ�Ѵ�.
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        //���� �÷��̾��� x��ġ�� ���콺�� x��ġ�� �÷��̾� ������ ������ ��� �÷��̾��� ������ ��ȯ�Ѵ�.
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
