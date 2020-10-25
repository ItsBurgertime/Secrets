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

    public int playerScore = 0;
    public float playerTime = 30f;

    [Header("Enemy")]
    public GameObject enemyPrefab;
    public bool isEnemySpawned;
    public int enemyAmount;


    [Header("Scenes")]
    private string currentSceneName;

    [Header("Timer")]
    public float timeRemaining = 30f;
    public bool timerIsRunning = false;


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

        WinGame();
        Timer();
    }

    void Timer()
    {
        if(currentGameState == GameState.Playing)
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Timer has run out of time");
                    currentGameState = GameState.GameOver;
                    timeRemaining = 0;
                    timerIsRunning = false;

                    StartCoroutine(RestartGame());
                }
            }
        }
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

    #region GameStates

    public void StartScene(string sceneName)
    {
        //for now this just starts the hard coded scene
        currentSceneName = sceneName;

        //loading scene additively to keep the menu and game manager
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        currentGameState = GameState.Playing;
        timerIsRunning = true;
    }

    public void PauseGame()
    {
        currentGameState = GameState.Paused;
    }

    public void BackToMainMenu()
    {
        currentGameState = GameState.Mainmenu;

        SceneManager.UnloadSceneAsync(currentSceneName);

        enemyAmount = 0;
        playerScore = 0;
        timeRemaining = 30;
        isPlayerSpawned = false;
        isEnemySpawned = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f);

        BackToMainMenu();
    }

    private void WinGame()
    {
        if(currentGameState == GameState.Playing)
        {
            if (enemyAmount != 0 && playerScore >= enemyAmount)
            {
                currentGameState = GameState.Win;

                StartCoroutine(RestartGame());
            }
        }
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
