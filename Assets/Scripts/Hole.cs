using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public static event Action<Ball, Hole> OnHoleEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (OnHoleEntered != null)
        {
            if (other.tag == "Player")
            {
                OnHoleEntered(other.gameObject.GetComponent<Ball>(), this);
            }           
        }
    }
}
