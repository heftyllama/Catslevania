using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isDead;
    float damageAmount;
    public bool isHiding, isSafe;
    public static Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    public static bool nearShelterObject;
    private bool damageable;
    private ParticleSystem healthParticleSystem;
    public GameObject healthParticles;
    public GameObject damageParticles;
    private ParticleSystem damageParticleSystem;
    public GameObject player;
    public Slider healthBar;

    Animator slideAnim; 

    #region Delegates
        
    public static event Action<float> onPlayerHealthChange;
    public static event Action<bool> playerIsSafe;
    public static event Action<Collider2D> onDamaged;

    #endregion

    void Start()
    { 
        rb = this.GetComponent<Rigidbody2D>();
        playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        isHiding = false;
        isDead = false;
        currentHealth = maxHealth;
        slideAnim = healthBar.GetComponent<Animator>();
    }

    void Update()
    {

        if (nearShelterObject)
        {
            Debug.Log ("You can hide");
            Hide();
        }
        if(currentHealth <= 0) {
            isDead = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Shelter")
        {
            Debug.Log("You are near a shelter object! Take cover with E");
            nearShelterObject = true;
        }
        if(other.gameObject.tag == "Enemy") {  
            OnDamaged(other);
        }
        if(other.gameObject.tag == "Safe") {
            isSafe = true;
                       
            playerIsSafe(true);
            
            playerSprite.sortingLayerName = "PlayerHidden";
            RegenerateHealth();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy") {
            slideAnim.SetBool("IsBeingDamaged", false);
        }
        if(other.gameObject.tag == "Shelter")
        {
            Debug.Log("You have walked away from the shelter please pray");
            nearShelterObject = false;
        }
        if(other.gameObject.tag == "Safe") {
            isSafe = false;
            playerIsSafe(false);
            playerSprite.sortingLayerName = "Player";
            Debug.Log("Good luck out there.");
        }
    }
    private void Hide()
    {
        /*if(Input.GetKey(KeyCode.E))
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
        }*/
    }
    public void Squeak()
    {

    }

    private void OnDamaged(Collider2D other) {
        damageable = other.gameObject.GetComponent<EnemyController>().canDamage;
        damageAmount = other.gameObject.GetComponent<EnemyController>().damage;
        slideAnim.SetBool("IsBeingDamaged", true);


        if(damageable && currentHealth > 0) {
            currentHealth -= damageAmount;
            Debug.Log("Damaged by " + damageAmount);
            Debug.Log("New health: " + currentHealth);
            onPlayerHealthChange(currentHealth);
        } 

        GameObject currentDamageParticles = Instantiate(damageParticles, player.transform);
        damageParticleSystem = damageParticles.GetComponent<ParticleSystem>();
        Destroy(currentDamageParticles, damageParticleSystem.main.duration);
    }

    private void RegenerateHealth() {
        currentHealth = maxHealth;
        onPlayerHealthChange(currentHealth);
        GameObject currentHealthParticles = Instantiate(healthParticles, player.transform);
        healthParticleSystem = healthParticles.GetComponent<ParticleSystem>();
        Destroy(currentHealthParticles, healthParticleSystem.main.duration);
    }

    ///////////////////////////////Bonus//////////////////////////////
    public void Laserpointer()
    {

    }
}
