using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlacer : MonoBehaviour
{
    [SerializeField] Ball testBall;
    float touchWorldTravel;

    Ball currentBall;

    Vector3 touchInSpawnPlane = new Vector3();

    private void Start()
    {
        currentBall = testBall;
    }

    private void Update()
    {
        if (MatchManager.Instance.matchState != MatchState.PlaceBalls)
        {
            return;
        }

        HandleTouchInput();
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

    public void TeeOff()
    {
        MatchManager.Instance.matchState = MatchState.PlayerTurn;
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
            }
        }
    }
}
