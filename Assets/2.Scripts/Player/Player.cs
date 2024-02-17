using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public bool isBusy {  get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown; //인스펙터 창에서 설정할 대시 쿨타임
    private float dashUsageTimer; //Time.DeltaTime으로 초기화 시킬 대시 쿨타임
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }

    [Header("Wall Jump info")]
    public float xWallJumpForce;
    public float yWallJumpForce;


    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        //PlayerState에서 가져올 정보들 Player라는 오브젝트가 StateMachine에 있는 Idle 애니메이션을 가져온다.
        StateMachine = new PlayerStateMachine();
        //idleState는 bool값인 Idle에 해당한다.
        idleState = new PlayerIdleState(this, StateMachine, "Idle");
        //moveState는 bool값인 Move에 해당한다.
        moveState = new PlayerMoveState(this, StateMachine, "Move");
        dashState = new PlayerDashState(this, StateMachine, "Dash");
        jumpState = new PlayerJumpState(this, StateMachine, "Jump");
        airState  = new PlayerAirState(this, StateMachine, "Jump");
        wallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, StateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.currentState.Update();
        CheckForDashInput();
    }

    //주어진 시간 동안 isBusy를 true로 설정한 후, 해당 시간이 경과하면 다시 false로 설정한다.
    public IEnumerator BusyFor(float _seconds)
    {
        //isBusy 변수를 true로 설정하여 현재 작업 중임을 나타냅니다.
        isBusy = true;

        //waitForSecounds(_seconds)를 사용하여 주어진 시간(_seconds)만큼 대기한다.
        //이를 통해 지정된 시간만큼 코루틴이 대기하고, 대기가 완료되면 다음 코드인 isBusy = false;를 진행함
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //람다 표현식의 화살표 연산자를 사용해 PlayerStateMachine클래스의 CurrentStage에 접근하고 AnimationFinishTrigger메서드를 호출한다.
    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        dashUsageTimer -= Time.deltaTime; //대시 쿨타임을 델타 타임만큼 뺀다.

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0) //대시 쿨타임이 0미만이 되었을 때 대시를 누르면
        {
            dashUsageTimer = dashCooldown; //다시 인스펙터창에서 정해둔 쿨타임으로 되돌리기
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            StateMachine.ChangeState(dashState); //dashState로 이동
        }
    }
}
