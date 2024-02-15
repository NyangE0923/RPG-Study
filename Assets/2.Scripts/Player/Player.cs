using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Wall Jump info")]
    public float xWallJumpForce;
    public float yWallJumpForce;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region Componets
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion
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
    private void Awake()
    {
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

    private void Start()
    {
        //주의 위에서부터 시작되므로 구성요소부터 얻고 초기화 해야함!!
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(idleState);
    }

    private void Update()
    {
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
    #region Velocity
    //이동속도를 0으로 만드는 함수
    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);

    //SetVelocity 라는 메서드(float x, y를 가진)를 생성한다.
    //이때 rigidbody2D의 velocity는 새로운 velocity _xVelocity, _yVelocity로 선언한다.
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
    #region Collision
    //땅 탐지
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    //벽 탐지 right라고 썼지만 facingDir을 통해 양쪽 방향 모두 탐지 가능
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        // 지면 체크용 선을 그립니다. groundCheck.position은 현재 groundCheck의 위치입니다.
        // 그리고 새로운 Vector3을 만들어 groundCheck 위치에서 (x 좌표는 groundCheck의 x 좌표와 같으며,
        // y 좌표는 groundCheck의 y 좌표에서 groundCheckDistance 만큼 아래에 있는 지점으로 생성합니다).
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        // 벽 체크용 선을 그립니다. wallCheck.position은 현재 wallCheck의 위치입니다.
        // 그리고 새로운 Vector3을 만들어 wallCheck 위치에서 (x 좌표는 wallCheck의 x 좌표에서
        // wallCheckDistance 만큼 오른쪽에 있는 지점으로, y 좌표는 wallCheck의 y 좌표와 같은 곳으로 생성합니다).
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

    }
    #endregion
    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    
    public void FlipController(float _x)
    {
        //Player의 X축 이동이 0보다 크면서 Player가 오른쪽을 바라보고 있지 않다면 Flip함수를 사용한다.
        if(_x > 0 && !facingRight)
        {
            Flip();
        }
        //Player의 X축 이동이 0보다 작으면서 Player가 오른쪽을 바라보고 있다면 Flip함수를 사용한다.
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
