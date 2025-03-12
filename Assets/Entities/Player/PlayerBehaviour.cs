using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerBehaviour : MonoBehaviour
{
    public int jumplimit = 1;
    public float xMult = 1;
    public float yMult = 1;
    public Rigidbody2D rb;
    private Animator MoveBitch;
    private bool leftright = true;
    private SpriteRenderer sprite;
    private bool isMove;

    [SerializeField] private float xInput = 0;
    [SerializeField] private int jumpctr;
    [SerializeField] private float yInput = 0;

    void Start()
    {
        Debug.Log("Ur mom");
        MoveBitch = GetComponentInChildren<Animator>();
        sprite = (SpriteRenderer) MoveBitch.GetComponent("SpriteRenderer");
        jumpctr = jumplimit;
    }

    // Update is called once per frame
    void Update()
    {
        InputCall();
        AnimationCall();
        MovementCall();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Platform")
        {
            //Checks if the player lands on the top half of a platform
            //will update with more tags but for now ground works
            var contact = collision.GetContact(0);
            var point  = contact.point;
            var center = contact.collider.bounds.center;

            if (point.y > center.y)
            {
                Debug.Log("Hit on top half");
                jumpctr = jumplimit;
            }
        }
    }

    private void InputCall()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        isMove = (xInput != 0) ? true : false;
    }

    private void MovementCall()
    {
        rb.velocity = new Vector2(xInput * xMult, rb.velocity.y);
        
        /*
        How Jumping works
        Player gets x amount of jumps, once they use them up, they must touch the top half
        of a platform to regain all their jumps. To modify the amount of jumps a player gets back
        change the variable jump limit
         
         
        */
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpctr > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, yMult);
                jumpctr--;
            }
            
        }

        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            Debug.Log("DOWN");
            rb.velocity = new Vector2(rb.velocity.x, yInput * (Mathf.Log(yMult, 2)));
        }
    }

    private void AnimationCall()
    {
        if (!leftright && rb.velocity.x > 0)
        {
            Flip();
        }

        else if (leftright && rb.velocity.x < 0)
        {
            Flip();
        }

        MoveBitch.SetBool("HorizontalMove", isMove);
    }

    private void Flip()
    {
        leftright = !leftright;
        transform.Rotate(0, 180, 0);
    }
}
