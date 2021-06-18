using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<string> LevelList;
    public List<GameObject> perservedGameObjects;
    void Awake()
    {
        for(int i = 0 ; i < perservedGameObjects.Count; i++)
        {
            DontDestroyOnLoad(perservedGameObjects[i]);
        }
    }

    void Update()
    {
        if(gameManager.isNewGame)
        {
            SceneLoader("New Game Scene");  //New Game Scene will explain the story and have a tutorial
            gameManager.isNewGame = false;
        }
        LoadGameOver();
    }

    void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGameOver()
    {
        if(gameManager.gameLost)
        {
            Debug.Log("Game Over");
            SceneLoader("GameOverScene");
        }
        if(gameManager.gameWon)
        {
            Debug.Log("Victory!");
            //Load Victory Scene
            SceneLoader("GameWonScene");
        }
    }
}
