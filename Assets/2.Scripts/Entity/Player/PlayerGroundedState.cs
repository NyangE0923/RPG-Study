using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    //생성자
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackHole);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSword);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        // 만약 스페이스바 키가 눌렸을 때(Player가 땅에 있을 때)
        // PlayerJumpState로 상태를 변경합니다.
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);


        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.counter.CanUseSkill())
        {
            stateMachine.ChangeState(player.counterAttack);
        }
    }

    private bool HasNoSword()
    {
        //플레이어 객체에 sword가 없다면
        if (!player.sword)
        {
            //true를 반환한다. (검을 가지고 있음을 의미)
            return true;
        }

        //플레이어 객체에 sword컴포넌트에 있는 Sword_Skill_Controller에 있는 ReturnSword메서드를 호출한다.
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        //false를 반환한다. (검을 가지고 있지 않음을 의미한다.)
        return false;
    }
}
