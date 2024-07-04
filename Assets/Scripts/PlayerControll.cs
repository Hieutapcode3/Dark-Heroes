using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour,ITakeDamage
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

    [Header("For Falling")]
    private float               fallStartPosition;
    private bool                isFalling;

    [Header("For WallJumping")]
    [SerializeField] float      wallJumpForce;
    [SerializeField] float      wallJumpDirection = -1f;
    [SerializeField] Vector2    wallJumpAngle;
    
    [Header("For Attack")]
    private int                 currentAttack = 0;
    private float               timeAttack = 0.0f;
    private bool                isAttacking;
    private int                 attackDamage = 10;
    public Transform            attackPoint;
    public float                attackRange = 0.5f;
    public LayerMask            enemyLayer;

    [Header("For Dashing")]
    private float               timeDashing =0.2f;
    private bool                canDash = true;
    [SerializeField] float      dashingPower = 30f;
    private float               dashingCooldown = 1f;

    [Header("Other")]
    private Rigidbody2D         rb;
    private Animator            anim;
    private Ghost               ghost;
    public int                  health = 100;
    [SerializeField] Slider     slide;
    
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
        Fall();
    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
        WallSlide();
        WallJump();
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
                rb.velocity = new Vector2(horizontallInput * moveSpeed, 0);

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
        if (rb.velocity.y < 0 && !isWallSliding && !isFalling)
        {
            anim.SetTrigger("Fall");
            fallStartPosition = transform.position.y;
            isFalling = true;
        }
        else if (rb.velocity.y >= 0 && isFalling && grounded)
        {
            CheckFallDamage();
            isFalling = false;
        }
    }
    private void CheckFallDamage()
    {
        if (isWallSliding || !isFalling)
        {
            return; 
        }
        float fallDistance = fallStartPosition - transform.position.y;

        if (fallDistance >= 15f)
        {
            TakeDamage(100);
            Debug.Log("-100hp");
        }
        else if (fallDistance >= 10f)
        {
            TakeDamage(50);
            Debug.Log("-50hp");

        }else if (fallDistance >= 5f)
        {
            TakeDamage(30);
            Debug.Log("-30hp");

        }
    }
    private void CheckWorld()
    {
        grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
        anim.SetBool("Ground", grounded);
    }
    private void WallSlide()
    {
        if (isTouchingWall && !grounded && horizontallInput != 0)
            isWallSliding = true;
        else
            isWallSliding = false;
        if (isWallSliding)
        {
            isFalling = false;
            rb.velocity = new Vector2(0, -wallSlideSpeed);
            anim.SetBool("WallSlide", true);
            fallStartPosition = 0;
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
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
            foreach (Collider2D enemyhit in hitEnemies)
            {
                enemyhit.GetComponent<EnemyController>().TakeDamage(attackDamage);
            }
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
            float originGra = rb.gravityScale;
            rb.gravityScale = 0f;
            yield return new WaitForSeconds(timeDashing);
            rb.gravityScale = originGra;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
            ghost.makeGhost = false;
        }
    }
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hurt");
        health -= damage;
        slide.value = health;
        if(health <= 0)
        {
            StartCoroutine(Death());
        }
    }
    private IEnumerator Death()
    {
        anim.SetTrigger("Death");
        anim.SetBool("run", false);
        this.enabled = false;
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameoverScenes");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            TakeDamage(10);
        }
    }

}
