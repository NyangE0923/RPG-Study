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
        // Physics2D.OverlapCircleAll �޼���� �־��� �߽����� �������� ������� �ϴ� �� �ȿ� �ִ� ��� Collider2D ��ü�� ã���ϴ�.
        // ���⼭ player.attackCheck.position�� �÷��̾��� attackCheck Transform�� ��ġ�� ��Ÿ���ϴ�.
        // attackCheck�� �÷��̾ ������ �����ϴ� ������ ��Ÿ���µ�, �� ��ġ�� �������� ���� �����մϴ�.
        // player.attackCheckRadius�� ���� �������� �����ϴ� �����Դϴ�.

        // �� �ڵ�� �÷��̾� �ֺ��� �ִ� ��� Collider2D ��ü�� �����Ͽ� colliders �迭�� �����մϴ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        // colliders �迭�� ����� �� collider2D ��ü�� ���� �ݺ��Ѵ�.
        foreach (var hit in colliders)
        {
            // �ش� collider2D ��ü�� Enemy ������Ʈ�� �ִ��� Ȯ���ϰ�, Enemy ������Ʈ�� �ִٸ� �ش� ��ü�� Damage �޼��带 ȣ���Ͽ� �������� ������.
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();

                //canDuplicateClone�� true��� Random.Range�� ȣ���Ͽ�
                //�ּ�0���� �ִ�100���� �������� ���� �������� �ϰ�
                //�̶� ������ ���� 99�̸��̶��(99%�� Ȯ���� ��Ÿ��) �̱��� ��ų�Ŵ�����
                //Ŭ�� ���� �Լ��� �̿��Ͽ� colliders�迭�� ����� collider2D��ü�� ��ġ���� ������(0.5)���� Ŭ���� �����Ѵ�.
                //�̶� FaceClosestTarget �޼��忡�� ��ü�� ��ġ�� ������ �����ʿ� �ִٸ� facingDir�� -1�� �����ϱ� ������
                //�ٽ� �ѹ� Ŭ���� �����ǰ� �ɶ��� 0.5f * -1�� �ǹǷ� �ݴ� ���⿡�� Ŭ���� �����ȴ�.
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
        //���� ����� ���� �����ϴ� ��쿡��
        if (closestEnemy != null)
        {
            //���� ������(+�ϼ��� ������)�� ������ Rotate�޼��带 �̿��� 180�� ȸ����Ų��.
            //facingDir�� -1�� �����Ѵ�.
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
