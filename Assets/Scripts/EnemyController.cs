using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed;
    float distance;
    public float damage;
    public float damageDelay = 3f;
    public bool enemyIsActive = false;
    private Transform target;
    public Transform groundDetection;
    public BoxCollider2D wallDetection;
    private bool isHidden;
    private bool isSafe;
    public bool canDamage = true;
    public bool isMovingRight = true;
    public GameObject enemy;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();  
    }

    void FixedUpdate() {
        if(name == "RatSkull") {
            Follow();
        }
        if(name == "EctoRat") {
            Patrol();
        }
    }

    void EnemyActive(bool activity) {
        if(activity) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void Follow() {
        distance = Vector3.Distance(transform.position, target.position);
        isHidden = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHiding;
        isSafe = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isSafe;

        if(distance < 10f && !isHidden && !isSafe) {
            enemyIsActive = true;
            canDamage = true;
        }
        else {
            enemyIsActive = false;
            canDamage = false;
        }
        Debug.Log("enemy is active: " + enemyIsActive);
        EnemyActive(enemyIsActive);
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
