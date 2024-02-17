using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    //�����ڿ� Enemy_Skeleton�� enemy�� �߰�
    private Enemy_Skeleton enemy;
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        //enemy�� �򰥸��� �����Ƿ� _(������ھ�)�� ����Ͽ� �˾ƺ��� ���� �����
        //this�� ����� �� Ŭ���� �������� enemy�� Enemy_Skeleton�� ��ӹ޴� enemy�� �����.
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //Time.DeltaTime���� ���ص� stateTimer(���� ���ӽð�)�� 0�ʰ� �Ǹ�
        //ChangeState ���� ����
        //enemy.moveState Enemy_Skeleton�� moveState�� �̵�
        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy.moveState);
    }
}
