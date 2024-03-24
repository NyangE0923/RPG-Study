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
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    //EnemyStateMachine 읽기 전용 속성정의
    public EnemyStateMachine stateMachine { get; private set; }
    //마지막 애니메이션 Bool값의 이름을 가져오는 프로퍼티
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //새로운 인스턴스 생성
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        //EnemyState의 currentState(현재 상태)를 호출하여 상태를 업데이트한다.
        stateMachine.currentState.Update();
    }

    //가상 메소드 AssignLastAnimName를 생성한다. 이때 매개변수 string _animBoolName값을 주고
    //애니메이션이 재생되고 Exit를 통해 빠져나올때 해당 매개변수가 할당되도록 한다.
    //이후 lastAnimBoolName변수에 해당 매개변수의 값을 주도록 한다.
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool _timeFrozen) //bool 매개변수를 가지고 있는 공용 가상 메서드 FreezeTime 메서드
    {
        if (_timeFrozen) //_timeFrozen이 true라면
        {
            moveSpeed = 0; //moveSpeed를 0으로 만들고, 애니메이션을 중지한다.
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed; //_timeFrozen이 false라면 moveSpeed를 defaultMoveSpeed로 변경하고 애니메이션을 재생한다.
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds) //보호받는 가상 코루틴 FreezeTimerFor(매개변수는 시간을 나타내는 float변수)
    {
        FreezeTime(true); //FreezeTime메서드의 매개변수를 True로 변경한다.

        yield return new WaitForSeconds(_seconds); //yield return을 _seconds 이후에 반환한다.

        FreezeTime(false); //FreezeTime메서드의 매개변수를 false로 변경한다.
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
