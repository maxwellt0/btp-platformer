using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private bool _isFacingRight = false;
    private bool _isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    private bool _isTouchingFront;
    public Transform frontCheck;
    private bool _wallSliding;
    public float wallSlidingSpeed;

    private bool _wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);

        if (input > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (input < 0 && _isFacingRight) 
        {
            Flip();
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        _isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (_isTouchingFront && _isGrounded == false && input != 0)
        {
            _wallSliding = true;
        }
        else
        {
            _wallSliding = false;
        }

        if (_wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && _wallSliding)
        {
            _wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (_wallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -input, yWallForce);
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = !_isFacingRight;
    }

    private void SetWallJumpingToFalse()
    {
        _wallJumping = false;
    }
}