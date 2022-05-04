using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float WallJumpTimer;


    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash = -100f;

    private float knockbackStartTime;
    [SerializeField] private float knockbackDuration;


    //public float ledgeClimpXOffset1 = 0f;
    //public float ledgeClimpYOffset1 = 0f;
    //public float ledgeClimpXOffset2 = 0f;
    //public float ledgeClimpYOffset2 = 0f;


    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;


    private bool isFacingRigth = true;
    private bool isRunning;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    public bool canMove;
    private bool canFlip;
    private bool hasWallJumped;

    //private bool isTouchingLedge;
    //private bool canClimpLedge = false;
    //private bool ledgeDetected;

    private bool isDashing;

    private bool knockback;
    [SerializeField] private Vector2 knockbackSpeed;


    //private Vector2 ledgePosBot;
    //private Vector2 ledgePos1;
    //private Vector2 ledgePos2;


    private Rigidbody2D rb;
    private Animator anim;


    public int amountOfJumps = 1;


    public float movementSpeed = 5f;
    public float jumpForce = 6.0f;
    public float wallSlideSpeed;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;

    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;


    public Image dashCooldownImage;
    public float dashCoolDown;
    bool isCooldown = false;


    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;


    public Transform groundCheck;
    public Transform wallCheck;
    //public Transform ledgeCheck;


    public LayerMask whatIsGround;


    void Start()
    {
        dashCooldownImage.fillAmount = 0;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }


    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        //CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
    }


    private void FixedUpdate() 
    {
        ApplyMovement();
        CheckSurroundings();
    }


    private void CheckIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 /*&& !canClimpLedge*/)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);

    }

    public void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        
    }
    
    /* private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimpLedge)
        {
            canClimpLedge = true;

            if(isFacingRigth)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x * wallCheckDistance) - ledgeClimpXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimpYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x * wallCheckDistance) + ledgeClimpXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimpYOffset2);
            }
            else
            {
                 ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimpXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimpYOffset1);
                 ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x - wallCheckDistance) + ledgeClimpXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimpYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimLedge", canClimpLedge);
        }
        if(canClimpLedge)
        {
            transform.position = ledgePos1;
        }
    } */

    /*public void FinishLedgeClimp()
    {
        canClimpLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimLedge", canClimpLedge);
    }*/

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if(isTouchingWall)
        {
            canWallJump = true;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }


    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        /*isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(isTouchingLedge && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;

        }*/
    }
    private void CheckMovementDirection()
    {
        //Flips player movement direction
        if(isFacingRigth && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRigth && movementInputDirection > 0)
        {
            Flip();
        }
        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void UpdateAnimation()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isDashing", isDashing);
    }


    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if(turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if(checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
        if(Input.GetButtonDown("Dash"))
        {
            //isCooldown = true;
            //dashCooldownImage.fillAmount = 1;

            if(Time.time >= (lastDash + dashCoolDown))
            AttemptToDash();
        }

        if(isCooldown)
        {
            dashCooldownImage.fillAmount -= 1 / dashCoolDown * Time.deltaTime;

            if(dashCooldownImage.fillAmount <= 0)
            {
                dashCooldownImage.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImageBool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }


    private void CheckDash()
    {


        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                
                isCooldown = true;
                dashCooldownImage.fillAmount = 1;
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;

                if(Mathf.Abs(-transform.position.x - lastImageXPos) > distanceBetweenImages)
                {
                PlayerAfterImageBool.Instance.GetFromPool();
                lastImageXPos = transform.position.x;
                }  
            }
            
            if(dashTimeLeft <- 0 || isTouchingWall)
            {

                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }       
    }

    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            //WALL JUMP
            if(!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if(isGrounded)
            {
                NormalJump();
            }
        }

        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if(WallJumpTimer > 0)
        {
            if(hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if(WallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                WallJumpTimer -= Time.deltaTime;
            }
        }
    }
    


    private void NormalJump()
    {
        if(canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }


    private void WallJump()
    {
        if(canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2
                (wallJumpForce * wallHopDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            //turnTimer = 0;
            //canMove =true;
            //canFlip = true;
            hasWallJumped = true;
            WallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }


    private void ApplyMovement()
    {
        if(!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        } 
        else if(canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }

        if(isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void DisableFlip()
    {
        canFlip = false;
    }
    private void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if(!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRigth = !isFacingRigth;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    /*private void SnapDirectionVector()
    {
        Vector3 SnapTo(Vector3 v3, float snapAngle) 
        {
        float   angle = Vector3.Angle (v3, Vector3.up);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return Vector3.up * v3.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return Vector3.down * v3.magnitude;
     
        float t = Mathf.Round(angle / snapAngle);
        float deltaAngle = (t * snapAngle) - angle;
     
        Vector3 axis = Vector3.Cross(Vector3.up, v3);
        Quaternion q = Quaternion.AngleAxis (deltaAngle, axis);
            return q * v3;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine
            (wallCheck.position, new Vector3
                (wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
