using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float speed;
    float distance;
    public float damage;
    public float damageDelay = 3f;
    public bool enemyIsActive = false;
    public Transform target;
    public Transform groundDetection;
    public BoxCollider2D wallDetection;
    private bool isHidden;
    private bool isSafe;
    public bool canDamage = true;

    public bool canInflictContactDamage;
    public bool isMovingRight = true;
    public GameObject enemy;
    public string enemyName;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();  
    }

    void FixedUpdate() {
        if(enemyName == "RatSkull") {
            Follow();
        }
        if(enemyName == "EctoRat") {
            Patrol();
        }
    }

    void EnemyActive(bool activity) 
    {
            if(activity) 
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
            }
           /* else if(!activity && name.Contains("Patrol"))
            {

            }*/
    }

    void Follow() {
        distance = Vector3.Distance(this.transform.position, target.position);
        isHidden = player.GetComponent<PlayerAttributes>().isHiding;

        if(distance < 10f && !isHidden) {
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
    /*void Idle()
    {
        
        this.transform.position = Vector2.MoveTowards(this.transform.position, moveSpot.position, GetComponent<EnemyController>().speed * Time.deltaTime);
    }*/


   /* void Disengage()
    {
         if(Vector2.Distance(this.transform.position, moveSpot.position) < 0.2f) 
         {
                if(waitTime <= 0) 
                {
                    moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    Debug.Log("I am " + this.gameObject.name + "and I am moving here" + moveSpot.position);
                    waitTime = startWaitTime;
                }
                else 
                {
                    Debug.Log("I am waitiing");
                    waitTime -= Time.deltaTime;
                }
        }
    }*/
    void Patrol() {
        this.transform.Translate(Vector2.right * speed * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Ground")
        {
            SwitchDirection();
        }
        
        if(canInflictContactDamage)
        {
            if(other.gameObject.tag == "Player")
            {
                ReducePlayerHealthOnContact(damage,other.gameObject);
            }
            else
            {
                SwitchDirection();
            }
        }  
    }


    void SwitchDirection() => enemy.transform.localScale = new Vector3(-1, 1, 1);
    bool WallDetected() {
        return true;
    }

    void ReducePlayerHealthOnContact(float damage,GameObject player)
    {
        bool dealingDamage = true;
        if(dealingDamage)
        {
            PlayerAttributes.playerHealth -= damage;
            dealingDamage =false;
        }
    }
}
