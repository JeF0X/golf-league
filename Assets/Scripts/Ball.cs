using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] float maxAngularVelocity = 20f;

    public int shotsTaken = 0;
    public bool isMoving = false;
    public bool isInHole = false;
    float maxVelocity = 0f;

    public CinemachineVirtualCamera ballCamera;
    public LineRenderer line;
    Rigidbody myRigidBody;
    BallInfo ballInfo;

    private void Start()
    {
        ballCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        ballCamera.enabled = false;
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.maxAngularVelocity = maxAngularVelocity;
        line = GetComponentInChildren<LineRenderer>();
        line.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        //DebugVelocities();
        isMoving = !myRigidBody.IsSleeping();

        if (myRigidBody.velocity.magnitude < 1f && myRigidBody.velocity.magnitude > 0.0001f)
        {
            DampenAngularVelocity();
        }
    }

    internal void SetInfo(BallInfo ballInfo)
    {
        this.ballInfo = ballInfo;
    }

    public void ResetBall()
    {
        shotsTaken = 0;
    }

    private void DebugVelocities()
    {
        float[] velocities = { myRigidBody.velocity.x, myRigidBody.velocity.y, myRigidBody.velocity.z };


        if (Mathf.Max(velocities) > maxVelocity)
        {
            maxVelocity = Mathf.Max(velocities);
            Debug.Log(maxVelocity);
        }
    }

    private void DampenAngularVelocity()
    {
        myRigidBody.AddTorque(-myRigidBody.angularVelocity);
        if (myRigidBody.velocity.magnitude < 0.01f)
        {
            myRigidBody.Sleep();
        }
    }

}