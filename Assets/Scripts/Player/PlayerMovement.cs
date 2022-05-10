using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInputActions controls;
    private Rigidbody2D playerRB;
    public float speed, jumpHeight;
    private float horizontalMovement, verticalMovement;
    public bool isGrounded;
    public bool isFacingRight;
    public bool isRunning;
    [SerializeField] private int jumpCount;
 
    private Animator animator;
    private SpriteRenderer playerSprite;

    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D groundCheck;
    
    private void Awake() {
        controls = new PlayerInputActions();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();

        isFacingRight = true;

        controls.PlayerMovement.MoveHorizontal.performed += context => MovePlayer(context.ReadValue<float>());
        controls.PlayerMovement.Jump.performed += context => Jump();

        controls.PlayerMovement.MoveHorizontal.canceled -= context => MovePlayer(context.ReadValue<float>());
        controls.PlayerMovement.Jump.canceled -= context => Jump();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    private void FixedUpdate() {
        Vector3 currentPosition = transform.position;
        currentPosition.x += horizontalMovement * speed * Time.fixedDeltaTime;
        transform.position = currentPosition;
    }

    private void MovePlayer(float horizontalDirection) {
        horizontalMovement = horizontalDirection;
        if(horizontalDirection != 0) {
            isRunning = true;
        }
        else isRunning = false;

        if(horizontalMovement > 0 && !isFacingRight) {
            FlipSprite();
        }
        if(horizontalMovement < 0 && isFacingRight) {
            FlipSprite();
        }

        animator.SetBool("isRunning", isRunning);
    }

    private void Jump() {
        if(IsGrounded()) {
            playerRB.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
    }

    private void Dash() {
        /* Vector2 dashDirection;
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
        dashing = false; */
    }

    private void FlipSprite() {
        playerSprite.flipX = isFacingRight;
        isFacingRight = !isFacingRight;
    }

    private bool IsGrounded() {
        isGrounded = groundCheck.IsTouchingLayers(ground);
        return isGrounded;
    }
}
