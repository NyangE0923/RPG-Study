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
    private bool isBouncing; //튕기기를 할 것인지
    private int bounceAmount; //튕기는 횟수
    private List<Transform> enemyTarget; //타겟을 담을 리스트
    private int targetIndex; //최대 타겟 수

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

    #region 다른 스크립트나 메서드에서 사용할 수 있도록 특정 동작을 설정하고 초기화 하는 데 사용하는 것.
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _FreezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _FreezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        //스핀 스킬의 정지후 이동 방향에 대한 코드
        //mathf.clamp(value, min, max) 주어진 값을 최소값과 최대값 사이의 값으로 제한한다.
        //만약 min보다 작으면 min값으로, max보다 크면 max 값으로 제한한다.
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 5);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>(); //새로운 위치값을 담을 리스트를 초기화 한다.
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
        //rigidbody의 물리 시뮬레이터 적용
        //rb.isKinematic = false;

        //부모 자식 관계를 해제함
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        //isReturning이 true라면
        if (isReturning)
        {
            //MoveTowards메서드를 사용하여 현재 위치에서 플레이어 위치로 returnSpeed * Time.deltaTime의 속도로 돌아온다.
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //거리를 측정하는 Distance메서드를 사용해 현재 위치와 플레이어의 위치가 2보다 작다면 ClearTheSword 메서드를 호출한다.
            //이때 ClearTheSword 메서드 안에는 Destroy함수가 들어가 있음.
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
            //Vector2.Distance(두개의 Vector2 객체 사이의 거리를 측정하는 메서드)
            //플레이어의 위치와 검의 위치가 maxTravelDistance보다 초과이면서 wasStopped(검 정지 변수)가 아닐경우
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime; //spinTimer에서 Time.deltaTime만큼 뺀다



                //transform.position == 현재위치
                //Vector2.MoveTowards == current(현재위치)에서 target(목표위치)까지 이동하는데 사용된다.
                //정리 : 현재위치에서 spinDirection만큼 이동한 위치로 설정되며 y축은 현재 위치와 동일하다.
                //이때 속도는 1.5f * Time.deltaTime으로 설정해서 일정한 속도(1.5f)로 이동하도록 보장한다.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection,transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0) //spinTimer가 0보다 작아질 경우
                {
                    isReturning = true; //검이 되돌아오도록 하고
                    isSpinning = false; //검 회전을 중단한다.
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    //현재위치에서 반지름 1인 원형을 만들고 원형안에 담기는 콜라이더 객체를 모두 colliders배열에 담는다.

                    foreach (var hit in colliders) //colliders 배열에 있는 각 콜라이더에 대해 반복한다.
                    {
                        if (hit.GetComponent<Enemy>() != null) //해당 콜라이더가 Enemy 컴포넌트를 가지고 있다면
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true; //검 정지 변수(bool)를 true로 설정해서 검이 멈추도록 하고
                           //constraints == Rigidbody2D의 제약을 설정하는 속성
                           //RigidbodyConstraints2D.FreezePosition == Rigidbody2D의 위치를 고정시키는 함수
        rb.constraints = RigidbodyConstraints2D.FreezePosition; //검을 해당 위치에 고정시킨다.
        spinTimer = spinDuration; //검 회전 시간은 spinDuration(지속시간)만큼으로 설정한다.
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0) //만약 isBouncing이 참이고 enemyTarget 리스트에 적이 하나 이상 존재한다면
        {
            //MoveTowards == current(현재위치), target(목표위치) float값의 maxDistanceDelta(current와 target 사이의 이동할 최대 거리)
            //현재위치에서 enemyTarget리스트에 담긴 적 콜라이더 위치로 최대 20만큼의 거리를 Time.deltaTime에 비례해서 이동한다.
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            //Distance(거리 계산) 자신의 위치와 적의 위치에 해당하는 리스트 인덱스의 거리가 0.1보다 작다면
            //타겟 인덱스를 증가시켜 다음 적을 추적할 수 있도록 한다.
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                //enemyTarget 리스트에서 targetIndex로 지정된 인덱스에 해당하는 요소를 가져오고
                //해당 요소가 Enemy컴포넌트를 가지고 있는지 확인한다.
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--; //튕기는 횟수 차감
                if (bounceAmount <= 0) //튕기는 횟수가 0이하가 되면
                {
                    isBouncing = false; //isBounCing을 false로 바꾸고
                    isReturning = true; //isReturning을 true로 변경한다.
                }

                //enemyTarget리스트의 Count(수)가 최대 타겟수(targetIndex)보다 이상이라면
                //targetIndex 최대 타겟수를 0으로 한다.
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

    private void SwordSkillDamage(Enemy enemy) //Enemy 클래스를 매개변수로 가져온다.
    {
        enemy.DamageEffect(); //enemy의 데미지 메서드
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration); //enemy의 코루틴
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) //Enemy 컴포넌트를 가지고 있다면
        {
            if (isBouncing && enemyTarget.Count <= 0) //바운싱과 적 타겟이 0이하라면
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                //현재위치에서 반지름 10인 원형을 만들고 원형안에 담기는 콜라이더 객체를 모두 colliders배열에 담는다.

                foreach (var hit in colliders) //colliders 배열에 있는 각 콜라이더에 대해 반복한다.
                {
                    if (hit.GetComponent<Enemy>() != null) //해당 콜라이더가 Enemy 컴포넌트를 가지고 있다면
                        enemyTarget.Add(hit.transform); //해당 콜라이더의 tranform(위치)를 enemyTarget리스트에 추가한다.
                }
            }
        }
    }

    private void StuckInfo(Collider2D collision) //Collider2D인 collision변수를 매개변수로 받아들이는 메서드
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        //pierceAmount가 0보다 크고 콜라이더에 담긴 객체가 Enemy컴포넌트를 가지고 있다면
        {
            pierceAmount--; //pierceAmount를 1 뺀다
            return; //반환한다. (밑의 내용은 출력되지 않음)
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;
        //rigidbody의 물리 시뮬레이터 없음
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        //닿은 콜라이더 객체의 transform을 부모로 여김
        transform.parent = collision.transform;
    }
}

