using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public enum MatchState
{
    Start, PlaceBalls, FindBalls, PlayerTurn, ShotInProgress, End
}

public class MatchManager : MonoBehaviour
{
    [SerializeField] float matchTimeInSeconds = 120f;

    public Transform debugBallStartPos;
    public MatchState matchState;
    private MatchState prevMatchState;
    Player[] players;
    public List<Team> teams = new List<Team>();
    int currentPlayerIndex = 0;
    Player currentPlayer = null;
    Ball[] balls;
    bool isGameInitialized = false;
    bool startTimer = false;

    public float matchTimer = 0;

    private static MatchManager _instance;
    
    public static MatchManager Instance {  get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        matchTimer = matchTimeInSeconds;
    }

    private void Update()
    {
        if (startTimer)
        {
            matchTimer -= Time.deltaTime;
        }

        if (matchTimer <= 0)
        {
            if (matchState == MatchState.PlayerTurn)
            {
                matchState = MatchState.End;
            }
            startTimer = false;
        }

        if (matchState == MatchState.Start)
        {
            InitializeMatch();
            matchState = MatchState.PlaceBalls;
        }

        if (matchState == MatchState.PlaceBalls)
        {
            
        }

        if (matchState == MatchState.FindBalls)
        {
            balls = FindObjectsOfType<Ball>();
            foreach (var ball in balls)
            {
                ball.GetComponent<Rigidbody>().WakeUp();
            }
            SaveBallPositions();
            matchState = MatchState.PlayerTurn;
        }

        if (matchState == MatchState.PlayerTurn)
        {
            startTimer = true;
            SetCamera();
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
                if (currentPlayer.AreAllBallsInHole())
                {
                    ChangePlayer();
                    return;
                }

                SetCamera();
            }
        }

        if (matchState == MatchState.ShotInProgress && !AreBallsMoving())
        {
            SaveBallPositions();
            ChangePlayer();
            Debug.Log(players[currentPlayerIndex] + ": " + players[currentPlayerIndex].playerName + " turn");
            matchState = MatchState.PlayerTurn;
        }

        if (matchState == MatchState.End)
        {
            if (HasMatchStateChanged())
            {
                Debug.Log("Match Ended");
                FindObjectOfType<CinemachineCoreGetInputTouchAxis>().GetComponent<CinemachineCoreGetInputTouchAxis>().enabled = false;
                DebugScores();
            }

        }

        prevMatchState = matchState;
    }

    public string DebugScores()
    {
        Player playerwithHighestScore = null;
        string winner;

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
                winner = "It's a tie with " + playerwithHighestScore.team.score + " goals.";
                
                return winner;
            }
        }
        winner = playerwithHighestScore.playerName + " wins with " + playerwithHighestScore.team.score + " goals.";
        return winner;
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
            if (players[i] == currentPlayer)
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
        currentPlayer = players[currentPlayerIndex];
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

    private void SaveBallPositions()
    {
        foreach (var ball in balls)
        {
            ball.SaveBallPosition();
        }
        Debug.Log("Positions saved");
    }

    private void InitializeMatch()
    {
        if (!isGameInitialized)
        {
            players = FindObjectsOfType<Player>();

            foreach (var player in players)
            {
                if (teams.Contains(player.team))
                {
                    continue;
                }
                else
                {
                    teams.Add(player.team);
                }
            }
            currentPlayer = players[0];
            //matchState = MatchState.Start;
            isGameInitialized = true;
        }
        
    }


    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void PlayerReady()
    {
        if (currentPlayer == players[players.Length-1])
        {
            matchState = MatchState.FindBalls;
        }
        else
        {
            ChangePlayer();
        }
        
    }

    public void ChangeBall()
    {
        currentPlayer.ChangeBall();
    }
}
