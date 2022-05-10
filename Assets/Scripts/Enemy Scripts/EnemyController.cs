using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{

    public float speed;
    public float damage;
    public float damageDelay = 3f;
    private Transform player;
    public bool canDamage = true;
    public static event Action patrol;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable() {
        PlayerController.playerIsSafe += EnemyActive;
    }

    private void OnDisable() {
        PlayerController.playerIsSafe -= EnemyActive;  
    }
    

    void EnemyActive(bool isSafe) {
        if(!isSafe) {
            canDamage = true;
        }
        if(isSafe) {
            canDamage = false;
        }
    }

}
