using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        //Vector2 mousePosition�̶�� ������ �����Ѵ�.
        //�̶� ������ ���� CameraŬ������ ScreenToWorldPoint�޼��带 �̿��Ͽ� ���콺�� ���� ��ġ�� ������ �Ѵ�.
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //���� �÷��̾��� x��ġ�� ���콺�� x��ġ�� �÷��̾� ������ �������� ��� �÷��̾��� ������ ��ȯ�Ѵ�.
        if (player.transform.position.x > mousePostion.x && player.facingDir == 1)
            player.Flip();
        //���� �÷��̾��� x��ġ�� ���콺�� x��ġ�� �÷��̾� ������ ������ ��� �÷��̾��� ������ ��ȯ�Ѵ�.
        else if(player.transform.position.x < mousePostion.x && player.facingDir == -1)
            player.Flip();
    }
}
