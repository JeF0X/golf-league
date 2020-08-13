using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler
{

    Vector3 touchInBallPlane = new Vector3();
    float touchWorldTravel = 0f;
    Ball currentBall;

    public static Vector3 MapScreenTouchToWorld(Touch touch, Vector3 objectPos)
    {
        Vector3 screenTouchPosNearPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
        Vector3 touchPosNearPlane = Camera.main.ScreenToWorldPoint(screenTouchPosNearPlane);
        Vector3 screenTouchPosFarPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane);
        Vector3 touchPosFarPlane = Camera.main.ScreenToWorldPoint(screenTouchPosFarPlane);

        Ray cameraNearFarPlaneRay = new Ray(touchPosNearPlane, touchPosFarPlane - touchPosNearPlane);
        Plane ballPlane = new Plane(Vector3.up, objectPos);
        float nearPlaneToBallDistance = 0;
        Vector3 touchInBallPlane = new Vector3();
        if (ballPlane.Raycast(cameraNearFarPlaneRay, out nearPlaneToBallDistance))
        {
            touchInBallPlane = cameraNearFarPlaneRay.GetPoint(nearPlaneToBallDistance);
        }
        else
        {
            Debug.LogError("Cannot find ball plane");
        }
        return touchInBallPlane;
    }

    public void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                CheckIfTouchHitBall(touch);
            }

            if (currentBall != null && !currentBall.isMoving)
            {
                touchInBallPlane = MapScreenTouchToWorld(touch, currentBall.transform.position);
                touchWorldTravel = Vector3.Distance(currentBall.transform.position, touchInBallPlane);
                currentBall.GetComponent<Shooter>().DrawForceMeterLine(touchInBallPlane);
            }

            if (touch.phase == TouchPhase.Ended && currentBall != null && !currentBall.isMoving)
            {
                currentBall.GetComponent<Shooter>().ShootBall(touchInBallPlane, touchWorldTravel);
                currentBall = null;
            }
        }
    }

    private void CheckIfTouchHitBall(Touch touch)
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
