using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosDrawLine : MonoBehaviour 
{
    #region Fields
    public Camera cam;
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
	}
	
	void Update () 
	{
        Vector3 screenPos = cam.ScreenToWorldPoint(Input.mousePosition);
        screenPos.z = 1f;
        Debug.DrawLine(Vector3.zero, screenPos, Color.red);
	}
	#endregion
	
	#region Methods
	#endregion
}
