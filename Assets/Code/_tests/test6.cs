using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test6 : MonoBehaviour 
{
    private void Start()
    {
        StartCoroutine(ILoveAsuka());
    }

    IEnumerator ILoveAsuka ()
    {
        while(true)
        {
            Debug.Log("Eternity");
            yield return null;
        }
    }

    private void OnGUI()
    {
        string[] temp = Input.GetJoystickNames();

        //GUI.Label(new Rect(20, 500, 1000, 20), "p1:" + temp[0]);
        //GUI.Label(new Rect(20, 520, 1000, 20), "p2:" + temp[1]);
        //GUI.Label(new Rect(20, 540, 1000, 20), "p3:" + temp[2]);
        //GUI.Label(new Rect(20, 560, 1000, 20), "p4:" + temp[3]);

        for (int i = 0; i < temp.Length; ++i)
        {
            GUI.Label(new Rect(100, 700 + 20 * i, Screen.width, Screen.height), i + "" + temp[i]);
        }
    }
}

/*
 using UnityEngine;
using XInputDotNetPure; // Required in C#

public class XInputTestCS : MonoBehaviour
{
    bool playerIndexSet = false; //Make sure gamepads are only set in the first frame
    GamePadState[] state;
    GamePadState[] prevState;
    GamePadState pState;

    void Start()
    {
        // No need to initialize anything for the plugin
        if (!pState.IsConnected)
        {
            GamePadState testState = GamePad.GetState((PlayerIndex)0);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", (PlayerIndex)0));
                playerIndexSet = true;
            }
        }

        return;
        for (int i = 0; i < 4; ++i)
        {
            if (!prevState[i].IsConnected)
            {
                GamePadState testState = GamePad.GetState((PlayerIndex)i);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", (PlayerIndex)i));
                    playerIndexSet = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        //GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet)
        {
               
        }
    }
    
    void OnGUI()
    {
        for (int i = 0; i < 4; ++i)
        {
            string text = string.Format("P{0}\n", i);
            text += string.Format("[CONNECT]\n", GamePad.GetState((PlayerIndex)i).IsConnected);
            text += string.Format("[TRIGGERS] {0} {1}\n", state[i].Triggers.Left, state[i].Triggers.Right);
            text += string.Format("[START] {0} [BACK] {1} [GUIDE] {2}\n", state[i].Buttons.Start, state[i].Buttons.Back, state[i].Buttons.Guide);
            text += string.Format("[LB] {0} [RB] {1}\n", state[i].Buttons.LeftShoulder, state[i].Buttons.RightShoulder);
            text += string.Format("[BUTTONS] [A] {0} [B] {1} [X] {2} [Y] {3}\n", state[i].Buttons.A, state[i].Buttons.B, state[i].Buttons.X, state[i].Buttons.Y);
            text += string.Format("[Stick] Left {0} {1} Right {2} {3}\n", state[i].ThumbSticks.Left.X, state[i].ThumbSticks.Left.Y, state[i].ThumbSticks.Right.X, state[i].ThumbSticks.Right.Y);
            GUI.Label(new Rect(i * 250, 0, Screen.width, Screen.height), text);
        }

        string[] temp = Input.GetJoystickNames();
        for (int i = 0; i < temp.Length; ++i)
        {
            GUI.Label(new Rect(100, 700 + 50 * i, Screen.width, Screen.height), temp[i]);
        }
    }
}
     */
