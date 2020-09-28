using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private bool _isFacingRight = true;
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

        if ((input > 0 && !_isFacingRight) || input < 0 && _isFacingRight)
        {
            Flip();
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        _isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);
        _wallSliding = _isTouchingFront && !_isGrounded && input != 0;
        
        rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (_wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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