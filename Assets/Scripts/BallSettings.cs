using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSettings : MonoBehaviour
{

    [SerializeField] float maxAngularVelocity = 200f;
    [SerializeField] float sleepThreshold = 0.01f;
    Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = maxAngularVelocity;
        myRigidbody.sleepThreshold = sleepThreshold;
    }
}
