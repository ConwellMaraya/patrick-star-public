using UnityEngine;

public class Player : Entity
{
    #region Components
    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce = 10F;
    [SerializeField] private bool isMoving;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int jumpctr = 0;


    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;

    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack Info")]
    [SerializeField] private float comboTime;
    [SerializeField] private float comboTimeWindow;
    private bool isAttacking;
    private int comboCounter;

    public Animator playerAnim { get; private set; }
    #endregion Components

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    #endregion States


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        jumpctr = maxJumps;
        playerAnim = GetComponentInChildren<Animator>();
        stateMachine.Initialize(idleState);
        
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckInput();
        

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        comboTimeWindow -= Time.deltaTime;


        FlipController();


    }

    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;



    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        if (newIsGrounded)
        {
            jumpctr = maxJumps;
        }
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttack();
        }

        if (Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartAttack()
    {
        if (!isGrounded)
            return;

        if (comboTimeWindow < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2 (xVelocity, yVelocity);
    }

    private void Jump()
    {
        if (isGrounded || jumpctr > 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpctr--;
        }
    }

    

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();

        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    
}
