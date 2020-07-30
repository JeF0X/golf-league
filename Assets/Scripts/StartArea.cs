using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartArea : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera startAreaCamera; 
    public Hole hole = null;
    StartPosition[] startPositions;
    int startPositionIndex = 0;
    bool hasInstantiatedBalls = false;


    private void Start()
    {
        startPositions = GetComponentsInChildren<StartPosition>();
    }


    public void InstantiatePlayerBalls()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach (var player in players)
        {
            List<Ball> balls = player.balls;
            for (int i = 0; i < balls.Count; i++)
            {
                if (!balls[i].hasInstantiated)
                {
                    balls[i] = Instantiate(balls[i], startPositions[startPositionIndex].transform.position + Vector3.up * 0.52f, Quaternion.identity);
                    balls[i].gameObject.SetActive(false);
                    balls[i].hasInstantiated = true;
                    balls[i].SetColor(player.color);
                }
                balls[i].SetStartPosition();
                //startPositionIndex++;
            }
            //player.hasPlacedBalls = true;
        } 
    }

    internal void ActivateCamera()
    {
        CameraManager.Instance.SetActiveCamera(startAreaCamera);
    }

    public void MoveBallsToStartArea()
    {
        Player player = MatchManager.Instance.GetCurrentPlayer();
        if (player.hasPlacedBalls)
        {
            return;
        }
        List<Ball> balls = player.balls;

        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].transform.position = startPositions[startPositionIndex].transform.position + Vector3.up * 0.52f;
            balls[i].gameObject.SetActive(true);
            balls[i].GetComponent<Rigidbody>().isKinematic = true;
            balls[i].SetColor(player.color);
            balls[i].SetStartPosition();
            balls[i].isInHole = false;
            startPositionIndex++;
        }
        player.hasPlacedBalls = true;
        CameraManager.Instance.SetActiveCamera(startAreaCamera);
    }



}
