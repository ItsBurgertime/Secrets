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
        else if(Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.Mainmenu)
        {
            Debug.Log("Application closing");
            Application.Quit();
        }
    }

    #region GameStates

    public void StartScene(string sceneName)
    {
        //for now this just starts the hard coded scene
        currentSceneName = sceneName;

        //loading scene additively to keep the menu and game manager
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        currentGameState = GameState.Playing;
    }

    public void PauseGame()
    {
        currentGameState = GameState.Paused;
    }

    public void BackToMainMenu()
    {
        currentGameState = GameState.Mainmenu;

        Cursor.lockState = CursorLockMode.None;

        SceneManager.UnloadSceneAsync(currentSceneName);

        GameManager.Instance.isPlayerSpawned = false;
        GameManager.Instance.isEnemySpawned = false;
    }

    #endregion



    #region Scene Loading

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
}
