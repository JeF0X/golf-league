using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTouchHandler
{
    float touchWorldTravel;
    Ball currentBall;
    Vector3 touchInSpawnPlane;

    public void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);



            if (touch.phase == TouchPhase.Began)
            {
                CheckIfTouchHitPlayerBall(touch);
            }

            if (currentBall != null)
            {
                StartArea startArea = MatchManager.Instance.GetCurrentPlayer().startArea;
                touchInSpawnPlane = MapScreenTouchToWorld(touch, startArea.transform.position);
                touchWorldTravel = Vector3.Distance(startArea.transform.position, touchInSpawnPlane);
                var collider = startArea.GetComponentInChildren<Collider>();
                Vector3 closestPointToArea = collider.ClosestPointOnBounds(touchInSpawnPlane);
                currentBall.transform.position = closestPointToArea + new Vector3(0f, .5f, 0f);
            }

            if (touch.phase == TouchPhase.Ended && currentBall != null)
            {
                currentBall.SetStartPosition();
                currentBall = null;
            }

        }
    }

    public Vector3 MapScreenTouchToWorld(Touch touch, Vector3 objectPos)
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
