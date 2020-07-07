using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] Ball ballPrefab = null;

    int currentBallIndex = 0;
    Ball currentBall;
    public StartArea startArea;
    public string playerName;
    User user;  
    public Team team;
    public Color color;

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

    public void SetPlayerInfo(PlayerInfo playerInfo, StartArea startArea)
    {
        this.playerInfo = playerInfo;
        this.startArea = startArea;
        balls = CreatePlayerBalls(playerInfo);
        playerName = playerInfo.playerName;
        color = playerInfo.playerColor;
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
        if (hole == startArea.hole)
        {
            ball.RespawnBall();
            return;
        }
        if (balls.Contains(ball))
        {
            Goal newGoal = new Goal(Time.time, this, ball.shotsTaken);
            ball.isInHole = true;
            //FindObjectOfType<Score>().AddGoal(newGoal);
            team.IncrementScore();
            ball.RespawnToStart();
            Debug.Log(team.teamName + ": Scored a hole in " + ball.shotsTaken + " shots. They now have " + team.score + " goals.");
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

    public void ToggleCamera(bool isCameraOn)
    {
        balls[currentBallIndex].ballCamera.enabled = isCameraOn;
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

        currentBall.ballCamera.enabled = true;
        prevBall.ballCamera.enabled = false;
    }

    private void OnDestroy()
    {
        Hole.OnHoleEntered -= Hole_OnHoleEntered;
    }
}