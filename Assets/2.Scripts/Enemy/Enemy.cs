using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    //EnemyStateMachine 읽기 전용 속성정의
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //새로운 인스턴스 생성
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        //EnemyState의 currentState(현재 상태)를 호출하여 상태를 업데이트한다.
        stateMachine.currentState.Update();
        if(IsPlayerDetected())
            Debug.Log(IsPlayerDetected().collider.gameObject.name + " I SEE");
    }

    //가상 메서드 생성
    //Physics2D의 레이캐스트를 벽확인 기즈모 위치에서부터 X축은 바라보는 방향, Y축은 50 으로 플레이어를 감지한다.
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
}
