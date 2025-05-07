using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isWallDetected { get; private set; }
    public bool newIsGrounded{ get; private set; }
    public float xInput;
    public int facingDirection = 1;
    public bool facingRight = true;
    public EntityFX FX { get; private set; }

    [Header("Collision Info")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDir;
    protected bool isKBed;
    [SerializeField] protected float KBTimer;

    protected virtual void Awake()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        FX = GetComponent<EntityFX>();

        if (wallCheck == null)
            wallCheck = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        CollisionChecks();

    }

    public virtual void Damage()
    {
        FX.StartCoroutine("FlashFX");
        StartCoroutine("HitKB");
        Debug.Log(gameObject.name + " Was Hit");
    }

    protected virtual IEnumerator HitKB()
    {
        isKBed = true;

        rb.velocity = new Vector2(knockbackDir.x * -facingDirection, knockbackDir.y);

        yield return new WaitForSeconds(KBTimer);

        isKBed = false;
    }

    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    }

    public virtual void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();

        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    protected virtual void CollisionChecks()
    {
        newIsGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position,Vector2.right, wallCheckDistance * facingDirection, whatIsGround);

        if (newIsGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void setZeroVelocity()
    {
        if (isKBed)
            return;

        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKBed)
            return;

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController();
    }
}
