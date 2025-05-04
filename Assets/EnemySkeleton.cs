using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [Header("Entity Detection")]
    [SerializeField] private float playerDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private bool isAttacking;


    private RaycastHit2D playerDetection;

    protected override void Start()
    {
        base.Start();  
    }

    protected override void Update()
    {
        base.Update();

        if (playerDetection)
        {
            if (playerDetection.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 1.5f * facingDirection, rb.velocity.y);

                Debug.Log("Found you");
                isAttacking = false;
            }

            else
            {
                Debug.Log("ATTACK " + playerDetection.collider.gameObject.name);
                isAttacking = true;
            }
        }
        if (!isGrounded || isWallDetected)
            Flip();
        Movement();
    }

    private void Movement()
    {
        if (!isAttacking)
            rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        playerDetection = Physics2D.Raycast(transform.position, Vector2.right, playerDistance * facingDirection, whatIsPlayer);

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + playerDistance * facingDirection, transform.position.y));
    }
}
