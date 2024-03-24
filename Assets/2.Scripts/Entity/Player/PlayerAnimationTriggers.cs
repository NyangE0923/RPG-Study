using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //�θ� ������Ʈ���Լ� ��ӹޱ� ���� InParent
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // Physics2D.OverlapCircleAll �޼���� �־��� �߽����� �������� ������� �ϴ� �� �ȿ� �ִ� ��� Collider2D ��ü�� ã���ϴ�.
        // ���⼭ player.attackCheck.position�� �÷��̾��� attackCheck Transform�� ��ġ�� ��Ÿ���ϴ�.
        // attackCheck�� �÷��̾ ������ �����ϴ� ������ ��Ÿ���µ�, �� ��ġ�� �������� ���� �����մϴ�.
        // player.attackCheckRadius�� ���� �������� �����ϴ� �����Դϴ�.

        // �� �ڵ�� �÷��̾� �ֺ��� �ִ� ��� Collider2D ��ü�� �����Ͽ� colliders �迭�� �����մϴ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // colliders �迭�� ����� �� collider2D ��ü�� ���� �ݺ��Ѵ�.
        foreach (var hit in colliders)
        {
            // �ش� collider2D ��ü�� Enemy ������Ʈ�� �ִ��� Ȯ���Ѵ�.
            if (hit.GetComponent<Enemy>() != null)
            {
                //�׸��� �ش� Enemy��ü�� EnemyStats ������Ʈ�� �����ͼ�
                //EnemyStats �������� target�� ���� �Ѱ��ش�.
                //�̷��� �ϴ� ������ _target�� �������� �� �� �ְ� �ȴ�.
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target);
            }
        }

    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
