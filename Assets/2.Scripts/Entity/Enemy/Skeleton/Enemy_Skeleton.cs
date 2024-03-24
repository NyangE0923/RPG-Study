using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region States

    //각 클래스에 속하는 상태를 가져오거나 설정한다. 이 속성은 읽기 전용이므로 외부에서 값을 변경할 수 없다.
    //private set이기 때문
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    //공용 한정자, Die 메소드를 재정의 한다.
    //상태머신을 통해 deadState로 변경한다.
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
