using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{

    private EnemySkelly enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkelly enemy) : base(_enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.setZeroVelocity();

        if (triggerCalled)
            enemy.stateMachine.ChangeState(enemy.battleState);
    }
}
