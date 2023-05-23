using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using XInputDotNetPure; // Required in C#

public class test7 : MonoBehaviour
{
    bool playerIndexSet = false; //Make sure gamepads are only set in the first frame
    GamePadState[] xInput_Cur = new GamePadState[4];
    GamePadState[] xInput_Prv = new GamePadState[4];

    KeyCode buttonA;
    KeyCode buttonB;

    void Start()
    {
        //Attempt to load default key values
        LoadKeysFromPlayerpref();
    }

    void LoadKeysFromPlayerpref ()
    {
        buttonA = (KeyCode)PlayerPrefs.GetInt("buttonA", (int)KeyCode.A);
    }

    void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        //GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(key))
                {
                    Debug.Log("keyboard" + key);
                }
            }
        }

        return;
        RemappingUpdate();
        InputUpdate();
    }

    void RemappingUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buttonA = KeyCode.Q;
            PlayerPrefs.SetInt("buttonA", (int)buttonA);
        }
            
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buttonA = KeyCode.W;
            PlayerPrefs.SetInt("buttonA", (int)buttonA);
        }
    }

    void InputUpdate()
    {
        //Standard
        if (Input.GetKeyDown(buttonA))
        {
            Debug.Log("I pressed Button A");
        }
    }

    void StandardInput()
    {
        //Standard
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Button A Down");
        }
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("Holding down button B");
        }
    }

    bool reassigningKey;
    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "Button A Keycode " + buttonA);
        GUI.Label(new Rect(20, 40, 200, 20), "Button B Keycode " + buttonB);
        if (reassigningKey)
            GUI.Label(new Rect(220, 20, 200, 20), "Reassigning... " + reassigningKey);
    }
}