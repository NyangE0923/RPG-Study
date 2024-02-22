using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {

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

        //Time.DeltaTime으로 정해둔 stateTimer(상태 지속시간)이 0초가 되면
        //ChangeState 상태 변경
        //enemy.moveState Enemy_Skeleton의 moveState로 이동
        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy.moveState);
    }
}
