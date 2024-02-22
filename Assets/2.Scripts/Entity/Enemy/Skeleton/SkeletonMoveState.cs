using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
        //Entity�� ��ӹ��� enemy�� SetVelocity ���
        //�ٶ󺸴� �������� 2��ŭ �̵��ϰ�, y���� �״�� ���� ������Ʈ�� ���� �����Ѵ�.
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

        //���� ���� �����߰ų�, ���� �������� ���ߴٸ�
        //������ ��ȯ�Ѵ�. �׸��� idleState ���·� �̵��Ѵ�.
        //������ �ٲٰ� ��� ���߰� �̵��ϵ��� ������
        if(enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
