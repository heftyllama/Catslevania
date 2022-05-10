using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSkullPatrol : MonoBehaviour
{
    private Vector2 positionToMove;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    private float minX;
    private float minY;
    private float maxX;
    private float maxY;
    private Vector2 spawnPosition;
    private float distanceFromPlayer;
    [SerializeField] private float distanceThreshold;
    private GameObject player;
    private PlayerController playerController;

    private float speed;

    void Awake() {
        speed = GetComponent<EnemyController>().speed;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
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
    }

    void Update()
    {
        distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
           
        if(distanceFromPlayer > distanceThreshold || playerController.isSafe) {
            Patrol();
            }
    
        else if(!playerController.isSafe && distanceFromPlayer < distanceThreshold){
            FollowPlayer();
        }

        transform.position = Vector2.MoveTowards(transform.position, positionToMove, speed * Time.deltaTime);
    }

    private void FollowPlayer() {
        positionToMove = player.transform.position;
        Debug.Log("im following the player!");
    }

    private void Patrol() {
        if(waitTime <= 0) {
            positionToMove = GetRandomPosition();
            waitTime = startWaitTime;
        }
        else waitTime -= Time.deltaTime;
        
        Debug.Log("i'm on patrol");
    }

    private Vector2 GetRandomPosition() {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
