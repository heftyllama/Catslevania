using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    
    void OnEnable() {
        PlayerController.onPlayerHealthChange += UpdateHealthBar;
    }

    void OnDisable() {
        PlayerController.onPlayerHealthChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float health) {
        healthBar.value = health;
    }
}
