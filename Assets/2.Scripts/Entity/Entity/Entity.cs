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
    //EntityFX 클래스 읽기 전용 속성정의
    public EntityFX fx { get; private set; }

    #endregion
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isknocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
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
        fx = GetComponentInChildren<EntityFX>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
        Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        //넉백을 활성화 한다.
        isknocked = true;
        //rigidbody2D velocity의 값을 현재 방향의 반대 방향으로 밀려난다.
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        //knocbackDuration 변수 시간만큼 반환한다.
        yield return new WaitForSeconds(knockbackDuration);
        //넉백을 비활성화 한다.
        isknocked = false;
    }

    #region Collision
    //땅 탐지
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    //벽 탐지 right라고 썼지만 facingDir을 통해 양쪽 방향 모두 탐지 가능
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        // 지면 체크용 선을 그린다. groundCheck.position은 현재 groundCheck의 위치
        // 그리고 새로운 Vector3을 만들어 groundCheck 위치에서 (x 좌표는 groundCheck의 x 좌표와 같으며,
        // y 좌표는 groundCheck의 y 좌표에서 groundCheckDistance 만큼 아래에 있는 지점으로 생성한다).
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        // 벽 체크용 선을 그린다. wallCheck.position은 현재 wallCheck의 위치
        // 그리고 새로운 Vector3을 만들어 wallCheck 위치에서 (x 좌표는 wallCheck의 x 좌표에서
        // wallCheckDistance 만큼 오른쪽에 있는 지점으로, y 좌표는 wallCheck의 y 좌표와 같은 곳으로 생성한다).
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //공격 체크용 선을 그린다. attackCheck.position은 현재 attackCheck의 위치
        //그리고 원의 반지름은 attackCheckRadius 변수의 값에 따라 결정된다.
        //이 원은 공격 범위를 시각적으로 표현하기 위해 사용됨
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }
    #region Velocity
    //이동속도를 0으로 만드는 함수
    public void SetZeroVelocity()
    {
        if (isknocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    //SetVelocity 라는 메서드(float x, y를 가진)를 생성한다.
    //이때 rigidbody2D의 velocity는 새로운 velocity _xVelocity, _yVelocity로 선언한다.
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        //넉백이 활성화 되어있을 때는 이동하는 값을 반환한다.(이동 못한다는 뜻)
        if(isknocked)
            return;

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
