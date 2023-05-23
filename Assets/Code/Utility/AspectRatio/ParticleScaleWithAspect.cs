using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaleWithAspect : MonoBehaviour 
{
    #region Fields
    ParticleSystem pfx;
    float desiredWidth = 1920f;
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
        //Debug.Log(Screen.width);
        if (Screen.width != desiredWidth)
        {
            pfx = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = pfx.main;
            main.startSize = main.startSize.constant * (Screen.width / desiredWidth);
        }
	}
	
	//void OnGUI () 
	//{
 //       GUI.Label(new Rect(20, 20, 200, 20), "Screen.width " + Screen.width);
	//}
	#endregion
	
	#region Methods
	#endregion


}
