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

        //��ü�� ��ġ�� MoveToards�� ����ؼ� ��ü�� ��ġ���� Ÿ���� ��ġ���� �����ϰ� speed�� �ӵ��� �̵��Ѵ�.
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        //��ü�� ��ġ�� Ÿ���� ��ġ�� �Ÿ��� 0.1���� �۾����ٸ�
        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            //�ִϸ��̼��� 
            anim.transform.localPosition = new Vector3(0, 0.5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            //Damage�� ��Ÿ���� int�� �Ű������� ������ �ִ� �޼ҵ带 ȣ���Ѵ�.
            //�ִϸ��̼� Ʈ���� Hit�� �����Ѵ�.
            //0.4���� ��ü�� �ı��Ѵ�.
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
