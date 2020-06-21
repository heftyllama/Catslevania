using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    public float speed;
    public float distance;
    bool enemyIsActive = false;
    private Transform target;
    private bool isHidden;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate() {
        distance = Vector3.Distance(transform.position, target.position);
        isHidden = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>().isHiding;

        if(distance < 10f && !isHidden) {
            enemyIsActive = true;
        }
        else if(isHidden) {
            enemyIsActive = false;
        }
        Debug.Log("enemy is active: " + enemyIsActive);
        EnemyActive(enemyIsActive);
    }

    void EnemyActive(bool activity) {
        if(activity) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
