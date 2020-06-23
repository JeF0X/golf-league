using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterVisualizer : MonoBehaviour
{
    Touch initialTouch;
    LineRenderer line;
    Ball ball;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        ball = GetComponentInParent<Ball>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                initialTouch = touch;
               
            }
            Vector3 touchPosFarPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane);
            Vector3 touchPosNearPlane = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
            Vector3 touchPosFar = Camera.main.ScreenToWorldPoint(touchPosFarPlane);
            Vector3 touchPosNear = Camera.main.ScreenToWorldPoint(touchPosNearPlane);
            Debug.DrawRay(touchPosNear, touchPosFar - touchPosNear, Color.white, 1f);

            if (touch.phase == TouchPhase.Moved)
            {

                line.gameObject.SetActive(true);
                line.SetPosition(0, ball.transform.position);
                line.SetPosition(1, ball.transform.position + ball.transform.forward * 4f);

                //Debug.DrawLine(Camera.main.ScreenToWorldPoint(touch.position), Camera.main.ScreenToWorldPoint(initialTouch.position));
            }

            if (touch.phase == TouchPhase.Ended)
            {
                line.gameObject.SetActive(false);
            }
        }
    }
}
