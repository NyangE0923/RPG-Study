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
            // 해당 collider2D 객체에 Player 컴포넌트가 있는지 확인하고, Player 컴포넌트가 있다면 해당 객체의 Damage 메서드를 호출하여 데미지를 입힌다.
            if (hit.GetComponent<Player>() != null) //null이 아니라면
                hit.GetComponent<Player>().Damage(); //player 객체의 Damage메서드 호출
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
