using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDrag : MonoBehaviour
{
    [SerializeField] float maxAngularVelocity = 200f;
    [SerializeField] float sleepThreshold = 0.2f;
    [SerializeField] float torqueForce = 0.2f;
    [SerializeField] float maxGroundBounceFactor = 0.01f;

    static int speedsQueueLenght = 10;
    Rigidbody myRigidbody;
    float maxPosition = 0;
    float groundAngle = 0f;
    float averagePositionDistance = Mathf.Infinity;
    float normalTorqueForce;
    Vector3 lastPosition = new Vector3();
    Queue<float> positionDistances = new Queue<float>(speedsQueueLenght);

    private void Start()
    {
        normalTorqueForce = torqueForce;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = maxAngularVelocity;
        myRigidbody.sleepThreshold = sleepThreshold;
        lastPosition = transform.position;
        StartCoroutine(CalcAveragePositionDifference());
    }

    private void FixedUpdate()
    {      
        if (!CheckGrounded())
        {
            Debug.Log(gameObject.name + " is not grounded");
            return;
        }

        if (groundAngle != 0 )
        {
            torqueForce = Mathf.Clamp(groundAngle / 20f, 0f, 0.25f);
        }
        else
        {
            torqueForce = normalTorqueForce;
        }

        if (myRigidbody.velocity.magnitude > 0.01f)
        {
            myRigidbody.freezeRotation = false;
        }
        else if (myRigidbody.angularVelocity.magnitude < 0.003f && groundAngle < 7f)
        {
            myRigidbody.freezeRotation = true;
        }
        
        if (averagePositionDistance < 0.0005f)
        {
            myRigidbody.Sleep();
            positionDistances.Clear();
            return;
        }

        Vector3 torque = -myRigidbody.angularVelocity.normalized * torqueForce;
        myRigidbody.AddTorque(torque);
    }

    IEnumerator CalcAveragePositionDifference()
    {
        while (true)
        {
            averagePositionDistance = AveragePosition();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool CheckGrounded()
    {
        RaycastHit hitInfo;
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        if (Physics.Linecast(transform.position, transform.position - new Vector3(0, radius * 2, 0), out hitInfo))
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, radius * 2, 0));
            float mass = myRigidbody.mass;
            groundAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
            return true;
        }
        return false;
    }

    private float AveragePosition()
    {
        float differenceInPosition = Vector3.Distance(transform.position, lastPosition);
        positionDistances.Enqueue(differenceInPosition);
        
        if (positionDistances.Count < speedsQueueLenght)
        {
            return Mathf.Infinity;
        }
        positionDistances.Dequeue();

        float avgDifferenceInPosition = 0;
        foreach (var position in positionDistances)
        {
            avgDifferenceInPosition = +position / positionDistances.Count;
            if (avgDifferenceInPosition > maxPosition)
            {
                maxPosition = avgDifferenceInPosition;
            }
        }
        //Debug.Log(avgDifferenceInPosition);
        //Debug.Log(myRigidbody.velocity.magnitude);
        lastPosition = transform.position;
        return avgDifferenceInPosition;
        
    }
}
