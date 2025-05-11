using System.Collections;
using UnityEngine;

public class Player : Entity
{
    #region Components
    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce = 10F;
    [SerializeField] public bool isMoving;
    [SerializeField] public int maxJumps = 2;
    [SerializeField] public int jumpctr = 0;

    [Header("Dash Info")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashDir {get; private set;}

    [SerializeField] public float dashCooldown;
    [SerializeField] public float dashCooldownTimer;

    [Header("Attack Info")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    

    public bool isBusy { get; private set; }    
    #endregion Components

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }  
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; } // TODO: Implement this state
    #endregion States


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        jumpctr = maxJumps;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDashInput();



        


    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }    


    public void AnimationTrigger() => stateMachine.currentState.finishAnim();
   

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        if (newIsGrounded)
        {
            jumpctr = maxJumps;
        }
    }

    private void CheckDashInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        dashCooldownTimer -= Time.deltaTime;

        if (!isWallDetected)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer < 0)
            {
                dashCooldownTimer = dashCooldown;
                dashDir = xInput;

                if (dashDir == 0)
                    dashDir = facingDirection;

                stateMachine.ChangeState(dashState);
            }
        }
    }

    
    


    

    

    
}
