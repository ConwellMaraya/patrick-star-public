using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkelly enemy;
    private Transform player;
    private int moveDir;
    
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkelly enemy) : base(_enemyBase, stateMachine, animBoolName)
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
        

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            
            
            if (enemy.isPlayerDetected().distance < enemy.attackDistance && canAttack())
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
            }
        }

        else
        {
            
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(5 * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCD)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    
}
