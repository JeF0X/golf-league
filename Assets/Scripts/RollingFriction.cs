using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollingFriction : MonoBehaviour
{
    [SerializeField] float maxAngularVelocity = 200f;
    [SerializeField] float rollingCoefficient = 0.1f;
    [SerializeField] float sleepThreshold = 0.01f;
    float radius;
    Rigidbody myRigidbody;

    private void Start()
    {     
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = maxAngularVelocity;
        myRigidbody.WakeUp();
        myRigidbody.sleepThreshold = sleepThreshold;
        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

    private void FixedUpdate()
    {
        AddRollingFriction();
    }
    private void AddRollingFriction()
    {
        // https://forum.unity.com/threads/stop-a-rolling-sphere-on-inclined-surface.109205/
        // the rolling resistance coefficient depends on materials. I'm using ~0.05f in my tests.
        // can combine with drag too. Setting the Coefficient to around 0.08 makes the ball stop
        // in a 2-3 degree slope. 0.05 and it can stop in a 1 degree slope. Longer grass would have
        // much higher value and the ball will stop in steeper slopes.
        RaycastHit hitInfo;
        
        float mass = myRigidbody.mass;
        if (Physics.Linecast(transform.position, transform.position - new Vector3(0, radius * 2, 0), out hitInfo))
        {
            float rollingFriction = (rollingCoefficient * mass * (Physics.gravity.y * -1) * Mathf.Cos(Vector3.Angle(hitInfo.normal, Vector3.up)));
            if (rollingFriction < 0)
                rollingFriction *= -1;

            if (myRigidbody.velocity.magnitude > rollingFriction * Time.fixedDeltaTime)
            {
                var vel = new Vector3(Mathf.Abs(myRigidbody.velocity.x), Mathf.Abs(myRigidbody.velocity.y), Mathf.Abs(myRigidbody.velocity.z));
                var toOne = 1 / Mathf.Max(Mathf.Max(vel.x, vel.y), vel.z);
                myRigidbody.AddForce((myRigidbody.velocity * toOne) * -rollingFriction);
            }
            else
            {
                //myRigidbody.Sleep();
            }
        }
    }
}
