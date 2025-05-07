using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected Rigidbody2D rb;
    protected bool triggerCalled;
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        this.rb = enemyBase.rb;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
