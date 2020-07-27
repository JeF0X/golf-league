using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    [SerializeField] float maxAngularVelocity = 200f;
    [SerializeField] float rollingCoefficent = 0.01f;

    float rollingResistance = 0;
    
    Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = maxAngularVelocity;
    }

    private void FixedUpdate()
    {
        AddRollingResistanceTorque();
    }

    private float GetNormalForce()
    {
        RaycastHit hitInfo;
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        if (Physics.Linecast(transform.position, transform.position - new Vector3(0, radius * 2, 0), out hitInfo))
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, radius * 2, 0));
            float mass = myRigidbody.mass;
            float groundAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
            float normalForce = mass * (Physics.gravity.y * -1) * Mathf.Cos(groundAngle);
            if (normalForce < 0)
            {
                normalForce *= -1;
            }
            return normalForce;
        }
        else
        {
            return 0f;
        }
    }

    private void AddRollingResistanceTorque()
    {
        if (myRigidbody.angularVelocity.magnitude  <=  0)
        {
            return;
        }

        float rollingResistance = rollingCoefficent * GetNormalForce();
        float torqueForce = myRigidbody.velocity.magnitude / myRigidbody.angularVelocity.magnitude * rollingResistance;
        Debug.Log(torqueForce);
        Vector3 torque = myRigidbody.angularVelocity * -1 * torqueForce;
        myRigidbody.AddTorque(torque);
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    Vector3 temp = Vector3.Cross(collision.GetContact(0).normal, Vector3.down);
    //    Vector3 groundSlopeDir = Vector3.Cross(temp, collision.GetContact(0).normal);
    //    float angle = Vector3.Angle(collision.GetContact(0).normal, Vector3.up);

    //    float normalForce = myRigidbody.mass * Physics.gravity.y * angle;
    //    //Debug.Log(angle);
    //    rollingResistance = rollingCoefficent * normalForce;
    //    if (rollingResistance < 0)
    //    {
    //        rollingResistance *= -1;
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    rollingResistance = 0;
    //}
}
