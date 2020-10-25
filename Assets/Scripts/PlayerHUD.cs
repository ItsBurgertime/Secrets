using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text playerScore;
    public Text timeLeft;

    private void Update()
    {
        playerScore.text = GameManager.Instance.playerScore.ToString();

        timeLeft.text = GameManager.Instance.timeRemaining.ToString("F0");
    }
}
