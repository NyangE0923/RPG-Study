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

        //crystalExistTimer�� 0�̸��϶� canExplode�� true��� Animator�� SetTrigger�޼���("Explode")�� ȣ���ϰ�
        //�ƴ϶�� SelfDestroy�޼��带 ȣ���Ѵ�.
        if (crystalExistTimer < 0)
        {
            FinishCrystal();

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
