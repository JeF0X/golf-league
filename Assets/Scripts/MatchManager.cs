using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] Transform player1StartPosition;
    [SerializeField] Transform player2StartPosition;

    Player[] players;
    Player currentPlayer;

    public enum GameState
    {

    }


    private void Start()
    {
        players = FindObjectsOfType<Player>();
    }

    private void InitializeMatch()
    {

    }


    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }
}
