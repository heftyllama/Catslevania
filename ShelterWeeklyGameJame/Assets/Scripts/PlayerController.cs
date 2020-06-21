using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAttributes playerAttributes;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    
    public float dashForce,dashLength;

    public float glideSpeed;
    public float fallMultiplier = 2.5f,lowJumpMultiplier = 2f;

    public int facingDirection = 1;
    [SerializeField]
    private bool isGrounded;
    public bool isOnSlope;

    private bool isNearWall;

    private bool isJumping,pressedJump,canJump;
    private bool shortJump;
    [SerializeField]
    private bool canWalkOnSlope;
    private bool isDashing, pressedDash, canDash;
    public float dashTimer = 0;
    private float maxDashTime = 0.2f;
    private bool isGliding;
    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;
    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        canDash = true;
        capsuleColliderSize = cc.size;
    }
    private void Update()
    {
        CheckInput();
        CheckJumpType();
        CheckDashTime();
    }
    private void FixedUpdate()
    {
        ApplyJumpGravity();
        CheckGround();
        SlopeCheck();
       // RotateObjectWithCurrentSlopeAngle();
        WallCheck();
        ApplyMovement();
        Dash();
        Glide();
        Debug.Log("isGrounded" + isGrounded);
        Debug.Log("canWalkOnSlope" + canWalkOnSlope);
       // Debug.Log("isOnSlope" + isOnSlope);
        Debug.Log("isJumping" + isJumping);
    }
    private void CheckInput()
    {

        xInput = Input.GetAxisRaw("Horizontal");

        if (xInput == 1 && facingDirection == -1)
        {
            Flip();
        }
        else if (xInput == -1 && facingDirection == 1)
        {
            Flip();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(canDash)
            {
                isDashing = true;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            pressedJump = true;
            Jump();
        }
        if(Input.GetKey(KeyCode.LeftShift) && !isGrounded)
        {
            isGliding = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isGliding = false;
        }
    }
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
            isGliding  = false;
        }
    }
    private void WallCheck()
    {
       if(isGrounded && isOnSlope)
       {
           Debug.Log("You're next tp wall");
           isNearWall = true;
       }
       else
       {
           isNearWall = false;
       }
    }
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
  
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
            Debug.Log("One slope is " + isOnSlope);
        }
    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }
    
    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
            newForce.Set(0.0f, jumpForce);
            //rb.AddForce(newForce, ForceMode2D.Impulse);
            rb.velocity = Vector2.up * newForce;
        }
    }   
    private void ApplyMovement()
    {
        if (isGrounded && !isOnSlope && !isJumping || (isNearWall && !isJumping)) //if not on slope
        {
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
        {
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            rb.velocity = newVelocity;
        }
        else if (!isGrounded) //If in air
        {
            newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }

    }
    private void Dash()
    {
        if(isDashing)
        {
            Debug.Log("Dash");
            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
            newForce.Set(dashForce, 0.0f);
            //rb.AddForce(newForce, ForceMode2D.Impulse);
            if(facingDirection == 1)
            {
                rb.velocity = Vector2.right * newForce;
            }
            else if(facingDirection == -1)
            {
                rb.velocity = -Vector2.right * newForce;
            }
        }
    }
    private void CheckDashTime()
    {

        if(dashTimer > maxDashTime)
        {
            isDashing = false;
            canDash = true;
            Debug.Log("Dashing stopped");
            dashTimer = 0;
        }
        else
        {
            if(isDashing)
            {
            dashTimer += Time.deltaTime;
            }
        }
    }
    private void Glide()
    {
        if(isGliding)
        {
            Vector2 glideDirection = new Vector2(glideSpeed,0.0f) * Time.fixedDeltaTime;
            
            if(facingDirection == 1)
            {
                rb.velocity = glideDirection;
            }            
            else if(facingDirection == -1)
            {
                rb.velocity = -glideDirection;
            }
        }
    }
    
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void RotateObjectWithCurrentSlopeAngle()
    {
        //Needs to rotate without rotating raycast
        if (slopeSideAngle != 0 && isOnSlope)
        {
        float rotationAngle = slopeSideAngle;
        rb.rotation = rotationAngle;
        }
        else
        {
        rb.rotation = 0;
        }
    }

    private void CheckJumpType()
    {
        if(pressedJump)
        {
            if(rb.velocity.y < 0) //if the object has a velocity = to 0
            {
                shortJump = false;
            }
        
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) //if the object is going up and the player is not holding jump
            {
                shortJump = true;
            }
        }
    }
    private void ApplyJumpGravity()
    {
        if(shortJump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            Debug.Log("short Jump");
        }
        else if (!shortJump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            Debug.Log("long jump");
        }
    }
    public void UseItem()
    {
        Vector2 throwDirection;
        GameObject itemObj;

        if(playerAttributes.hasYarn == true)
        {
            if(facingDirection ==  1 )
            {
                throwDirection = new Vector2(dashLength,0);
                itemObj = Instantiate(playerAttributes.currentItem,new Vector2(this.transform.position.x + 1, this.transform.position.y),Quaternion.identity) as GameObject;
                itemObj.GetComponent<Rigidbody2D>().AddForce(throwDirection * dashForce,ForceMode2D.Impulse);
                playerAttributes.hasYarn = false;
            }
            else if(facingDirection == -1)
            {
                throwDirection = new Vector2(-dashLength,0);
                itemObj = Instantiate(playerAttributes.currentItem,new Vector2(this.transform.position.x - 1, this.transform.position.y),Quaternion.identity) as GameObject;
                itemObj.GetComponent<Rigidbody2D>().AddForce(throwDirection * dashForce,ForceMode2D.Impulse);
                playerAttributes.hasYarn = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}

