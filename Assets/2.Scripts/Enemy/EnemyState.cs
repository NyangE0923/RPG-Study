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

    //EnemyStateŬ������ ������
    //�� �����ڴ� EnemyState������� ������� ������Ʈ�� ������ �� ȣ�� �Ǹ�, ������Ʈ�� �ʱ�ȭ�� ���.
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    //���� �޼��� ������Ʈ
    public virtual void Update()
    {
        //stateTimer�� Time.deltaTime�� ���� ���� ����.
        stateTimer -= Time.deltaTime;
    }
    //���� �޼��� ����
    public virtual void Enter()
    {
        rb = enemyBase.rb;
        triggerCalled = false;
        //enemyŬ������ �ִϸ��̼�(�̸�, Ȱ��ȭ)
        enemyBase.anim.SetBool(animBoolName, true);
    }
    //���� �޼��� ������
    public virtual void Exit()
    {
        //enemyŬ������ �ִϸ��̼�(�̸�, ��Ȱ��ȭ)
        enemyBase.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
