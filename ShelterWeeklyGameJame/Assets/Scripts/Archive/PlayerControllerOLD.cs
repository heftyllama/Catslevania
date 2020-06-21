using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOLD : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private LayerMask whatIsGround;
    public float slopeCheckDistance = 2f;

    private float slopeDownAngle;

    private Vector2 colliderSize,slopeNormalPerpendicular, velocity;
    private Vector2 horizontalMovement,verticalMovement;
    public GameObject yarn;
    private SpriteRenderer playerSprite;

    private float angle;
    public static float health = 100;
    public float damageAmount,playerVelocity,stoppingSpeed,dashLength;
    private float moveHorizontal,moveVertical;  
    public float jumpVelocity,dashForce;
    public float fallMultiplier = 2.5f,lowJumpMultiplier = 2f;

    public bool isDead,isFacingRight,isHiding,damageable,hasYarn;
    public static bool isJumping,dashing,isGrounded,nearShelterObject;

    void Start()
    {
        whatIsGround = LayerMask.NameToLayer("Ground");
        rb = this.GetComponent<Rigidbody2D>();
        cc = this.GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
        
        playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        isHiding = false;
        isDead = false;
    }

    void FixedUpdate()
    {
        ApplyGravityWhileMoving();
        //MoveCharacter(horizontalMovement);
        Dash();
        Jump(isJumping);
        SlopeCheck();
        //TODO:Drop Through the Floor
    }
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        horizontalMovement = new Vector2(moveHorizontal,0); 

        moveVertical = Input.GetAxisRaw("Vertical");
        verticalMovement = new Vector2(0,moveVertical);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("You can dash");
            dashing = true;
        }

        if (nearShelterObject)
        {
            Debug.Log ("You can hide");
            Hide();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ThrowYarn();
        }
        if(Input.GetKeyDown(KeyCode.W) & isGrounded)
        {
            isJumping = true;
        }
        else if(!isGrounded)
        {
            isJumping = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if(other.gameObject.tag == "Shelter")
        {
            Debug.Log("You are near a shelter object! Take cover with E");
            nearShelterObject = true;
        }
        if(other.gameObject.tag == "Enemy") {
            damageable = other.gameObject.GetComponent<EnemyController>().canDamage;
            damageAmount = other.gameObject.GetComponent<EnemyController>().damage;

            if(damageable && health > 0) {
                health -= damageAmount;
                Debug.Log("Damaged by " + damageAmount);
                Debug.Log("New health: " + health);
            } 
        }
        if(other.gameObject.tag == "Safe") {
            isHiding = true;
            playerSprite.sortingLayerName = "PlayerHidden";
            health += 10;
            Debug.Log("You are safe. Health restored. " + health);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
        if(other.gameObject.tag == "Shelter")
        {
            Debug.Log("You have walked away from the shelter please pray");
            nearShelterObject = false;
        }
        if(other.gameObject.tag == "Safe") {
            isHiding = false;
            playerSprite.sortingLayerName = "Player";
            Debug.Log("Good luck out there.");
        }
    }

    private void CheckDirectionFacing(float directionFacing)
    {
        if(directionFacing < 0)
        {
            playerSprite.flipX = true;
            isFacingRight = false;
        }
        else if (directionFacing > 0)
        {
            playerSprite.flipX = false;
            isFacingRight = true;
        }
    }
    private void StopPlayerMomentum()
    {
        rb.drag = stoppingSpeed;
    }

    float gravityModifier = 10f;
    private void ApplyGravityWhileMoving()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = -horizontalMovement * deltaPosition.y;
        MoveCharacter(move);
    }

    private void MoveCharacter(Vector2 direction)
    {
        CheckDirectionFacing(moveHorizontal);

        rb.MovePosition((Vector2)transform.position + (direction * playerVelocity * Time.fixedDeltaTime));
    }


    private void Jump(bool pressedJumpButton)
    {
        JumpControl();
        if(pressedJumpButton)
        {
            Debug.Log("Jumping");
            rb.velocity = Vector2.up * jumpVelocity;
        }
        //rb.AddForce(direction * jumpForce,ForceMode2D.Impulse);
    }

    private void SlopeCheck()
    {
       Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y/2);
       SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos,Vector2.down,slopeCheckDistance,whatIsGround);

        if(hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal); //should return vector2 that points to the left
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.DrawRay(hit.point,slopeNormalPerpendicular,Color.red);
            Debug.DrawRay(hit.point,hit.normal,Color.green);
        }
    }
    private void JumpControl()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W)) //TODO: Add another jump key
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void Dash()
    {
        Vector2 dashDirection;
        if(dashing == true)
        {
            if(isFacingRight)
            {
                dashDirection = new Vector2(dashLength,0);
                Debug.Log("Dash action happening");
                rb.AddForce(dashDirection * dashForce,ForceMode2D.Impulse);
            }
            else if(!isFacingRight)
            {
                dashDirection = new Vector2(-dashLength,0);
                rb.AddForce(dashDirection * dashForce,ForceMode2D.Impulse);
            }
        }
        dashing = false;
    }
    private void Hide()
    {
        if(Input.GetKey(KeyCode.E))
        {
            isHiding = true;
            Debug.Log("You are hiding");
            playerSprite.sortingLayerName = "PlayerHidden";
            
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isHiding = false;
            Debug.Log("You are not hiding");
            playerSprite.sortingLayerName = "Player";
        }
    }
    public void Squeak()
    {

    }

    ///////////////////////////////Bonus//////////////////////////////
    public void Laserpointer()
    {

    }

    public void ThrowYarn()
    {
        Vector2 throwDirection;
        GameObject yarnObj;

        if(hasYarn == true)
        {
            if(isFacingRight)
            {
                throwDirection = new Vector2(dashLength,0);
                yarnObj = Instantiate(yarn,new Vector2(this.transform.position.x + 1, this.transform.position.y),Quaternion.identity) as GameObject;
                yarnObj.GetComponent<Rigidbody2D>().AddForce(throwDirection * dashForce,ForceMode2D.Impulse);
                hasYarn = false;
            }
            else if(!isFacingRight)
            {
                throwDirection = new Vector2(-dashLength,0);
                yarnObj = Instantiate(yarn,new Vector2(this.transform.position.x - 1, this.transform.position.y),Quaternion.identity) as GameObject;
                yarnObj.GetComponent<Rigidbody2D>().AddForce(throwDirection * dashForce,ForceMode2D.Impulse);
                hasYarn = false;
            }
        }
    }
}
