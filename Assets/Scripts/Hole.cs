using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public static event Action<Ball> OnHoleEntered;
    BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        if (OnHoleEntered != null)
        {
            if (other.tag == "Player")
            {
                
                OnHoleEntered(other.gameObject.GetComponent<Ball>());
            }           
        }
    }
}
