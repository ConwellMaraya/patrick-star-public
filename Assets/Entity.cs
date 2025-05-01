using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected bool isGrounded;
    protected bool isWallDetected;
    protected bool newIsGrounded;
    protected float xInput;
    protected int facingDirection = 1;
    protected bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();

        if (wallCheck == null)
            wallCheck = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();

    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
