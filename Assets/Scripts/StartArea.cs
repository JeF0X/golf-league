using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    public Hole hole = null;
    StartPosition[] startPositions;


    float touchWorldTravel;

    Ball currentBall;
    bool hasInstantiatedBalls = false;

    Vector3 touchInSpawnPlane = new Vector3();
    private void Start()
    {
        startPositions = GetComponentsInChildren<StartPosition>();
    }

    private void Update()
    {
        if (MatchManager.Instance.matchState != MatchState.PlaceBalls)
        {
            return;
        }

        if (MatchManager.Instance.GetCurrentPlayer().startArea != this)
        {
            return;
        }

        if (!hasInstantiatedBalls)
        {
            InstantiatePlayerBalls();
            
        }
        

        HandleTouchInput();
    }


    private void InstantiatePlayerBalls()
    {
        Player player = MatchManager.Instance.GetCurrentPlayer();
        

        List<Ball> balls = player.balls;
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i] = Instantiate(balls[i], startPositions[i].transform.position + Vector3.up*0.52f, Quaternion.identity);
            balls[i].SetColor(player.color);
        }
        hasInstantiatedBalls = true;
    }


    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchInSpawnPlane = TouchHandler.MapScreenTouchToWorld(touch, transform.position);
            touchWorldTravel = Vector3.Distance(transform.position, touchInSpawnPlane);

            if (touch.phase == TouchPhase.Began)
            {
                CheckIfTouchHitPlayerBall(touch);
            }

            if (currentBall != null)
            {
                var collider = GetComponentInChildren<Collider>();
                Vector3 closestPointToArea = collider.ClosestPointOnBounds(touchInSpawnPlane);
                currentBall.transform.position = closestPointToArea + new Vector3(0f, .5f, 0f);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                currentBall = null;
            }

        }
    }




    private void CheckIfTouchHitPlayerBall(Touch touch)
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Physics.Raycast(ray, out raycastHit);

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.tag == "Player")
            {
                currentBall = raycastHit.collider.GetComponent<Ball>();
                if (!IsBallCurrentPlayers())
                {
                    currentBall = null;
                }
            }
        }
    }

    private bool IsBallCurrentPlayers()
    {
        Player currentPlayer = MatchManager.Instance.GetCurrentPlayer();
        if (currentPlayer.balls.Contains(currentBall))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
