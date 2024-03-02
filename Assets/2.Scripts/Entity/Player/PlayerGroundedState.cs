using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    //������
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
        // ���� �����̽��� Ű�� ������ ��(Player�� ���� ���� ��)
        // PlayerJumpState�� ���¸� �����մϴ�.
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);


        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.counter.CanUseSkill())
        {
            stateMachine.ChangeState(player.counterAttack);
        }
    }

    private bool HasNoSword()
    {
        //�÷��̾� ��ü�� sword�� ���ٸ�
        if (!player.sword)
        {
            //true�� ��ȯ�Ѵ�. (���� ������ ������ �ǹ�)
            return true;
        }

        //�÷��̾� ��ü�� sword������Ʈ�� �ִ� Sword_Skill_Controller�� �ִ� ReturnSword�޼��带 ȣ���Ѵ�.
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        //false�� ��ȯ�Ѵ�. (���� ������ ���� ������ �ǹ��Ѵ�.)
        return false;
    }
}
