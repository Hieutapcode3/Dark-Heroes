using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float      moveSpeed = 5f;
    [SerializeField] float      airMoveSpeed = 10f;
    private float               horizontallInput;
    private bool                facingRight = true;

    [Header("For Jumping")]
    [SerializeField] float      jumpForce = 7.5f;
    [SerializeField] LayerMask  groundLayer;
    [SerializeField] Transform  groundCheckPoint;
    [SerializeField] Vector2    groundCheckSize;
    private bool                grounded;

    [Header("For WallSliding")]
    [SerializeField] float      wallSlideSpeed;
    [SerializeField] LayerMask  wallLayer;
    [SerializeField] Transform  wallCheckPoint;
    [SerializeField] Vector2    wallCheckSize;
    private bool                isTouchingWall;
    private bool                isWallSliding;

    [Header("For WallJumping")]
    [SerializeField] float      wallJumpForce;
    [SerializeField] float      wallJumpDirection = -1f;
    [SerializeField] Vector2    wallJumpAngle;
    
    [Header("For Attack")]
    private int                 currentAttack = 0;
    private float               timeAttack = 0.0f;
    private bool                isAttacking;

    [Header("For Dashing")]
    private float               timeDashing =0.2f;
    private bool                canDash = true;
    [SerializeField] float      dashingPower = 30f;
    private bool                isDashing;
    private float               dashingCooldown = 1f;



    [Header("Other")]
    private Rigidbody2D         rb;
    private Animator            anim;
    private Ghost               ghost;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ghost = GetComponent<Ghost>();
    }
    private void Start()
    {
        wallJumpAngle.Normalize();
    }

    void Update()
    {
        Inputs();
        CheckWorld();
        Attack();

    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
        WallSilde();
        WallJump();
        Fall();
        StartCoroutine(Dash());

    }
    void Inputs()
    {
        horizontallInput = Input.GetAxis("Horizontal");
    }
    private void Movement()
    {
        if (grounded)
        {
            
            if(isAttacking)
                rb.velocity = new Vector2(horizontallInput * 1f, rb.velocity.y);
            else
                rb.velocity = new Vector2(horizontallInput * moveSpeed, rb.velocity.y);

        }else if(!grounded && !isWallSliding && horizontallInput != 0)
        {
            rb.AddForce(new Vector2(airMoveSpeed * horizontallInput, 0));
            if(Mathf.Abs(rb.velocity.x) > moveSpeed)
            {
                rb.velocity = new Vector2(horizontallInput * moveSpeed,rb.velocity.y);
            }
        }
        if (horizontallInput > 0 && !facingRight)
            Flip();
        else if (horizontallInput < 0 && facingRight)
            Flip();

        if (grounded)
            anim.SetBool("run", horizontallInput != 0);
        else
            anim.SetBool("run", false);
    }
    void Flip()
    {
        if (!isWallSliding)
        {
            wallJumpDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

    }
    private void Jump()
    {
        if(Input.GetKey(KeyCode.Space) && grounded )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("jump");
        }
    }
    private void Fall()
    {
        if (rb.velocity.y < 0 && !isTouchingWall)
        {
            anim.SetTrigger("Fall");
        }
    }
    private void CheckWorld()
    {
        grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
        anim.SetBool("Ground", grounded);
    }
    private void WallSilde()
    {
        if (isTouchingWall && !grounded && horizontallInput !=0)
            isWallSliding = true;
        else
            isWallSliding = false;
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            anim.SetBool("WallSlide", true);
        }
        else
            anim.SetBool("WallSlide", false);
    }
    private void WallJump()
    {
        if (Input.GetKey(KeyCode.Space) && isWallSliding )
        {
            rb.AddForce(new Vector2 (wallJumpForce * wallJumpDirection*wallJumpAngle.x,wallJumpForce * wallJumpAngle.y),ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }
    private void Attack()
    {
        timeAttack += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeAttack > 0.55f && grounded) 
        {
            currentAttack++;
            isAttacking = true;
            if (currentAttack > 2)
                currentAttack = 1;
            if (timeAttack > 1.0f)
                currentAttack = 1;
            anim.SetTrigger("Attack" + currentAttack);
            timeAttack = 0.0f;
        }
        if(timeAttack > 0.7f)
            isAttacking = false;
    }
    private IEnumerator Dash()
    {
        if(Input.GetKeyDown(KeyCode.E) && canDash && horizontallInput !=0)
        {
            ghost.makeGhost = true;
            anim.SetTrigger("Dash");
            if (transform.rotation.y > 0)
                rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            else
                rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
            canDash = false;
            isDashing = true;
            float originGra = rb.gravityScale;
            rb.gravityScale = 0f;
            yield return new WaitForSeconds(timeDashing);
            rb.gravityScale = originGra;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
            ghost.makeGhost = false;
        }
    }
    private void Hurt()
    {

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }
}
