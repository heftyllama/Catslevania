using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public bool isNewGame;
    public bool gameLost;
    public bool gameWon;
    public bool finalBossDefeated;

    public void Update()
    {
        CheckGameWinCondition();
    }
    public void NewGame()
    {
        finalBossDefeated = false;
        isNewGame = true;
    }
    public void CheckGameWinCondition()
    {
        if(playerController.isDead)
        {
            gameLost = true;
            Time.timeScale = 0;
        }
        else if(finalBossDefeated)
        {
            gameWon = true;
        }
    }

    
}
