using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //부모 오브젝트에게서 상속받기 위한 InParent
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // Physics2D.OverlapCircleAll 메서드는 주어진 중심점과 반지름을 기반으로 하는 원 안에 있는 모든 Collider2D 객체를 찾습니다.
        // 여기서 player.attackCheck.position은 플레이어의 attackCheck Transform의 위치를 나타냅니다.
        // attackCheck는 플레이어가 공격을 수행하는 지점을 나타내는데, 이 위치를 기준으로 원을 생성합니다.
        // player.attackCheckRadius는 원의 반지름을 결정하는 변수입니다.

        // 이 코드는 플레이어 주변에 있는 모든 Collider2D 객체를 검출하여 colliders 배열에 저장합니다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // colliders 배열에 저장된 각 collider2D 객체에 대해 반복한다.
        foreach (var hit in colliders)
        {
            // 해당 collider2D 객체에 Enemy 컴포넌트가 있는지 확인하고, Enemy 컴포넌트가 있다면 해당 객체의 Damage 메서드를 호출하여 데미지를 입힌다.
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }

    }
}
