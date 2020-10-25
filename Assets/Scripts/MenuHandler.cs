using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject playerHUD;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    private void Update()
    {
        //showing and hiding the menus based on game state
        if (GameManager.Instance.currentGameState == GameState.Mainmenu)
        {
            mainMenu.SetActive(true);
            mainCamera.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
            mainCamera.SetActive(false);
        }

        if(GameManager.Instance.currentGameState == GameState.Playing)
        {
            playerHUD.SetActive(true);
        }
        else
        {
            playerHUD.SetActive(false);
        }

        if (GameManager.Instance.currentGameState == GameState.Win)
        {
            winPanel.SetActive(true);
        }
        else
        {
            winPanel.SetActive(false);
        }

        if (GameManager.Instance.currentGameState == GameState.GameOver)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(false);
        }

        if (GameManager.Instance.currentGameState == GameState.Paused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }
}
