using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;
    private float lastTimeAttacked;
    //공격 유지시간
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;

        //콤보 카운터가 2 이상이거나 마지막 공격으로부터 comboWindow 만큼 지났다면 콤보 카운터를 0으로 초기화한다.
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        //Player의 애니메이션에서 ComboCounter를 comboCounter에 맞게 SetInteger 메서드를 이용하여 애니메이션에 전달한다.
        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose attack direction

        //이동하면 attackDir은 xInput의 값을 가지며 이 값은 캐릭터의 방향과 같다.
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion

        // player 클래스의 SetVelocity 메서드를 호출하여 플레이어의 속도를 설정한다.
        // 플레이어의 공격 움직임은 player.attackMovement 배열에서 comboCounter(0,1,2)에 해당하는 값으로 결정된다.
        // player.facingDir은 플레이어가 바라보는 방향을 나타내며, 왼쪽으로 바라볼 때는 -1, 오른쪽으로 바라볼 때는 1
        // rb.velocity.y는 플레이어의 수직 방향(위쪽 또는 아래쪽)으로의 속도를 나타내는 변수이므로 수직 방향의 움직임은 이전 속도를 유지합니다.
        //즉 공격할때 바라보는 방향으로 일정거리 이동하는 코드!
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        //코루틴을 시작한다.
        player.StartCoroutine("BusyFor", 0.15f);

        //콤보 카운터를 1개 쌓는다.
        comboCounter++;
        //마지막 공격한 시간을 기록한다.
        lastTimeAttacked = Time.time;
        Debug.Log(lastTimeAttacked);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        //공격이 끝났다면 PlayerIdleState로 이동
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
