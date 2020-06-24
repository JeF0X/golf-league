using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] Ball debugBallPrefab;

    int currentBallIndex = 0;
    public string playerName;
    User user;  
    public Team team;

    public List<Ball> balls = new List<Ball>();

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

    private void DebugPlayer()
    {
        List<Ball> player1Balls = new List<Ball>();
        List<Ball> player2Balls = new List<Ball>();

        player1Balls.Add(debugBallPrefab);
        player2Balls.Add(debugBallPrefab);

        Player player1 = new Player(null, "Player1", player1Balls);
        Player player2 = new Player(null, "Player2", player2Balls);

        Instantiate(player1);
        Instantiate(player2);
    }

    private void OnDestroy()
    {
        Hole.OnHoleEntered -= Hole_OnHoleEntered;
    }
}