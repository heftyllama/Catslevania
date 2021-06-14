using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isDead;
    float damageAmount;
    public float playerMovementSpeed;
    public float stoppingSpeed;
    public float dashLength;
    public float jumpForce,dashForce;
    public bool isFacingRight,isHiding, isSafe;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    public static bool isGrounded;
    public static bool nearShelterObject;
    private bool damageable;
    private Vector2 horizontalMovement;
    private Vector2 verticalMovement;
    float moveHorizontal,moveVertical;

    public static bool dashing;
    private ParticleSystem healthParticleSystem;
    public GameObject healthParticles;
    public GameObject player;
    public Slider healthBar;

    void Start()
    { 
        rb = this.GetComponent<Rigidbody2D>();
        playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        isHiding = false;
        isDead = false;
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if(moveHorizontal == 0)
        {
            StopPlayerMomentum();
        }
        else
        {
            if(isHiding == false)
            {
                MoveCharacter(horizontalMovement);
            }
        }

        Dash();

        if(Input.GetAxisRaw("Vertical") > 0 & isGrounded == true)
        {

        Jump(verticalMovement);

        }

        //TODO:Drop Through the Floor
    }
    void Update()
    {
        UpdateHealthBar();
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
        if(currentHealth <= 0) {
            isDead = true;
            Debug.Log("Game over");
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

            if(damageable && currentHealth > 0) {
                currentHealth -= damageAmount;
                Debug.Log("Damaged by " + damageAmount);
                Debug.Log("New health: " + currentHealth);
            } 
        }
        if(other.gameObject.tag == "Safe") {
            isSafe = true;
            playerSprite.sortingLayerName = "PlayerHidden";
            RegenerateHealth();
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
            isSafe = false;
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
        GetComponent<Rigidbody2D>().drag = stoppingSpeed;
    }
    private void MoveCharacter(Vector2 direction)
    {
        CheckDirectionFacing(moveHorizontal);
        rb.GetComponent<Rigidbody2D>().AddForce(direction * playerMovementSpeed);
    }
    private void Jump(Vector2 direction)
    {
    
        rb.GetComponent<Rigidbody2D>().AddForce(direction * jumpForce,ForceMode2D.Impulse);
        Debug.Log("Jumping");
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
            isSafe = true;
            Debug.Log("You are hiding");
            playerSprite.sortingLayerName = "PlayerHidden";
            
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isHiding = false;
            isSafe = false;
            Debug.Log("You are not hiding");
            playerSprite.sortingLayerName = "Player";
        }
    }
    public void Squeak()
    {

    }

    private void RegenerateHealth() {
        currentHealth = maxHealth;
        Debug.Log("You are safe. Health restored. " + currentHealth);
        GameObject currentHealthParticles = Instantiate(healthParticles, player.transform);
        healthParticleSystem = healthParticles.GetComponent<ParticleSystem>();
        Destroy(currentHealthParticles, healthParticleSystem.main.duration);
    }

    ///////////////////////////////Bonus//////////////////////////////
    public void Laserpointer()
    {

    }

    private void UpdateHealthBar() {
        healthBar.value = currentHealth;
    }
}
