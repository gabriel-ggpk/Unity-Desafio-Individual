using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.5f;
    private float dashingCooldown = 0.5f;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(2f, 16f);
    private float screenWipeCooldown = 20f;
    private bool canScreenWipe = true;
        
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private LogicManager logic;
    [SerializeField] private GameObject screenWipe;
    
    public Animator animator;
    void Update()
    {
        if(isDashing)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontal*speed));
        if (!IsGrounded())
        {
            if(rb.velocity.y < 0)
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.R) && canScreenWipe)
        {
           StartCoroutine (ScreenWipe());
        }
        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing || isWallJumping)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && horizontal != 0) {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private IEnumerator Dash()
    {

        
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(horizontal, vertical).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        rb.velocity = rb.velocity * 0.1f ;
        animator.SetBool("isDashing", false);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private void ActivateBulletTime()
    {
        Time.timeScale = 0.5f;  // Slow down time to half speed
        Time.fixedDeltaTime = Time.timeScale * 0.02f;  // Adjust the physics update rate
    }

    private void DeactivateBulletTime()
    {
        Time.timeScale = 1.0f;  // Reset time scale to normal speed
        Time.fixedDeltaTime = 0.02f;  // Reset the physics update rate to default
    }

    private IEnumerator BulletTimeCoroutine(float duration)
    {
        ActivateBulletTime();
        yield return new WaitForSecondsRealtime(duration);  // Use WaitForSecondsRealtime to ignore time scaling
        DeactivateBulletTime();
    }

    public void TakeDamage()
    {
        if (!isDashing)
        {

        logic.gameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyLogic enemy = collision.gameObject.GetComponent<EnemyLogic>();
        if (enemy != null)
        {
            if(isDashing)
            {

            enemy.TakeDamage();
            logic.addScore();
              StartCoroutine(BulletTimeCoroutine(1.0f));  // 1 second of bullet time
            canDash = true;
            }
            TakeDamage();
        }
    }

    private IEnumerator ScreenWipe()
    {
    
        screenWipe.GetComponent<Animator>().SetTrigger("Wipe");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        canScreenWipe = false;
        yield return new WaitForSeconds(screenWipeCooldown);
        canScreenWipe = true;
    }
}