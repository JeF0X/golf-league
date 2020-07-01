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
    Vector3 positionBeforeShot = new Vector3();
    Color color = Color.white;

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

    public void SetColor(Color color)
    {
        Material ballMaterial = new Material(GetComponent<MeshRenderer>().material);
        ballMaterial.color = color;
        GetComponent<MeshRenderer>().material = ballMaterial;
    }

    private void FixedUpdate()
    {
        if (MatchManager.Instance.matchState == MatchState.PlaceBalls)
        {
            myRigidBody.Sleep();
        }
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
        this.color = ballInfo.color;
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

    public void RespawnBall()
    {
        myRigidBody.Sleep();
        transform.position = positionBeforeShot;
    }

    public void SaveBallPosition()
    {
        positionBeforeShot = transform.position;
    }

}