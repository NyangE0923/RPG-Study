using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    public Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //�ִϸ��̼��� ������ �ִϸ��̼����� �����Ѵ�
        //�ִϸ��̼��� ��� �ӵ��� 0���� �����Ѵ�.
        //��ü�� ĸ�� �ݶ��̴�2D�� ��Ȱ��ȭ ��Ų��.
        //���� Ÿ�̸Ӹ� 0.15f�� �����Ѵ�.
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();
        //���� Ÿ�̸Ӵ� time.deltatime�� ���� ���� �پ���
        //���� ���� Ÿ�̸Ӱ� 0���ϰ� �Ǹ� ��ü�� Velocity�� ���ο� Vector2����
        //x���� 0 y���� 10�� �ش�.
        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
