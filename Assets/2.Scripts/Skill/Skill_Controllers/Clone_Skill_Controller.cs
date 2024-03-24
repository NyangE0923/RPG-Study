using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sr.color = Color.yellow;
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 0.92f, 0.016f, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        // Physics2D.OverlapCircleAll 메서드는 주어진 중심점과 반지름을 기반으로 하는 원 안에 있는 모든 Collider2D 객체를 찾습니다.
        // 여기서 player.attackCheck.position은 플레이어의 attackCheck Transform의 위치를 나타냅니다.
        // attackCheck는 플레이어가 공격을 수행하는 지점을 나타내는데, 이 위치를 기준으로 원을 생성합니다.
        // player.attackCheckRadius는 원의 반지름을 결정하는 변수입니다.

        // 이 코드는 플레이어 주변에 있는 모든 Collider2D 객체를 검출하여 colliders 배열에 저장합니다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        // colliders 배열에 저장된 각 collider2D 객체에 대해 반복한다.
        foreach (var hit in colliders)
        {
            // 해당 collider2D 객체에 Enemy 컴포넌트가 있는지 확인하고, Enemy 컴포넌트가 있다면 해당 객체의 Damage 메서드를 호출하여 데미지를 입힌다.
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();

                //canDuplicateClone이 true라면 Random.Range를 호출하여
                //최소0부터 최대100까지 랜덤으로 수가 나오도록 하고
                //이때 나오는 수가 99미만이라면(99%의 확률을 나타냄) 싱글톤 스킬매니저의
                //클론 생성 함수를 이용하여 colliders배열에 저장된 collider2D객체의 위치에서 오른쪽(0.5)에서 클론을 생성한다.
                //이때 FaceClosestTarget 메서드에서 객체의 위치가 적보다 오른쪽에 있다면 facingDir를 -1로 설정하기 때문에
                //다시 한번 클론이 생성되게 될때는 0.5f * -1이 되므로 반대 방향에서 클론이 생성된다.
                if (canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        //가장 가까운 적이 존재하는 경우에는
        if (closestEnemy != null)
        {
            //적의 오른쪽(+일수록 오른쪽)에 있으면 Rotate메서드를 이용해 180도 회전시킨다.
            //facingDir를 -1로 설정한다.
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
