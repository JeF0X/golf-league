using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text scoreText;

    public void SetPlayerName(string name) 
    {
        nameText.text = name;
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void UpdateScore()
    {
        if (MatchManager.Instance.gameType == GameType.MiniGolf)
        {
            scoreText.text = MatchManager.Instance.GetCurrentPlayer().shots.ToString();
        }
        else if (MatchManager.Instance.gameType == GameType.GolfLeague)
        {
            scoreText.text = MatchManager.Instance.GetCurrentPlayer().goals.ToString();
        }
        
    }

}
