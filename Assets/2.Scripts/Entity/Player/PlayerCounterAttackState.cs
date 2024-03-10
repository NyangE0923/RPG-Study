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

        // Physics2D.OverlapCircleAll 메서드는 주어진 중심점과 반지름을 기반으로 하는 원 안에 있는 모든 Collider2D 객체를 찾습니다.
        // 여기서 player.attackCheck.position은 플레이어의 attackCheck Transform의 위치를 나타냅니다.
        // attackCheck는 플레이어가 공격을 수행하는 지점을 나타내는데, 이 위치를 기준으로 원을 생성합니다.
        // player.attackCheckRadius는 원의 반지름을 결정하는 변수입니다.

        // 이 코드는 플레이어 주변에 있는 모든 Collider2D 객체를 검출하여 colliders 배열에 저장합니다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // colliders 배열에 저장된 각 collider2D 객체에 대해 반복한다.
        foreach (var hit in colliders)
        {
            // 해당 collider2D 객체에 Enemy 컴포넌트가 있는지 확인하고, 적이 있다면 기절시킬 수 있는지 확인한다.
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                    //canCreateclone이 true라면 false로 변경하고
                    //CreatecloneOnCounterAttack메서드를 호출한다.
                    //동시에 하나 이상의 클론이 생성되는 것을 막기 위한 코드
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
