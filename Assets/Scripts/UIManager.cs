using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    [SerializeField] private TextMeshProUGUI currentCollectiblesCount;
    [SerializeField] private TextMeshProUGUI totalCollectiblesCount;
    
    void OnEnable() {
        PlayerController.onPlayerHealthChange += UpdateHealthBar;
        CollectibleManager.setCurrentCollectibles += UpdateCurrentCollectibles;
        CollectibleManager.setTotalCollectibles += UpdateTotalCollectibles;
    }

    void OnDisable() {
        PlayerController.onPlayerHealthChange -= UpdateHealthBar;
        CollectibleManager.setCurrentCollectibles -= UpdateCurrentCollectibles;
        CollectibleManager.setTotalCollectibles -= UpdateTotalCollectibles;
    }

    private void UpdateTotalCollectibles(float totalCollectibles) {
        totalCollectiblesCount.text = totalCollectibles.ToString();
    }

    private void UpdateCurrentCollectibles(float currentCollectibles) {
        currentCollectiblesCount.text = currentCollectibles.ToString();
    }

    private void UpdateHealthBar(float health) {
        if(health < healthBar.value) {
            healthBar.animator.SetBool("IsBeingDamaged", true);
        }
        healthBar.value = health;
    }
}
