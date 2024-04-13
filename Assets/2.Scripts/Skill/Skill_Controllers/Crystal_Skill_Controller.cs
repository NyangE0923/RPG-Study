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
        //싱글톤 스킬매니저가 가지고 있는 BlackHole_Skill 컴포넌트의 GetBlackholeRadius메소드 값을 선언한다.
        //GetBlackholeRadius : maxSize / 2 (최대크기의 반으로 줄인다.)
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        //객체의 위치에서 지역변수 radius 크기의 반지름을 생성해 그 안에 들어오는 모든 whatIsEnemy에 해당하는 Collider 객체를 colliders배열에 담는다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        //closestTarget은 Transform변수이므로 colliders배열에 담긴 객체들의 transform값을 랜덤으로 가져온다.
        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        //crystalExistTimer가 0미만일때 canExplode가 true라면 Animator의 SetTrigger메서드("Explode")를 호출하고
        //아니라면 SelfDestroy메서드를 호출한다.
        if (crystalExistTimer < 0)
        {
            FinishCrystal();

        }
        //만약 canMove가 true라면 객체의 위치를 Vector2.MoveTowards메서드를 사용하여
        //객체의 위치에서 closestTarget위치로 moveSpeed에 Time.deltaTime을 곱한 속도로 이동한다.
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

            //만약 객체의 위치와 closestTarget의 위치가 1미만이라면 FinishCrystal메서드의 SelfDestroy를 이용해 객체를 파괴한다.
            //이후 객체의 canMove(이동)를 false로 설정한다.
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        //canGrow가 true라면 Vector2.Lerp 메서드를 호출한다.
        //transform.localScale에서 new Vector2(3, 3)의 크기가 될 때까지
        //growSpeed * Time.deltaTime의 속도로 부드럽게 커진다.
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        //현재 위치에서 현재 객체의 Circle Collider 2D의 반지름 안의 모든 충돌체를 반환한다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        //colliders배열에 담긴 모든 객체들에게 반복한다.
        foreach (var hit in colliders)
        {
            //만약 해당 객체가 Enemy컴포넌트를 가지고 있다면
            //Enemy컴포넌트의 Damage메서드를 호출한다.
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
