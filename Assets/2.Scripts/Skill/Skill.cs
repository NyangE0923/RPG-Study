using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //do some skill specific things
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        //_checkTransform��ġ���� ����25 �ȿ� �ִ� Collider2D ��ü���� colliders �迭�� �����Ѵ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        //���� ����� ������ �Ÿ��� �����ϴ� ������ �ʱ�ȭ �ϰ�
        //_checkTransform�� ����ִ� Transform ���� ������ �����Ѵ�.
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        //���� ��ġ �ֺ��� ��� Collider2D ��ü�� ���� �ݺ��ϴ� foreach��
        foreach (var hit in colliders)
        {
            //���� Collider2D ��ü�� Enemy ������Ʈ�� �����ϰ� �ִ��� Ȯ�� (������ Ȯ��)
            //���� Boss��� ������Ʈ�� ���� ����� �ȴٸ� �� ��ų�� ��ȿȭ ��ų �� ���� ������ ����
            if (hit.GetComponent<Enemy>() != null)
            {
                //float distanceToEnemy ���������� ���� ���� ��ġ��
                //colliders �迭�� ��� Enemy��ũ��Ʈ ������Ʈ�� ������ �ִ� Collider2D ��ü���� �Ÿ��� ����Ѵ�.
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                //����� ���� �Ÿ��� closestDistance(������ ����� ���� ����� ������ �Ÿ�)�������� ª����
                if (distanceToEnemy < closestDistance)
                {
                    //���� ���� ���� ����� ������ �����ϰ�,
                    //closestDistance���� distanceToEnemy�� �����Ѵ�.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        //closestEnemy�� ��ȯ�Ѵ�.
        return closestEnemy;
    }
}
