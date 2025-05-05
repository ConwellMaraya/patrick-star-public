using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    private string animBoolName;
    protected float xInput;

    public PlayerState (Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.playerAnim.SetBool(animBoolName,true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        player.playerAnim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.playerAnim.SetBool(animBoolName, false);
    }
}
