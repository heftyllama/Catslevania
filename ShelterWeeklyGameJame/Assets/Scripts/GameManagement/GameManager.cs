using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerAttributes playerAttributes;
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
        if(playerAttributes.isDead)
        {
            gameLost = true;
        }
        else if(finalBossDefeated)
        {
            gameWon = true;
        }
    }

    
}
