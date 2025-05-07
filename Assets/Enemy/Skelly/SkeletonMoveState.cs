using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{

    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkelly _enemy) : base(enemyBase, stateMachine, animBoolName, _enemy)
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

    }
}
