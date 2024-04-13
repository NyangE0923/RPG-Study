using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;

    private float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        //PlayerState.SkillManager.Blackhole_SkillŬ������ �ִ� SkillCompleted �޼��忡 true�� ��ȯ�Ǹ�
        //PlayerStateMachine�� ChangeState�� player.airState�� ȣ���Ѵ�. 
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
