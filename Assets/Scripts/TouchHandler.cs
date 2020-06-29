using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler
{

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
}
