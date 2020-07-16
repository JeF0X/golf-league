using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MatchManager : StateMachine
{
    [SerializeField] float matchTimeInSeconds = 120f;

    public Transform debugBallStartPos;
    Player[] players;
    public List<Team> teams = new List<Team>();
    int currentPlayerIndex = 0;
    Player currentPlayer = null;
    Ball[] balls;
    bool isGameInitialized = false;
    public bool startTimer = false;

    public TouchHandler touchHandler = new TouchHandler();
    public float matchTimer = 0;

    private static MatchManager _instance;
    
    public static MatchManager Instance {  get { return _instance; } }

    public State GetState()
    {
        return State;
    }

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
        SetState(new MatchStart(this));
    }

    private void Update()
    {
        State.Tick();
        if (startTimer)
        {
            matchTimer -= Time.deltaTime;
        }
    }

    public void FindBalls()
    {
        balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
        {
            ball.GetComponent<Rigidbody>().WakeUp();
        }
        SaveBallPositions();
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

    public void SetCamera()
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

    public void ChangePlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
        }
        currentPlayer = players[currentPlayerIndex];
    }

    public bool AreBallsMoving()
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

    public void SaveBallPositions()
    {
        foreach (var ball in balls)
        {
            ball.SaveBallPosition();
        }
        Debug.Log("Positions saved");
    }

    public void InitializeMatch()
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
                else if (player.team == null)
                {
                    player.team = new Team();
                }
                teams.Add(player.team);

            }
            currentPlayer = players[0];
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

    //Used For Buttons
    public void PlayerReady()
    {
        if (currentPlayer == players[players.Length-1])
        {
            SetState(new PlayerTurn(this));
        }
        else
        {
            ChangePlayer();
        }
        
    }
    //Used For Buttons
    public void ChangeBall()
    {
        currentPlayer.ChangeBall();
    }
}
