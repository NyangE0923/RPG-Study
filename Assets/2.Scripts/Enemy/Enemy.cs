using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
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
        if(IsPlayerDetected())
            Debug.Log(IsPlayerDetected().collider.gameObject.name + " I SEE");
    }

    //���� �޼��� ����
    //Physics2D�� ����ĳ��Ʈ�� ��Ȯ�� ����� ��ġ�������� X���� �ٶ󺸴� ����, Y���� 50 ���� �÷��̾ �����Ѵ�.
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
}
