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

        //애니메이션을 마지막 애니메이션으로 설정한다
        //애니메이션의 재생 속도를 0으로 설정한다.
        //객체의 캡슐 콜라이더2D를 비활성화 시킨다.
        //상태 타이머를 0.15f로 설정한다.
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();
        //상태 타이머는 time.deltatime에 의해 점점 줄어들며
        //만약 상태 타이머가 0이하가 되면 객체의 Velocity를 새로운 Vector2값인
        //x값은 0 y값은 10을 준다.
        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
