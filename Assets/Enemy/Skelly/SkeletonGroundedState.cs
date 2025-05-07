using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{

    protected EnemySkelly enemy;

    protected Transform player;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkelly enemy) : base(_enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy; 
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position,player.position) < 8)
            enemy.stateMachine.ChangeState(enemy.battleState);
    }
}
