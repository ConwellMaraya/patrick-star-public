using UnityEngine;

public class Player : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce = 10;
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

    

    



    protected override void Start()
    {
        base.Start();
        jumpctr = maxJumps;
    }

    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        comboTimeWindow -= Time.deltaTime;




        FlipController();
        AnimatorControllers();

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

    private void Movement()
    {
        if(isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }

        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDirection * dashSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded || jumpctr > 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpctr--;
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();

        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    
}
