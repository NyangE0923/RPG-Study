using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Componets
    //Animator 읽기 전용 속성정의
    public Animator anim { get; private set; }
    //Rigidbody2D 읽기 전용 속성정의
    public Rigidbody2D rb { get; private set; }

    #endregion


    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        //주의 위에서부터 시작되므로 구성요소부터 얻고 초기화 해야함!!
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }
    #region Collision
    //땅 탐지
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    //벽 탐지 right라고 썼지만 facingDir을 통해 양쪽 방향 모두 탐지 가능
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
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
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        //Player의 X축 이동이 0보다 크면서 Player가 오른쪽을 바라보고 있지 않다면 Flip함수를 사용한다.
        if (_x > 0 && !facingRight)
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
