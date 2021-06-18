using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{

    public float speed;
    float distance;
    public float damage;
    public float damageDelay = 3f;
    public bool enemyIsActive = false;
    private Transform player;
    public Transform groundDetection;
    public BoxCollider2D wallDetection;
    private bool isHidden;
    private bool isSafe;
    public bool canDamage = true;
    public bool isMovingRight = true;
    public GameObject enemy;
    public string enemyName;

    public static event Action followPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate() {
        
        if(enemyName == "EctoRat") {
            Patrol();
        }
    }

    private void OnEnable() {
        PlayerController.playerIsSafe += EnemyActive;
        PlayerController.playerIsSafe += Follow;
    }

    private void OnDisable() {
        PlayerController.playerIsSafe -= EnemyActive;
        PlayerController.playerIsSafe -= Follow;   
    }
    

    void EnemyActive(bool isSafe) {
        Debug.Log(isSafe + " from enemyactive");
        if(!isSafe) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Follow(bool isSafe) {
        if(enemyName == "RatSkull" && !isSafe) {
            
            distance = Vector3.Distance(transform.position, player.position);
            //isHidden = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHiding;
            //isSafe = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isSafe;

            if(distance < 10f) {
                enemyIsActive = true;
                canDamage = true;
                Debug.Log("player can be followed");
                followPlayer();
            }
            else {
                enemyIsActive = false;
                canDamage = false;
                Debug.Log("player can not be followed");
            }
            Debug.Log("enemy is active: " + enemyIsActive);
        }  
    }

    void Patrol() {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        if(groundInfo.collider == false ) {
            if(isMovingRight == true) {
                transform.eulerAngles = new Vector3(0, -180, 0);
                isMovingRight = false;
            }
            else {
                transform.eulerAngles = new Vector3(0,0,0);
                isMovingRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ground")
        {
            SwitchDirection();
        }
    }

    void SwitchDirection() => enemy.transform.localScale = new Vector3(-1, 1, 1);
    bool WallDetected() {
        return true;
    }
}
