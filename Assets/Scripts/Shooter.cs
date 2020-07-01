using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float shootingForce = 10f;
    Ball ball = null;
    float touchWorldTravel;
    
    Vector3 touchInBallPlane = new Vector3();

    void Update()
    {
        HandleTouchInput();
        if (MatchManager.Instance.matchState == MatchState.ShotInProgress)
        {
            ball = null;
        }
    }

    private void HandleTouchInput()
    {
        if (MatchManager.Instance.matchState != MatchState.PlayerTurn)
        {
            return;
        }
        if (Input.touchCount > 0)
        {        
            Touch touch = Input.GetTouch(0);
 
            if (touch.phase == TouchPhase.Began)
            {
                CheckIfTouchHitPlayerBall(touch);
            }

            if (ball != null && !ball.isMoving)
            { 
                touchInBallPlane = TouchHandler.MapScreenTouchToWorld(touch, ball.transform.position);
                touchWorldTravel = Vector3.Distance(ball.transform.position, touchInBallPlane);
                DrawForceMeterLine();
            }

            if (touch.phase == TouchPhase.Ended && ball != null && !ball.isMoving)
            {
                ShootBall();
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
                ball = raycastHit.collider.GetComponent<Ball>();
                if (!IsBallCurrentPlayers())
                {
                    ball = null;
                }
            }
        }
    }

    private void ShootBall()
    {
        Vector3 shootDirection = Vector3.Normalize(touchInBallPlane - ball.transform.position);
        ball.GetComponent<Rigidbody>().AddForce(-shootDirection * touchWorldTravel * shootingForce);
        ball.shotsTaken++;
        ball.line.gameObject.SetActive(false);
        ball = null;
        MatchManager.Instance.matchState = MatchState.ShotInProgress;
    }

    private void DrawForceMeterLine()
    {
        Vector3 lineVector = ball.transform.position + (touchInBallPlane - ball.transform.position) * -1f;
        ball.line.gameObject.SetActive(true);
        ball.line.SetPosition(0, ball.transform.position);
        ball.line.SetPosition(1, lineVector);
    }

    private bool IsBallCurrentPlayers()
    {
        Player currentPlayer = MatchManager.Instance.GetCurrentPlayer();
        if (currentPlayer.balls.Contains(ball))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
