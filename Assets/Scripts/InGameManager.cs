using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    [SerializeField] Text scoreText, timerText;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelWin;
    [SerializeField] GameObject panelHud;
    [SerializeField] float sceneDuration;

    int startingEnemies;


    int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        startingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        SetScore(0);
        SetTimer(sceneDuration);
        SetGameState(GameState.Playing);
    }

    private void Update()
    {
        SetTimer(sceneDuration - Time.timeSinceLevelLoad);

        if (Time.timeSinceLevelLoad > sceneDuration && GetGameState() == GameState.Playing)
            GameOver(false);
    }

    public void AddScore(int add)
    {
        SetScore(score + add);
    }

    public void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
        if (score >= startingEnemies)
            GameOver(true);
    }

    public void SetTimer(float seconds)
    {
        if (seconds >= 0)
            timerText.text = Mathf.CeilToInt(seconds).ToString();
    }

    public void GameOver(bool win)
    {
        SetTimer(0);
        SetGameState(win ? GameState.Win : GameState.GameOver);
    }

    GameState GetGameState()
    {
        return GameManager.Instance.currentGameState;
    }

    void SetGameState(GameState state)
    {
        GameManager.Instance.currentGameState = state;

        Time.timeScale = state == GameState.Playing ? 1f : 0f;

        panelGameOver.SetActive(state == GameState.GameOver);
        panelWin.SetActive(state == GameState.Win);
        panelHud.SetActive(state == GameState.Win || state == GameState.GameOver || state == GameState.Playing || state == GameState.Paused);

    }
}
