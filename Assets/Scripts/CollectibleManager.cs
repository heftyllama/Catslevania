using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private GameObject collectible;
    public Collectible[] collectibles;
    public static int numberOfCollectibles;
    public int numberOfCollectiblesCollected;
    public bool allCollectiblesCollected;

    private void Awake() {
        collectible = GetComponent<GameObject>(); 
        collectibles = FindObjectsOfType<Collectible>();
        numberOfCollectibles = collectibles.Length;
        
        numberOfCollectiblesCollected = 0;
        allCollectiblesCollected = false;
    }

    private void OnEnable() {
        Collectible.pickedUp += OnPickUp;
        Collectible.pickedUp += AllPickupsCheck;
    }

    private void OnDisable() {
        Collectible.pickedUp -= OnPickUp;
        Collectible.pickedUp -= AllPickupsCheck;
    }

    public void OnPickUp() {
        numberOfCollectiblesCollected++;
    }

    public void AllPickupsCheck() {
        if(numberOfCollectiblesCollected == numberOfCollectibles) {
            Debug.Log("you got all the collectibles!");
            allCollectiblesCollected = true;
        }
    }
}
