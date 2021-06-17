using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSkullPatrol : MonoBehaviour
{
    private Vector2 positionToMove;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    private Vector2 spawnPosition;
    public float distanceFromPlayer;
    private Transform player;

    private float speed;

    void Awake() {
        speed = GetComponent<EnemyController>().speed;
    }
    void Start()
    {
        spawnPosition = transform.position;

        minX = spawnPosition.x - 5f;
        maxX = spawnPosition.x + 5f;
        minY = spawnPosition.y - 5f;
        maxY = spawnPosition.y + 5f;

        waitTime = startWaitTime;

        positionToMove = GetRandomPosition();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable() {
        EnemyController.followPlayer += FollowPlayer;
    }

    private void OnDisable() {
        EnemyController.followPlayer -= FollowPlayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GetComponent<EnemyController>().enemyIsActive == false) {
            transform.position = Vector2.MoveTowards(transform.position, positionToMove, speed * Time.deltaTime);
                        
            if(Vector2.Distance(transform.position, positionToMove) < 0.2f) {
                if(waitTime <= 0) {
                    positionToMove = GetRandomPosition();
                    waitTime = startWaitTime;
                    }
                else {
                    waitTime -= Time.deltaTime;
                    }
            }
        }
    }

    private void FollowPlayer() {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);
    }

    private Vector2 GetRandomPosition() {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
