using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBehaviour : BaseItem
{
    
    public void OnTriggerEnter2D(Collider2D col)
    {

        if(col.tag == "Player" && col.GetComponent<PlayerAttributes>().hasYarn == false)
        {
            col.GetComponent<PlayerAttributes>().playerInventory.Add(this.BaseItem);
            col.GetComponent<PlayerAttributes>().hasYarn = true;
            StartCoroutine(WaitAndDestroy());
        }

    }

    public float delay = 0.2f; //This implies a delay of 2 seconds.
 
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy (gameObject);
     }

}
