using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private float Move;

    public float speed;
    public float jump;
    public float run;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxisRaw("Horizontal");

        //rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded()) 
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
           rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        }
    }


    public bool isGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
