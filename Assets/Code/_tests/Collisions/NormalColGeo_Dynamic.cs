
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalColGeo_Dynamic : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("NormalColGeo_Dynamic's OnTriggerEnter2D");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("NormalColGeo_Dynamic's OnCollisionEnter2D");
    }
}
