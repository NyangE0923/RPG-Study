using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    //EnemyState클래스의 생성자
    //이 생성자는 EnemyState기반으로 만들어진 오브젝트가 생성될 때 호출 되며, 오브젝트의 초기화를 담당.
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    //가상 메서드 업데이트
    public virtual void Update()
    {
        //stateTimer는 Time.deltaTime을 빼는 값과 같다.
        stateTimer -= Time.deltaTime;
    }
    //가상 메서드 들어가기
    public virtual void Enter()
    {
        rb = enemyBase.rb;
        triggerCalled = false;
        //enemy클래스의 애니메이션(이름, 활성화)
        enemyBase.anim.SetBool(animBoolName, true);
    }
    //가상 메서드 나가기
    public virtual void Exit()
    {
        //enemy클래스의 애니메이션(이름, 비활성화)
        enemyBase.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
