using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSkullPatrol : MonoBehaviour
{
    public Transform moveSpot;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    // Update is called once per frame
    void Update()
    {

        if(GetComponent<EnemyController>().enemyIsActive == false) 
        {
            if(Vector2.Distance(this.transform.position, moveSpot.position) < 0.2f) {
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
        this.transform.position = Vector2.MoveTowards(this.transform.position, moveSpot.position, GetComponent<EnemyController>().speed * Time.deltaTime);
        }
    }
}
