using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGeo_Kinematic : MonoBehaviour 
{
	#region Fields
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
	}
	
	void Update () 
	{
	}
    #endregion

    
    #region Methods
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("TriggerGeo's OnTriggerEnter2D");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("TriggerGeo's OnCollisionEnter2D");
    }
    #endregion
}
