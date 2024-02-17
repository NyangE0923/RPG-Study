using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    //생성자에 Enemy_Skeleton을 enemy로 추가
    private Enemy_Skeleton enemy;
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        //enemy가 헷갈릴수 있으므로 _(언더스코어)를 사용하여 알아보기 쉽게 만들고
        //this를 사용해 이 클래스 내에서의 enemy는 Enemy_Skeleton을 상속받는 enemy만 사용함.
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

        //Time.DeltaTime으로 정해둔 stateTimer(상태 지속시간)이 0초가 되면
        //ChangeState 상태 변경
        //enemy.moveState Enemy_Skeleton의 moveState로 이동
        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy.moveState);
    }
}
