using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
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
                hit.GetComponent<Enemy>().Damage();
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
