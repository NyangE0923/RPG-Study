using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
        //Entity를 상속받은 enemy의 SetVelocity 사용
        //바라보는 방향으로 2만큼 이동하고, y값은 그대로 현재 오브젝트의 값을 유지한다.
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

        //만약 벽을 감지했거나, 땅을 감지하지 못했다면
        //방향을 전환한다. 그리고 idleState 상태로 이동한다.
        //방향을 바꾸고 잠시 멈추고 이동하도록 구현됨
        if(enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
