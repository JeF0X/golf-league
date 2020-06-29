using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] Ball ballPrefab = null;

    int currentBallIndex = 0;
    public string playerName;
    User user;  
    public Team team;

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

    public void SetPlayerInfo(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        balls = CreatePlayerBalls(playerInfo);
        playerName = playerInfo.playerName;
    }

    private List<Ball> CreatePlayerBalls(PlayerInfo player)
    {
        List<Ball> playerBalls = new List<Ball>();
        Vector3 debugBallPos = MatchManager.Instance.debugBallStartPos.position;
        foreach (var ballInfo in player.balls)
        {
            Debug.Log(debugBallPos);

            Ball ball = Instantiate(ballPrefab, debugBallPos, Quaternion.identity);
            debugBallPos.z += 1f;
            
            ball.SetInfo(ballInfo);     
            playerBalls.Add(ball);  
        }
        return playerBalls;
    }

    private void Hole_OnHoleEntered(Ball ball)
    {
        if (balls.Contains(ball))
        {
            Goal newGoal = new Goal(Time.time, this, ball.shotsTaken);
            ball.isInHole = true;
            //FindObjectOfType<Score>().AddGoal(newGoal);
            team.IncrementScore();
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

    private void OnDestroy()
    {
        Hole.OnHoleEntered -= Hole_OnHoleEntered;
    }
}