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
        //player class의 SkillManager에서 Clone_Skill 클래스를 상속받고
        //CreateClone메서드 함수를 불러온다.
        //이때 필요한 Transform은 player의 transform(위치)로 한다.
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

        //땅을 감지한 상태가 아닐때, 벽을 감지한다면 PlayerWallSlide에 들어간다.
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        //stateTimer가 0미만이면 state를 player클래스의 idleState로 들어간다.
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
