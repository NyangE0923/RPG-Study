using System.Collections;
using System.Collections.Generic;
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
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

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
                hit.GetComponent<Enemy>().Damage();
        }

    }

    private void FaceClosestTarget()
    {
        //���� ��ġ �ֺ�(tranform.position) ����25 �ȿ� �ִ� Collider2D ��ü���� colliders �迭�� �����Ѵ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        //���� ����� ������ �Ÿ��� �����ϴ� ������ �ʱ�ȭ �ϰ�
        float closestDistance = Mathf.Infinity;
        //���� ��ġ �ֺ��� ��� Collider2D ��ü�� ���� �ݺ��ϴ� foreach��
        foreach (var hit in colliders)
        {
            //���� Collider2D ��ü�� Enemy ������Ʈ�� �����ϰ� �ִ��� Ȯ�� (������ Ȯ��)
            //���� Boss��� ������Ʈ�� ���� ����� �ȴٸ� �� ��ų�� ��ȿȭ ��ų �� ���� ������ ����
            if(hit.GetComponent<Enemy>() != null)
            {
                //float distanceToEnemy ���������� ���� ���� ��ġ��
                //colliders �迭�� ��� Enemy��ũ��Ʈ ������Ʈ�� ������ �ִ� Collider2D ��ü���� �Ÿ��� ����Ѵ�.
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                //����� ���� �Ÿ��� closestDistance(������ ����� ���� ����� ������ �Ÿ�)�������� ª����
                if(distanceToEnemy < closestDistance)
                {
                    //���� ���� ���� ����� ������ �����ϰ�,
                    //closestDistance���� distanceToEnemy�� �����Ѵ�.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        //���� ����� ���� �����ϴ� ��쿡��
        if(closestEnemy != null)
        {
            //������ġ�� ���� ��ġ���� ������(+�ϼ��� ������)�� ������ Rotate�޼��带 �̿��� 180�� ȸ����Ų��.
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
