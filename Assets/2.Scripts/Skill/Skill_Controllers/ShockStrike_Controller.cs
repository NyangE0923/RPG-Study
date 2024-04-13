using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Update()
    {
        if (!targetStats)
            return;
        if (triggered)
            return;

        //객체의 위치를 MoveToards를 사용해서 객체의 위치에서 타겟의 위치까지 일정하게 speed의 속도로 이동한다.
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        //객체의 위치와 타겟의 위치의 거리가 0.1보다 작아진다면
        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            //애니메이션의 
            anim.transform.localPosition = new Vector3(0, 0.5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            //Damage를 나타내는 int형 매개변수를 가지고 있는 메소드를 호출한다.
            //애니메이션 트리거 Hit를 실행한다.
            //0.4초후 객체를 파괴한다.
            anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(1);
        Destroy(gameObject, .4f);
    }
}
