using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    //EnemyStateMachine �б� ���� �Ӽ�����
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //���ο� �ν��Ͻ� ����
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        //EnemyState�� currentState(���� ����)�� ȣ���Ͽ� ���¸� ������Ʈ�Ѵ�.
        stateMachine.currentState.Update();
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // ���� ���� �ӽ��� currentState���� AnimationFinishTrigger �޼��带 ȣ���Ѵ�.
    // AnimationFinishTrigger �޼���� ���� ���¿��� �ִϸ��̼��� �Ϸ�Ǿ��� �� ����Ǵ� �޼���.
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    //���� �޼��� ����
    //Physics2D�� ����ĳ��Ʈ�� ��Ȯ�� ����� ��ġ�������� X���� �ٶ󺸴� ����, Y���� 50 ���� �÷��̾ �����Ѵ�.
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //����� ��������� ����
        Gizmos.color = Color.yellow;
        // ������� DrawLine�� ��ü�� ��ġ���� attackDistance�� ���� ��ġ���� ���� �׸���.
        // ���ο� Vector3�� ������ ���� ����
        // - x ��ǥ�� ��ü�� ��ġ�� attackDistance�� ���� ���̸�, �̴� �ٶ󺸴� ����(facingDir)�� ���� -1 �Ǵ� 1�� ��
        // - y ��ǥ�� ��ü�� ���� y ��ǥ�� �״�� ������
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
