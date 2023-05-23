
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalColGeo_Kinematic : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("NormalColGeo_Kinematic's OnTriggerEnter2D");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("NormalColGeo_Kinematic's OnCollisionEnter2D");
    }
}
