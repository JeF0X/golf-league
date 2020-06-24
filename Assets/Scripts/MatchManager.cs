using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public enum MatchState
{
    Start, PlaceBalls, PlayerTurn, Shoot, End
}

public class MatchManager : MonoBehaviour
{
    [SerializeField] Transform player1StartPosition;
    [SerializeField] Transform player2StartPosition;

    public MatchState matchState;
    private MatchState prevMatchState;
    Player[] players;
    int currentPlayerIndex = 0;
    Ball[] balls;

    private static MatchManager _instance;
    public static MatchManager Instance {  get { return _instance; } }

    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        matchState = MatchState.Start;
        players = FindObjectsOfType<Player>();
        balls = FindObjectsOfType<Ball>();
    }

    private void Update()
    {
        if (matchState == MatchState.Start)
        {
            SetCamera();
            matchState = MatchState.PlayerTurn;
        }

        if (matchState == MatchState.Shoot && !AreBallsMoving())
        {
            ChangePlayer();
            Debug.Log(players[currentPlayerIndex].playerName + " turn");
            matchState = MatchState.PlayerTurn;
        }

        if (matchState == MatchState.PlayerTurn)
        {
            if (HasMatchStateChanged())
            {
                int ballsInHole = 0;
                foreach (var ball in balls)
                {
                    if (ball.isInHole)
                    {
                        ballsInHole++;
                    }
                }

                if (ballsInHole == balls.Length)
                {
                    matchState = MatchState.End;
                    return;
                }
                Debug.Log(prevMatchState + " " + matchState);
                if (players[currentPlayerIndex].AreAllBallsInHole())
                {
                    ChangePlayer();
                    return;
                }

                SetCamera();
            } 
        }

        if (matchState == MatchState.End)
        {
            if (HasMatchStateChanged())
            {
                Debug.Log("Match Ended");
                DebugScores();
            }

        }
        prevMatchState = matchState;
    }

    private void DebugScores()
    {
        Player playerwithHighestScore = null;

        foreach (var player in players)
        {
            if (playerwithHighestScore == null)
            {
                playerwithHighestScore = player;
            }
            else if (player.team.score > playerwithHighestScore.team.score)
            {
                playerwithHighestScore = player;
            }
            else if (player.team.score == playerwithHighestScore.team.score)
            {
                Debug.Log("It's a tie with " + playerwithHighestScore.team.score + " shots.");
                return;
            }
        }
        Debug.Log(playerwithHighestScore.playerName + " wins with " + playerwithHighestScore.team.score + " shots.");
    }

    private bool HasMatchStateChanged()
    {
        if (matchState != prevMatchState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetCamera()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i == currentPlayerIndex)
            {
                players[i].ToggleCamera(true);
            }
            else
            {
                players[i].ToggleCamera(false);
            }
        }
    }

    private void ChangePlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
        }
    }

    private bool AreBallsMoving()
    {
        foreach (var ball in balls)
        {
            if (ball.isMoving)
            {
                return true;
            }
        }
        // If No ball is moving
        return false;
    }

    private void InitializeMatch()
    {

    }


    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
}
