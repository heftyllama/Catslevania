using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject collectible;
    public static event Action pickedUp;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "PlayerCharacterControl"){
            OnPickUp();
        }   
    }
    private void OnPickUp() {
        pickedUp();
        Destroy(this.gameObject);
    }
}
