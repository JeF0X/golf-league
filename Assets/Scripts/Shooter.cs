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
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {        
            Touch touch = Input.GetTouch(0);
 
            if (touch.phase == TouchPhase.Began)
            {
                CheckIfTouchHitPlayerBall(touch);
            }

            if (ball != null && !ball.isMoving)
            { 
                MapScreenTouchToWorld(touch);
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
        MatchManager.Instance.matchState = MatchState.Shoot;
    }

    private void MapScreenTouchToWorld(Touch touch)
    {
        Vector3 screenTouchPosNearPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
        Vector3 touchPosNearPlane = Camera.main.ScreenToWorldPoint(screenTouchPosNearPlane);
        Vector3 screenTouchPosFarPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane);
        Vector3 touchPosFarPlane = Camera.main.ScreenToWorldPoint(screenTouchPosFarPlane);

        Ray cameraNearFarPlaneRay = new Ray(touchPosNearPlane, touchPosFarPlane - touchPosNearPlane);
        Plane ballPlane = new Plane(Vector3.up, ball.transform.position);
        float nearPlaneToBallDistance = 0;
        touchInBallPlane = new Vector3();
        if (ballPlane.Raycast(cameraNearFarPlaneRay, out nearPlaneToBallDistance))
        {
            touchInBallPlane = cameraNearFarPlaneRay.GetPoint(nearPlaneToBallDistance);
        }
        else
        {
            Debug.LogError("Cannot find ball plane");
        }

        touchWorldTravel = Vector3.Distance(ball.transform.position, touchInBallPlane);
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
