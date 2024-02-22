using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;
    private float lastTimeAttacked;
    //���� �����ð�
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;

        //�޺� ī���Ͱ� 2 �̻��̰ų� ������ �������κ��� comboWindow ��ŭ �����ٸ� �޺� ī���͸� 0���� �ʱ�ȭ�Ѵ�.
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        //Player�� �ִϸ��̼ǿ��� ComboCounter�� comboCounter�� �°� SetInteger �޼��带 �̿��Ͽ� �ִϸ��̼ǿ� �����Ѵ�.
        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose attack direction

        //�̵��ϸ� attackDir�� xInput�� ���� ������ �� ���� ĳ������ ����� ����.
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion

        // player Ŭ������ SetVelocity �޼��带 ȣ���Ͽ� �÷��̾��� �ӵ��� �����Ѵ�.
        // �÷��̾��� ���� �������� player.attackMovement �迭���� comboCounter(0,1,2)�� �ش��ϴ� ������ �����ȴ�.
        // player.facingDir�� �÷��̾ �ٶ󺸴� ������ ��Ÿ����, �������� �ٶ� ���� -1, ���������� �ٶ� ���� 1
        // rb.velocity.y�� �÷��̾��� ���� ����(���� �Ǵ� �Ʒ���)������ �ӵ��� ��Ÿ���� �����̹Ƿ� ���� ������ �������� ���� �ӵ��� �����մϴ�.
        //�� �����Ҷ� �ٶ󺸴� �������� �����Ÿ� �̵��ϴ� �ڵ�!
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        //�ڷ�ƾ�� �����Ѵ�.
        player.StartCoroutine("BusyFor", 0.15f);

        //�޺� ī���͸� 1�� �״´�.
        comboCounter++;
        //������ ������ �ð��� ����Ѵ�.
        lastTimeAttacked = Time.time;
        Debug.Log(lastTimeAttacked);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        //������ �����ٸ� PlayerIdleState�� �̵�
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
