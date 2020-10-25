using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState 
{ 
    Mainmenu, 
    Playing, 
    Paused, 
    GameOver, 
    Win 
};

public class GameManager : MonoBehaviour
{
    //public GameManager instance for easy access
    public static GameManager Instance;

    //setting game state to organize game loop
    public GameState currentGameState;


    [Header("Player")]
    public GameObject playerPrefab;
    public bool isPlayerSpawned;


    [Header("Enemy")]
    public GameObject enemyPrefab;
    public bool isEnemySpawned;

    private EntitySpawner entitySpawner;


    [Header("Scenes")]
    private string currentSceneName;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && currentGameState != GameState.Mainmenu)
        {
            BackToMainMenu();
        }
    }

    #region GameStates

    public void StartScene(string sceneName)
    {
        currentGameState = GameState.Playing;

        //for now this just starts the hard coded scene
        currentSceneName = sceneName;

        //loading scene additively to keep the menu and game manager
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void PauseGame()
    {
        currentGameState = GameState.Paused;
    }

    public void BackToMainMenu()
    {
        currentGameState = GameState.Mainmenu;
        Application.Quit();
        /*
        if (entitySpawner == null)
        {
            entitySpawner = FindObjectOfType<EntitySpawner>();
            entitySpawner.RemoveEntities();
        }

        SceneManager.UnloadSceneAsync(currentSceneName);
        SceneManager.LoadScene("MainMenu");
        */
    }

    #endregion
}
