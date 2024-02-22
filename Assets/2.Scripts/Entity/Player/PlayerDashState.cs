using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player class�� SkillManager���� Clone_Skill Ŭ������ ��ӹް�
        //CreateClone�޼��� �Լ��� �ҷ��´�.
        //�̶� �ʿ��� Transform�� player�� transform(��ġ)�� �Ѵ�.
        player.skill.clone.CreateClone(player.transform);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        //���� ������ ���°� �ƴҶ�, ���� �����Ѵٸ� PlayerWallSlide�� ����.
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        //stateTimer�� 0�̸��̸� state�� playerŬ������ idleState�� ����.
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}