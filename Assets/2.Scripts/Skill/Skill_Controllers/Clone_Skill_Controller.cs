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
                hit.GetComponent<Enemy>().Damage();
        }

    }

    private void FaceClosestTarget()
    {
        //현재 위치 주변(tranform.position) 범위25 안에 있는 Collider2D 객체들을 colliders 배열에 저장한다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        //가장 가까운 적과의 거리를 저장하는 변수를 초기화 하고
        float closestDistance = Mathf.Infinity;
        //현재 위치 주변의 모든 Collider2D 객체에 대해 반복하는 foreach문
        foreach (var hit in colliders)
        {
            //현재 Collider2D 객체가 Enemy 컴포넌트를 포함하고 있는지 확인 (적인지 확인)
            //만약 Boss라는 컴포넌트를 따로 만들게 된다면 이 스킬을 무효화 시킬 수 있을 것으로 보임
            if(hit.GetComponent<Enemy>() != null)
            {
                //float distanceToEnemy 지역변수를 만들어서 현재 위치와
                //colliders 배열에 담긴 Enemy스크립트 컴포넌트를 가지고 있는 Collider2D 객체와의 거리를 계산한다.
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                //계산한 적의 거리가 closestDistance(이전에 저장된 가장 가까운 적과의 거리)변수보다 짧으면
                if(distanceToEnemy < closestDistance)
                {
                    //현재 적을 가장 가까운 적으로 설정하고,
                    //closestDistance값에 distanceToEnemy를 갱신한다.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        //가장 가까운 적이 존재하는 경우에는
        if(closestEnemy != null)
        {
            //현재위치가 적의 위치보다 오른쪽(+일수록 오른쪽)에 있으면 Rotate메서드를 이용해 180도 회전시킨다.
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
