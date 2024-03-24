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
        //주어진 중심점과 반지름을 기반으로 하는 원안에 있는 객체를 모두 찾아서 colliders에 저장
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
       
        // colliders 배열에 저장된 각 collider2D 객체에 대해 반복한다.
        foreach (var hit in colliders)
        {
            // 해당 collider2D 객체가 Player 컴포넌트를 가지고 있는지 확인하고, Player 컴포넌트가 있다면
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
