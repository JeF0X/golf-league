using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float shootingForce = 10f;

    public void ShootBall(Vector3 touchInBallPlane, float touchWorldTravel)
    {
        Ball ball = GetComponent<Ball>();
        Vector3 shootDirection = Vector3.Normalize(touchInBallPlane - ball.transform.position);
        ball.GetComponent<Rigidbody>().AddForce(-shootDirection * touchWorldTravel * shootingForce);
        ball.shotsTaken++;
        ball.line.gameObject.SetActive(false);
        MatchManager.Instance.SetState(new ShotInProgress(MatchManager.Instance));
    }

    public void DrawForceMeterLine(Vector3 touchInBallPlane)
    {
        Ball ball = GetComponent<Ball>();
        Vector3 lineVector = ball.transform.position + (touchInBallPlane - ball.transform.position) * -1f;
        ball.line.gameObject.SetActive(true);
        ball.line.SetPosition(0, ball.transform.position);
        ball.line.SetPosition(1, lineVector);
    }
}
