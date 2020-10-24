using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject mainMenu;
    public GameObject pauseMenu;

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
