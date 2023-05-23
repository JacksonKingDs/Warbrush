using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBarReposition : MonoBehaviour 
{
    #region Fields
    public Camera cam;
    public float startingX = -887.6f;
    public float moveXMod;
    float desiredAspect = 1.777778f;
    

    RectTransform trans;
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
        trans = GetComponent<RectTransform>();
	}
	
	void Update () 
	{
        if (desiredAspect != cam.aspect)
        {
            float scaleAmount = desiredAspect / cam.aspect;
            Vector3 pos = trans.anchoredPosition;
            pos.x = startingX + scaleAmount * moveXMod;
            trans.anchoredPosition = pos;
        }
    }
    #endregion

    #region Methods
    #endregion

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "pixelWidth: " + cam.pixelWidth);
        GUI.Label(new Rect(20, 40, 200, 20), "aspect: " + cam.aspect);
    }
}
