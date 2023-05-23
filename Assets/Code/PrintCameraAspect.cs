using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintCameraAspect : MonoBehaviour 
{
    #region Fields
    Camera c;
    void Start ()
    {
        c = GetComponent<Camera>();
    }
    #endregion
	
	#region MonoBehaviour    

	void Update () 
	{
	}
    #endregion

    #region Methods
    #endregion

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "aspect: " + c.aspect);
        GUI.Label(new Rect(20, 40, 200, 20), "p.width: " + c.pixelWidth);
        GUI.Label(new Rect(20, 60, 200, 20), "p.height: " + c.pixelHeight);
    }
}
