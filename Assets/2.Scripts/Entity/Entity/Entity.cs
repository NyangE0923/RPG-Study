using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Componets
    //Animator �б� ���� �Ӽ�����
    public Animator anim { get; private set; }
    //Rigidbody2D �б� ���� �Ӽ�����
    public Rigidbody2D rb { get; private set; }
    //EntityFX Ŭ���� �б� ���� �Ӽ�����
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
        //���� ���������� ���۵ǹǷ� ������Һ��� ��� �ʱ�ȭ �ؾ���!!
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
        //�˹��� Ȱ��ȭ �Ѵ�.
        isknocked = true;
        //rigidbody2D velocity�� ���� ���� ������ �ݴ� �������� �з�����.
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        //knocbackDuration ���� �ð���ŭ ��ȯ�Ѵ�.
        yield return new WaitForSeconds(knockbackDuration);
        //�˹��� ��Ȱ��ȭ �Ѵ�.
        isknocked = false;
    }

    #region Collision
    //�� Ž��
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    //�� Ž�� right��� ������ facingDir�� ���� ���� ���� ��� Ž�� ����
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        // ���� üũ�� ���� �׸���. groundCheck.position�� ���� groundCheck�� ��ġ
        // �׸��� ���ο� Vector3�� ����� groundCheck ��ġ���� (x ��ǥ�� groundCheck�� x ��ǥ�� ������,
        // y ��ǥ�� groundCheck�� y ��ǥ���� groundCheckDistance ��ŭ �Ʒ��� �ִ� �������� �����Ѵ�).
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        // �� üũ�� ���� �׸���. wallCheck.position�� ���� wallCheck�� ��ġ
        // �׸��� ���ο� Vector3�� ����� wallCheck ��ġ���� (x ��ǥ�� wallCheck�� x ��ǥ����
        // wallCheckDistance ��ŭ �����ʿ� �ִ� ��������, y ��ǥ�� wallCheck�� y ��ǥ�� ���� ������ �����Ѵ�).
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //���� üũ�� ���� �׸���. attackCheck.position�� ���� attackCheck�� ��ġ
        //�׸��� ���� �������� attackCheckRadius ������ ���� ���� �����ȴ�.
        //�� ���� ���� ������ �ð������� ǥ���ϱ� ���� ����
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }
    #region Velocity
    //�̵��ӵ��� 0���� ����� �Լ�
    public void SetZeroVelocity()
    {
        if (isknocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    //SetVelocity ��� �޼���(float x, y�� ����)�� �����Ѵ�.
    //�̶� rigidbody2D�� velocity�� ���ο� velocity _xVelocity, _yVelocity�� �����Ѵ�.
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        //�˹��� Ȱ��ȭ �Ǿ����� ���� �̵��ϴ� ���� ��ȯ�Ѵ�.(�̵� ���Ѵٴ� ��)
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
        //Player�� X�� �̵��� 0���� ũ�鼭 Player�� �������� �ٶ󺸰� ���� �ʴٸ� Flip�Լ��� ����Ѵ�.
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        //Player�� X�� �̵��� 0���� �����鼭 Player�� �������� �ٶ󺸰� �ִٸ� Flip�Լ��� ����Ѵ�.
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
