using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    public Hole hole = null;
    StartPosition[] startPositions;

    bool hasInstantiatedBalls = false;

    
    private void Start()
    {
        startPositions = GetComponentsInChildren<StartPosition>();
    }


    public void InstantiatePlayerBalls()
    {
        if (hasInstantiatedBalls)
        {
            return;
        }
        Player player = MatchManager.Instance.GetCurrentPlayer();
        

        List<Ball> balls = player.balls;
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i] = Instantiate(balls[i], startPositions[i].transform.position + Vector3.up*0.52f, Quaternion.identity);
            balls[i].SetColor(player.color);
            balls[i].SetStartPosition();
        }
        hasInstantiatedBalls = true;
    }


   
}
