using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForceFullRect : MonoBehaviour 
{
	#region Fields
    #endregion
	
	#region MonoBehaviour    
	IEnumerator Start () 
	{
        yield return null;
        GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
	}
	
	void Update () 
	{
	}
	#endregion
	
	#region Methods
	#endregion
}
