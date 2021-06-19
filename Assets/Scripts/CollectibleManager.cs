using System;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private GameObject collectible;
    public Collectible[] collectibles;
    public static int numberOfCollectibles;
    public int numberOfCollectiblesCollected;
    public bool allCollectiblesCollected;

    public static event Action<float> setTotalCollectibles;
    public static event Action<float> setCurrentCollectibles;

    private void Awake() {
        collectible = GetComponent<GameObject>(); 
        collectibles = FindObjectsOfType<Collectible>();
        numberOfCollectibles = collectibles.Length;
        setTotalCollectibles(numberOfCollectibles);
        
        numberOfCollectiblesCollected = 0;
        setCurrentCollectibles(numberOfCollectiblesCollected);
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
        setCurrentCollectibles(numberOfCollectiblesCollected);
    }

    public void AllPickupsCheck() {
        if(numberOfCollectiblesCollected == numberOfCollectibles) {
            Debug.Log("you got all the collectibles!");
            allCollectiblesCollected = true;
        }
    }
}
