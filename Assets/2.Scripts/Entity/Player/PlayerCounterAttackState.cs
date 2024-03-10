using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // Physics2D.OverlapCircleAll �޼���� �־��� �߽����� �������� ������� �ϴ� �� �ȿ� �ִ� ��� Collider2D ��ü�� ã���ϴ�.
        // ���⼭ player.attackCheck.position�� �÷��̾��� attackCheck Transform�� ��ġ�� ��Ÿ���ϴ�.
        // attackCheck�� �÷��̾ ������ �����ϴ� ������ ��Ÿ���µ�, �� ��ġ�� �������� ���� �����մϴ�.
        // player.attackCheckRadius�� ���� �������� �����ϴ� �����Դϴ�.

        // �� �ڵ�� �÷��̾� �ֺ��� �ִ� ��� Collider2D ��ü�� �����Ͽ� colliders �迭�� �����մϴ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // colliders �迭�� ����� �� collider2D ��ü�� ���� �ݺ��Ѵ�.
        foreach (var hit in colliders)
        {
            // �ش� collider2D ��ü�� Enemy ������Ʈ�� �ִ��� Ȯ���ϰ�, ���� �ִٸ� ������ų �� �ִ��� Ȯ���Ѵ�.
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                    //canCreateclone�� true��� false�� �����ϰ�
                    //CreatecloneOnCounterAttack�޼��带 ȣ���Ѵ�.
                    //���ÿ� �ϳ� �̻��� Ŭ���� �����Ǵ� ���� ���� ���� �ڵ�
                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);

                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
