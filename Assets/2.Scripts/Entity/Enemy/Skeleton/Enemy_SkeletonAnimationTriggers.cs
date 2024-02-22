using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        //�־��� �߽����� �������� ������� �ϴ� ���ȿ� �ִ� ��ü�� ��� ã�Ƽ� colliders�� ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
       
        // colliders �迭�� ����� �� collider2D ��ü�� ���� �ݺ��Ѵ�.
        foreach (var hit in colliders)
        {
            // �ش� collider2D ��ü�� Player ������Ʈ�� �ִ��� Ȯ���ϰ�, Player ������Ʈ�� �ִٸ� �ش� ��ü�� Damage �޼��带 ȣ���Ͽ� �������� ������.
            if (hit.GetComponent<Player>() != null) //null�� �ƴ϶��
                hit.GetComponent<Player>().Damage(); //player ��ü�� Damage�޼��� ȣ��
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
