using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    public void ChooseRandomEnemy()
    {
        //�̱��� ��ų�Ŵ����� ������ �ִ� BlackHole_Skill ������Ʈ�� GetBlackholeRadius�޼ҵ� ���� �����Ѵ�.
        //GetBlackholeRadius : maxSize / 2 (�ִ�ũ���� ������ ���δ�.)
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        //��ü�� ��ġ���� �������� radius ũ���� �������� ������ �� �ȿ� ������ ��� whatIsEnemy�� �ش��ϴ� Collider ��ü�� colliders�迭�� ��´�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        //closestTarget�� Transform�����̹Ƿ� colliders�迭�� ��� ��ü���� transform���� �������� �����´�.
        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        //crystalExistTimer�� 0�̸��϶� canExplode�� true��� Animator�� SetTrigger�޼���("Explode")�� ȣ���ϰ�
        //�ƴ϶�� SelfDestroy�޼��带 ȣ���Ѵ�.
        if (crystalExistTimer < 0)
        {
            FinishCrystal();

        }
        //���� canMove�� true��� ��ü�� ��ġ�� Vector2.MoveTowards�޼��带 ����Ͽ�
        //��ü�� ��ġ���� closestTarget��ġ�� moveSpeed�� Time.deltaTime�� ���� �ӵ��� �̵��Ѵ�.
        if (canMove)
        {
            if(closestTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                return;
            }

            //���� ��ü�� ��ġ�� closestTarget�� ��ġ�� 1�̸��̶�� FinishCrystal�޼����� SelfDestroy�� �̿��� ��ü�� �ı��Ѵ�.
            //���� ��ü�� canMove(�̵�)�� false�� �����Ѵ�.
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        //canGrow�� true��� Vector2.Lerp �޼��带 ȣ���Ѵ�.
        //transform.localScale���� new Vector2(3, 3)�� ũ�Ⱑ �� ������
        //growSpeed * Time.deltaTime�� �ӵ��� �ε巴�� Ŀ����.
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        //���� ��ġ���� ���� ��ü�� Circle Collider 2D�� ������ ���� ��� �浹ü�� ��ȯ�Ѵ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        //colliders�迭�� ��� ��� ��ü�鿡�� �ݺ��Ѵ�.
        foreach (var hit in colliders)
        {
            //���� �ش� ��ü�� Enemy������Ʈ�� ������ �ִٸ�
            //Enemy������Ʈ�� Damage�޼��带 ȣ���Ѵ�.
            if (hit.GetComponent<Enemy>() != null)
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
