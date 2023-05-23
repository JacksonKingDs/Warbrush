using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour 
{
	#region Fields
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
	}

    void Update()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        transform.position = p;

        Debug.DrawLine(Camera.main.ScreenToViewportPoint(Input.mousePosition), Camera.main.transform.position, Color.yellow);
        Debug.DrawLine(Camera.main.WorldToViewportPoint(Input.mousePosition), Camera.main.transform.position, Color.red);
        Debug.DrawLine(Camera.main.WorldToScreenPoint(Input.mousePosition), Camera.main.transform.position, Color.blue);
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.position, Color.white);
    }
    #endregion

    #region Methods
    #endregion
}
