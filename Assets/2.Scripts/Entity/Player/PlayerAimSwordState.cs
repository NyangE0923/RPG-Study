using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        //Vector2 mousePosition이라는 변수를 선언한다.
        //이때 변수의 값은 Camera클래스의 ScreenToWorldPoint메서드를 이용하여 마우스의 현재 위치를 값으로 한다.
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //만약 플레이어의 x위치가 마우스의 x위치와 플레이어 방향이 오른쪽일 경우 플레이어의 방향을 전환한다.
        if (player.transform.position.x > mousePostion.x && player.facingDir == 1)
            player.Flip();
        //만약 플레이어의 x위치가 마우스의 x위치와 플레이어 방향이 왼쪽일 경우 플레이어의 방향을 전환한다.
        else if(player.transform.position.x < mousePostion.x && player.facingDir == -1)
            player.Flip();
    }
}
