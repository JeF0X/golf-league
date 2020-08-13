using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] Ball ballPrefab = null;

    public int shots = 0;
    public int goals = 0;
    public PlayerScore playerScore;
    int currentBallIndex = 0;
    Ball currentBall;
    public StartArea startArea;
    public string playerName;
    User user;  
    public Team team;
    public Color color;
    public bool hasPlacedBalls = false;

    internal void AddShot()
    {
        shots++;
        playerScore.UpdateScore();
    }

    public void AddGoal()
    {
        goals++;
    }

    public List<Ball> balls = new List<Ball>();

    PlayerInfo playerInfo;

    public Player(User user, string name, List<Ball> balls)
    {
        this.user = user;
        this.playerName = name;
        this.balls = balls;
    }

    private void Start()
    {
        Hole.OnHoleEntered += Hole_OnHoleEntered;
        if (team == null)
        {
            team = new Team();
        }
    }

    public void SetPlayerInfo(PlayerInfo playerInfo, StartArea startArea, PlayerScore playerScore)
    {
        this.playerInfo = playerInfo;
        this.startArea = startArea;
        this.playerScore = playerScore;
        this.playerScore.SetPlayerName(playerInfo.playerName);
        balls = CreatePlayerBalls(playerInfo);
        playerName = playerInfo.playerName;
        color = playerInfo.playerColor;
        this.playerScore.SetColor(color + new Color(0f,0f,0f,0.3f));
    }

    public void SetStartArea(StartArea startArea)
    {
        this.startArea = startArea;
    }

    private List<Ball> CreatePlayerBalls(PlayerInfo player)
    {
        List<Ball> playerBalls = new List<Ball>();
        foreach (var ballInfo in player.balls)
        {
            Ball ball = ballPrefab;
            
            ball.SetInfo(ballInfo);     
            playerBalls.Add(ball);  
        }
        currentBall = balls[0];
        return playerBalls;
    }

    private void Hole_OnHoleEntered(Ball ball, Hole hole)
    {
        if (!balls.Contains(ball))
        {
            return;
        }
        if (hole == startArea.hole && MatchManager.Instance.gameType == GameType.GolfLeague)
        {
            ball.RespawnBall();
            return;
        }
        if (balls.Contains(ball) && MatchManager.Instance.gameType == GameType.GolfLeague)
        {
            ball.isInHole = true;
            ball.isMoving = false;
            //FindObjectOfType<Score>().AddGoal(newGoal);
            AddGoal();
            playerScore.UpdateScore();
            ball.RespawnToStart();
            Debug.Log(team.teamName + ": Scored a hole in " + ball.shotsTaken + " shots. They now have " + team.score + " goals.");
            return;
        }

        if (MatchManager.Instance.gameType == GameType.MiniGolf)
        {
            if (ball.isInHole)
            {
                return;
            }
            ball.isInHole = true;
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            ball.isMoving = false;
            ball.gameObject.SetActive(false);
            MatchManager.Instance.SetState(new PlayerTurn(MatchManager.Instance));
        }
    }

    public bool AreAllBallsInHole()
    {
        foreach (var ball in balls)
        {
            if (!ball.isInHole)
            {
                return false;
            }
        }
        return true;
    }

    public void ToggleCamera()
    {
        if (currentBall == null)
        {
            currentBall = balls[0];
        }
        CameraManager.Instance.SetActiveCamera(currentBall.ballCamera);
    }

    public void ChangeBall()
    {
        if (currentBall == null)
        {
            currentBall = balls[0];
        }
        Ball prevBall = currentBall;
        currentBallIndex++;
        if (currentBallIndex >= balls.Count)
        {
            currentBallIndex = 0;
        }
        currentBall = balls[currentBallIndex];

        CameraManager.Instance.SetActiveCamera(currentBall.ballCamera);
    }

    private void OnDestroy()
    {
        Hole.OnHoleEntered -= Hole_OnHoleEntered;
    }

    internal void RespawnToNextHole()
    {
        foreach (var ball in balls)
        {
            ball.gameObject.SetActive(false);
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ball.SetStartPosition();
        }
        
    }
}