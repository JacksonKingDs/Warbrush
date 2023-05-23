using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour 
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
    void OnGUI ()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            GUI.Label(new Rect(1500, i * 20, 200, 20), i + ": " + Input.GetJoystickNames()[i]);
        }

        //P1 ================================================================
        GUI.Label(new Rect(20, 20, 200, 20), "P1 Controller");

        //Move
        GUI.Label(new Rect(20, 50, 200, 20), "Vertical: " + Input.GetAxisRaw("Joystick 1 axis X"));
        GUI.Label(new Rect(20, 70, 200, 20), "Horizontal: " + Input.GetAxisRaw("Joystick 1 axis Y"));

        //
        GUI.Label(new Rect(20, 100, 200, 20), "A Button: " + Input.GetButton("Joystick 1 Button 0"));
        GUI.Label(new Rect(20, 120, 200, 20), "B Button: " + Input.GetButton("Joystick 1 Button 1"));
        GUI.Label(new Rect(20, 140, 200, 20), "Xbox Button: " + Input.GetButton("Joystick 1 Button 15"));


        //P2 ================================================================
        GUI.Label(new Rect(220, 20, 200, 20), "P2 Controller");

        //Move
        GUI.Label(new Rect(220, 50, 200, 20), "Vertical: " + Input.GetAxisRaw("Joystick 2 axis X"));
        GUI.Label(new Rect(220, 70, 200, 20), "Horizontal: " + Input.GetAxisRaw("Joystick 2 axis Y"));

        //
        GUI.Label(new Rect(220, 100, 200, 20), "A Button: " + Input.GetButton("Joystick 2 Button 0"));
        GUI.Label(new Rect(220, 120, 200, 20), "B Button: " + Input.GetButton("Joystick 2 Button 1"));

        //P3 ================================================================
        GUI.Label(new Rect(320, 20, 200, 20), "P3 Controller");

        //Move
        GUI.Label(new Rect(320, 50, 200, 20), "Vertical: " + Input.GetAxisRaw("Joystick 3 axis X"));
        GUI.Label(new Rect(320, 70, 200, 20), "Horizontal: " + Input.GetAxisRaw("Joystick 3 axis Y"));

        //
        GUI.Label(new Rect(320, 100, 200, 20), "A Button: " + Input.GetButton("Joystick 3 Button 0"));
        GUI.Label(new Rect(320, 120, 200, 20), "B Button: " + Input.GetButton("Joystick 3 Button 1"));

        //P4 ================================================================
        GUI.Label(new Rect(420, 20, 200, 20), "P4 Controller");

        //Move
        GUI.Label(new Rect(420, 50, 200, 20), "Vertical: " + Input.GetAxisRaw("Joystick 4 axis X"));
        GUI.Label(new Rect(420, 70, 200, 20), "Horizontal: " + Input.GetAxisRaw("Joystick 4 axis Y"));

        //
        GUI.Label(new Rect(420, 100, 200, 20), "A Button: " + Input.GetButton("Joystick 4 Button 0"));
        GUI.Label(new Rect(420, 120, 200, 20), "B Button: " + Input.GetButton("Joystick 4 Button 1"));
    }


	#endregion
}