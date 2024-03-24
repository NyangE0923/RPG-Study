using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 12f;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing; //ƨ��⸦ �� ������
    private int bounceAmount; //ƨ��� Ƚ��
    private List<Transform> enemyTarget; //Ÿ���� ���� ����Ʈ
    private int targetIndex; //�ִ� Ÿ�� ��

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    [SerializeField] private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    #region �ٸ� ��ũ��Ʈ�� �޼��忡�� ����� �� �ֵ��� Ư�� ������ �����ϰ� �ʱ�ȭ �ϴ� �� ����ϴ� ��.
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _FreezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _FreezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        //���� ��ų�� ������ �̵� ���⿡ ���� �ڵ�
        //mathf.clamp(value, min, max) �־��� ���� �ּҰ��� �ִ밪 ������ ������ �����Ѵ�.
        //���� min���� ������ min������, max���� ũ�� max ������ �����Ѵ�.
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 5);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>(); //���ο� ��ġ���� ���� ����Ʈ�� �ʱ�ȭ �Ѵ�.
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    #endregion
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rigidbody�� ���� �ùķ����� ����
        //rb.isKinematic = false;

        //�θ� �ڽ� ���踦 ������
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        //isReturning�� true���
        if (isReturning)
        {
            //MoveTowards�޼��带 ����Ͽ� ���� ��ġ���� �÷��̾� ��ġ�� returnSpeed * Time.deltaTime�� �ӵ��� ���ƿ´�.
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //�Ÿ��� �����ϴ� Distance�޼��带 ����� ���� ��ġ�� �÷��̾��� ��ġ�� 2���� �۴ٸ� ClearTheSword �޼��带 ȣ���Ѵ�.
            //�̶� ClearTheSword �޼��� �ȿ��� Destroy�Լ��� �� ����.
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            //Vector2.Distance(�ΰ��� Vector2 ��ü ������ �Ÿ��� �����ϴ� �޼���)
            //�÷��̾��� ��ġ�� ���� ��ġ�� maxTravelDistance���� �ʰ��̸鼭 wasStopped(�� ���� ����)�� �ƴҰ��
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime; //spinTimer���� Time.deltaTime��ŭ ����



                //transform.position == ������ġ
                //Vector2.MoveTowards == current(������ġ)���� target(��ǥ��ġ)���� �̵��ϴµ� ���ȴ�.
                //���� : ������ġ���� spinDirection��ŭ �̵��� ��ġ�� �����Ǹ� y���� ���� ��ġ�� �����ϴ�.
                //�̶� �ӵ��� 1.5f * Time.deltaTime���� �����ؼ� ������ �ӵ�(1.5f)�� �̵��ϵ��� �����Ѵ�.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection,transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0) //spinTimer�� 0���� �۾��� ���
                {
                    isReturning = true; //���� �ǵ��ƿ����� �ϰ�
                    isSpinning = false; //�� ȸ���� �ߴ��Ѵ�.
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    //������ġ���� ������ 1�� ������ ����� �����ȿ� ���� �ݶ��̴� ��ü�� ��� colliders�迭�� ��´�.

                    foreach (var hit in colliders) //colliders �迭�� �ִ� �� �ݶ��̴��� ���� �ݺ��Ѵ�.
                    {
                        if (hit.GetComponent<Enemy>() != null) //�ش� �ݶ��̴��� Enemy ������Ʈ�� ������ �ִٸ�
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true; //�� ���� ����(bool)�� true�� �����ؼ� ���� ���ߵ��� �ϰ�
                           //constraints == Rigidbody2D�� ������ �����ϴ� �Ӽ�
                           //RigidbodyConstraints2D.FreezePosition == Rigidbody2D�� ��ġ�� ������Ű�� �Լ�
        rb.constraints = RigidbodyConstraints2D.FreezePosition; //���� �ش� ��ġ�� ������Ų��.
        spinTimer = spinDuration; //�� ȸ�� �ð��� spinDuration(���ӽð�)��ŭ���� �����Ѵ�.
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0) //���� isBouncing�� ���̰� enemyTarget ����Ʈ�� ���� �ϳ� �̻� �����Ѵٸ�
        {
            //MoveTowards == current(������ġ), target(��ǥ��ġ) float���� maxDistanceDelta(current�� target ������ �̵��� �ִ� �Ÿ�)
            //������ġ���� enemyTarget����Ʈ�� ��� �� �ݶ��̴� ��ġ�� �ִ� 20��ŭ�� �Ÿ��� Time.deltaTime�� ����ؼ� �̵��Ѵ�.
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            //Distance(�Ÿ� ���) �ڽ��� ��ġ�� ���� ��ġ�� �ش��ϴ� ����Ʈ �ε����� �Ÿ��� 0.1���� �۴ٸ�
            //Ÿ�� �ε����� �������� ���� ���� ������ �� �ֵ��� �Ѵ�.
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                //enemyTarget ����Ʈ���� targetIndex�� ������ �ε����� �ش��ϴ� ��Ҹ� ��������
                //�ش� ��Ұ� Enemy������Ʈ�� ������ �ִ��� Ȯ���Ѵ�.
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--; //ƨ��� Ƚ�� ����
                if (bounceAmount <= 0) //ƨ��� Ƚ���� 0���ϰ� �Ǹ�
                {
                    isBouncing = false; //isBounCing�� false�� �ٲٰ�
                    isReturning = true; //isReturning�� true�� �����Ѵ�.
                }

                //enemyTarget����Ʈ�� Count(��)�� �ִ� Ÿ�ټ�(targetIndex)���� �̻��̶��
                //targetIndex �ִ� Ÿ�ټ��� 0���� �Ѵ�.
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInfo(collision);
    }

    private void SwordSkillDamage(Enemy enemy) //Enemy Ŭ������ �Ű������� �����´�.
    {
        enemy.DamageEffect(); //enemy�� ������ �޼���
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration); //enemy�� �ڷ�ƾ
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) //Enemy ������Ʈ�� ������ �ִٸ�
        {
            if (isBouncing && enemyTarget.Count <= 0) //�ٿ�̰� �� Ÿ���� 0���϶��
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                //������ġ���� ������ 10�� ������ ����� �����ȿ� ���� �ݶ��̴� ��ü�� ��� colliders�迭�� ��´�.

                foreach (var hit in colliders) //colliders �迭�� �ִ� �� �ݶ��̴��� ���� �ݺ��Ѵ�.
                {
                    if (hit.GetComponent<Enemy>() != null) //�ش� �ݶ��̴��� Enemy ������Ʈ�� ������ �ִٸ�
                        enemyTarget.Add(hit.transform); //�ش� �ݶ��̴��� tranform(��ġ)�� enemyTarget����Ʈ�� �߰��Ѵ�.
                }
            }
        }
    }

    private void StuckInfo(Collider2D collision) //Collider2D�� collision������ �Ű������� �޾Ƶ��̴� �޼���
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        //pierceAmount�� 0���� ũ�� �ݶ��̴��� ��� ��ü�� Enemy������Ʈ�� ������ �ִٸ�
        {
            pierceAmount--; //pierceAmount�� 1 ����
            return; //��ȯ�Ѵ�. (���� ������ ��µ��� ����)
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;
        //rigidbody�� ���� �ùķ����� ����
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        //���� �ݶ��̴� ��ü�� transform�� �θ�� ����
        transform.parent = collision.transform;
    }
}

