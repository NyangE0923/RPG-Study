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
        //_checkTransform위치에서 범위25 안에 있는 Collider2D 객체들을 colliders 배열에 저장한다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        //가장 가까운 적과의 거리를 저장하는 변수를 초기화 하고
        //_checkTransform를 비어있는 Transform 지역 변수로 선언한다.
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        //현재 위치 주변의 모든 Collider2D 객체에 대해 반복하는 foreach문
        foreach (var hit in colliders)
        {
            //현재 Collider2D 객체가 Enemy 컴포넌트를 포함하고 있는지 확인 (적인지 확인)
            //만약 Boss라는 컴포넌트를 따로 만들게 된다면 이 스킬을 무효화 시킬 수 있을 것으로 보임
            if (hit.GetComponent<Enemy>() != null)
            {
                //float distanceToEnemy 지역변수를 만들어서 현재 위치와
                //colliders 배열에 담긴 Enemy스크립트 컴포넌트를 가지고 있는 Collider2D 객체와의 거리를 계산한다.
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                //계산한 적의 거리가 closestDistance(이전에 저장된 가장 가까운 적과의 거리)변수보다 짧으면
                if (distanceToEnemy < closestDistance)
                {
                    //현재 적을 가장 가까운 적으로 설정하고,
                    //closestDistance값에 distanceToEnemy를 갱신한다.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        //closestEnemy를 반환한다.
        return closestEnemy;
    }
}
