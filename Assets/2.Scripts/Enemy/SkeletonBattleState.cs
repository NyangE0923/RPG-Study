using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        //적이 플레이어를 감지했다면 
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            //플레이어를 감지한 거리가 적의 공격 반응 거리보다 짧을 때
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        //만약 플레이어의 x위치가 적의 x위치보다 초과일경우 적의 방향은 오른쪽을 향한다.
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        //만약 플레이어의 x위치가 적의 x위치보다 미만일경우 적의 방향은 왼쪽을 향한다.
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
        //적의 이동속도 * 바라보는 방향으로 이동, y는 그대로 적의 y값
        enemy.SetVelocity(enemy.moveSpeed * 2f * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
