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

    // 현재 상태 머신의 currentState에서 AnimationFinishTrigger 메서드를 호출한다.
    // AnimationFinishTrigger 메서드는 현재 상태에서 애니메이션이 완료되었을 때 실행되는 메서드.
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    //가상 메서드 생성
    //Physics2D의 레이캐스트를 벽확인 기즈모 위치에서부터 X축은 바라보는 방향, Y축은 50 으로 플레이어를 감지한다.
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //기즈모를 노란색으로 설정
        Gizmos.color = Color.yellow;
        // 기즈모의 DrawLine은 객체의 위치에서 attackDistance를 더한 위치까지 선을 그린다.
        // 새로운 Vector3는 다음과 같이 구성
        // - x 좌표는 객체의 위치에 attackDistance를 더한 값이며, 이는 바라보는 방향(facingDir)에 따라 -1 또는 1이 됨
        // - y 좌표는 객체의 현재 y 좌표를 그대로 유지함
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
