using UnityEngine;
using System;
using System.Collections.Generic;
using XInputDotNetPure;

#region Input Classes
//[System.Serializable]
public class ControlScheme
{
    public float MoveX;
    public float MoveY;

    public bool A_BtnDown;
    public bool A_Btn;
    public bool A_BtnUp;

    public bool B_BtnDown;
    public bool B_Btn;
    public bool B_BtnUp;

    public bool StartDown;
    public bool XboxXDown;
}
#endregion

/*    
    This class knows nothing about other classes. 
    It detects and maps analog inputs (AnalogJoypad) to ControlScheme, and other classes access this. 
*/

public class InputManager : MonoBehaviour
{
    #region Fields
    //Singleton
    public static InputManager Instance;
    
    [HideInInspector] public bool AnyStart_Down;
    [HideInInspector] public bool AnyXboxX_Down;
    [HideInInspector] public bool AnyA_Down;
    [HideInInspector] public bool AnyB_Down;
    [HideInInspector] public bool AnyLeft_Down;
    [HideInInspector] public bool AnyRight_Down;
    [HideInInspector] public bool AnyUp_Down;
    [HideInInspector] public bool AnyDown_Down;

    [HideInInspector] public bool AnyLB_Down;
    [HideInInspector] public bool AnyRB_Down;
    [HideInInspector] public bool AnyLT_Down;
    [HideInInspector] public bool AnyRT_Down;

    //Control Scheme, accessed by external classes
    [HideInInspector]
    public ControlScheme[] playerInputs = 
    {
        new ControlScheme(),new ControlScheme(),new ControlScheme(),new ControlScheme()
    };

    GamePadState[] xInput_Cur = new GamePadState[4];
    GamePadState[] xInput_Prv = new GamePadState[4];

    //XInput
    GamePadState state;
    GamePadState prevState;

    //INPUT SUBSTATE
    //Axis once-down
    [HideInInspector] public bool[] leftBtn_OnceDown;
    [HideInInspector] public bool[] rightBtn_OnceDown;
    [HideInInspector] public bool[] upBtn_OnceDown;
    [HideInInspector] public bool[] downBtn_OnceDown;
    [HideInInspector] bool[] startBtn_OnceDown;
    [HideInInspector] bool[] LT_OnceDown;
    [HideInInspector] bool[] RT_OnceDown;

    [HideInInspector] bool[] leftReset;
    [HideInInspector] bool[] rightReset;
    [HideInInspector] bool[] upReset;
    [HideInInspector] bool[] downReset;
    [HideInInspector] bool[] LT_Reset;
    [HideInInspector] bool[] RT_Reset;

    [HideInInspector] public static KeyCode p1_leftKey;
    [HideInInspector] public static KeyCode p1_rightKey;
    [HideInInspector] public static KeyCode p1_upKey;
    [HideInInspector] public static KeyCode p1_downKey;
    [HideInInspector] public static KeyCode p1_aKey;
    [HideInInspector] public static KeyCode p1_bKey;

    [HideInInspector] public static KeyCode p2_leftKey;
    [HideInInspector] public static KeyCode p2_rightKey;
    [HideInInspector] public static KeyCode p2_upKey;
    [HideInInspector] public static KeyCode p2_downKey;
    [HideInInspector] public static KeyCode p2_aKey;
    [HideInInspector] public static KeyCode p2_bKey;

    [HideInInspector] public static KeyCode p3_leftKey;
    [HideInInspector] public static KeyCode p3_rightKey;
    [HideInInspector] public static KeyCode p3_upKey;
    [HideInInspector] public static KeyCode p3_downKey;
    [HideInInspector] public static KeyCode p3_aKey;
    [HideInInspector] public static KeyCode p3_bKey;

    [HideInInspector] public static KeyCode p4_leftKey;
    [HideInInspector] public static KeyCode p4_rightKey;
    [HideInInspector] public static KeyCode p4_upKey;
    [HideInInspector] public static KeyCode p4_downKey;
    [HideInInspector] public static KeyCode p4_aKey;
    [HideInInspector] public static KeyCode p4_bKey;

    #endregion

    //void OnGUI()
    //{
      
        //GUI.Label(new Rect(20 + 300, 600, 300, 20), "DELETE THE DEBUG REMAPPER IN INPUT MANAGER");
        //GUI.Label(new Rect(20 + 300, 600, 300, 20), "Cur LB:" + (xInput_Cur[0].Buttons.LeftShoulder));
        //GUI.Label(new Rect(20 + 300, 620, 300, 20), "Prv LB:" + (xInput_Prv[0].Buttons.LeftShoulder));
        //GUI.Label(new Rect(20 + 300, 640, 300, 20), "Cur RB:" + (xInput_Cur[0].Buttons.RightShoulder));
        //GUI.Label(new Rect(20 + 300, 660, 300, 20), "Prv RB:" + (xInput_Prv[0].Buttons.RightShoulder));

        //for (int i = 0; i < 4; ++i)
        //{
        //    GUI.Label(new Rect(20 + 300 * i, 20, 300, 20), "MoveX: " + playerInputs[i].MoveX);
        //    GUI.Label(new Rect(20 + 300 * i, 40, 300, 20), "MoveY: " + playerInputs[i].MoveY);
        //    GUI.Label(new Rect(20 + 300 * i, 60, 300, 20), "A    : " + playerInputs[i].A_Btn);
        //    GUI.Label(new Rect(20 + 300 * i, 80, 300, 20), "Aup  : " + playerInputs[i].A_BtnUp);
        //    GUI.Label(new Rect(20 + 300 * i, 100, 300, 20), "Adw  : " + playerInputs[i].A_BtnDown);
        //    GUI.Label(new Rect(20 + 300 * i, 120, 300, 20), "B    : " + playerInputs[i].B_Btn);
        //    GUI.Label(new Rect(20 + 300 * i, 140, 300, 20), "Bup  : " + playerInputs[i].B_BtnUp);
        //    GUI.Label(new Rect(20 + 300 * i, 160, 300, 20), "Bdw  : " + playerInputs[i].B_BtnDown);

        //    GUI.Label(new Rect(20 + 300 * i, 200, 300, 20), "Cur LB:" + (xInput_Cur[0].Buttons.LeftShoulder));
        //    GUI.Label(new Rect(20 + 300 * i, 220, 300, 20), "Prv LB:" + (xInput_Prv[0].Buttons.LeftShoulder));
        //    GUI.Label(new Rect(20 + 300 * i, 240, 300, 20), "Cur RB:" + (xInput_Cur[0].Buttons.RightShoulder));
        //    GUI.Label(new Rect(20 + 300 * i, 260, 300, 20), "Prv RB:" + (xInput_Prv[0].Buttons.RightShoulder));
        //}
    //}

    #region Start
    void Awake()
    {
        //Singleton
        Instance = this;

        leftBtn_OnceDown = new bool[]{ false, false, false, false };
        rightBtn_OnceDown = new bool[] { false, false, false, false };
        upBtn_OnceDown = new bool[] { false, false, false, false };
        downBtn_OnceDown = new bool[] { false, false, false, false };
        startBtn_OnceDown = new bool[] { false, false, false, false };
        LT_OnceDown = new bool[] { false, false, false, false };
        RT_OnceDown = new bool[] { false, false, false, false };

        leftReset = new bool[] { true, true, true, true};
        rightReset = new bool[] { true, true, true, true};
        upReset = new bool[] { true, true, true, true};
        downReset = new bool[] { true, true, true, true};
        LT_Reset = new bool[] { true, true, true, true };
        RT_Reset = new bool[] { true, true, true, true };

        
    }

    #endregion



    void Update()
    {
        //Quick debug, delete later
        MapInputToControlScheme();

        //Directions once down
        UpdateAxisOnceDown(0);
        UpdateAxisOnceDown(1);
        UpdateAxisOnceDown(2);
        UpdateAxisOnceDown(3);
        
        DirectionAxisDown(xInput_Cur[0].Triggers.Left > 0.3f, ref LT_Reset[0], ref LT_OnceDown[0]);
        DirectionAxisDown(xInput_Cur[1].Triggers.Left > 0.3f, ref LT_Reset[1], ref LT_OnceDown[1]);
        DirectionAxisDown(xInput_Cur[2].Triggers.Left > 0.3f, ref LT_Reset[2], ref LT_OnceDown[2]);
        DirectionAxisDown(xInput_Cur[3].Triggers.Left > 0.3f, ref LT_Reset[3], ref LT_OnceDown[3]);
        DirectionAxisDown(xInput_Cur[0].Triggers.Right > 0.3f, ref RT_Reset[0], ref RT_OnceDown[0]);
        DirectionAxisDown(xInput_Cur[1].Triggers.Right > 0.3f, ref RT_Reset[1], ref RT_OnceDown[1]);
        DirectionAxisDown(xInput_Cur[2].Triggers.Right > 0.3f, ref RT_Reset[2], ref RT_OnceDown[2]);
        DirectionAxisDown(xInput_Cur[3].Triggers.Right > 0.3f, ref RT_Reset[3], ref RT_OnceDown[3]);

        //Any player direction
        AnyUp_Down    = (upBtn_OnceDown[0]   || upBtn_OnceDown[1]   || upBtn_OnceDown[2]   || upBtn_OnceDown[3]) ? true : false;
        AnyDown_Down  = (downBtn_OnceDown[0] || downBtn_OnceDown[1] || downBtn_OnceDown[2] || downBtn_OnceDown[3]) ? true : false;
        AnyLeft_Down  = (leftBtn_OnceDown[0] || leftBtn_OnceDown[1] || leftBtn_OnceDown[2] || leftBtn_OnceDown[3]) ? true : false;
        AnyRight_Down = (rightBtn_OnceDown[0]|| rightBtn_OnceDown[1]|| rightBtn_OnceDown[2]|| rightBtn_OnceDown[3]) ? true : false;
        
        AnyLT_Down = LT_OnceDown[0] || LT_OnceDown[1] || LT_OnceDown[2] || LT_OnceDown[3];
        AnyRT_Down = RT_OnceDown[0] || RT_OnceDown[1] || RT_OnceDown[2] || RT_OnceDown[3];

        //Any button
        AnyStart_Down   = playerInputs[0].StartDown     || playerInputs[1].StartDown    || playerInputs[2].StartDown    || playerInputs[3].StartDown;
        AnyA_Down       = playerInputs[0].A_BtnDown || playerInputs[1].A_BtnDown|| playerInputs[2].A_BtnDown|| playerInputs[3].A_BtnDown || Input.GetKeyDown(KeyCode.Return);
        AnyB_Down       = playerInputs[0].B_BtnDown || playerInputs[1].B_BtnDown|| playerInputs[2].B_BtnDown|| playerInputs[3].B_BtnDown;
        AnyXboxX_Down   = playerInputs[0].XboxXDown || playerInputs[1].XboxXDown || playerInputs[2].XboxXDown || playerInputs[3].XboxXDown;
    }

    #region Button OnceDown
    void UpdateAxisOnceDown(int i)
    {
        //left
        DirectionAxisDown(playerInputs[i].MoveX < -0.3f, ref leftReset[i], ref leftBtn_OnceDown[i]);

        //right
        DirectionAxisDown(playerInputs[i].MoveX > 0.3f, ref rightReset[i], ref rightBtn_OnceDown[i]);

        //up
        DirectionAxisDown(playerInputs[i].MoveY > 0.3f, ref upReset[i], ref upBtn_OnceDown[i]);

        //down
        DirectionAxisDown(playerInputs[i].MoveY < -0.3f, ref downReset[i], ref downBtn_OnceDown[i]);
    }
    

    void DirectionAxisDown(bool condition, ref bool btnHasReset, ref bool btnIsDown)
    {
        if (condition)
        {
            if (btnHasReset)
            {
                btnHasReset = false;
                btnIsDown = true;
            }
            else
            {
                btnIsDown = false;
            }
        }
        else
        {
            btnHasReset = true;
            btnIsDown = false;
        }
    }
    #endregion

    #region MapInputToControlScheme
    int xMod1, xMod2, xMod3, xMod4;
    int yMod1, yMod2, yMod3, yMod4;
    void MapInputToControlScheme() //Map raw input
    {
        //Update GamePad input state
        for (int i = 0; i < 4; ++i)
        {
            xInput_Cur[i] = GamePad.GetState((PlayerIndex)i);
        }

        //P1
        //Movement
        if (xInput_Cur[0].DPad.Left == ButtonState.Pressed || Input.GetKey(p1_leftKey))
        {
            xMod1 = -1;
        }
        else if (xInput_Cur[0].DPad.Right == ButtonState.Pressed || Input.GetKey(p1_rightKey))
        {
            xMod1 = 1;
        }
        else
        {
            xMod1 = 0;
        }

        if (xInput_Cur[0].DPad.Down == ButtonState.Pressed || Input.GetKey(p1_downKey))
        {
            yMod1 = -1;
        }
        else if (xInput_Cur[0].DPad.Up == ButtonState.Pressed || Input.GetKey(p1_upKey))
        {
            yMod1 = 1;
        }
        else
        {
            yMod1 = 0;
        }

        playerInputs[0].MoveX       = xInput_Cur[0].ThumbSticks.Left.X + xMod1;
        playerInputs[0].MoveY       = xInput_Cur[0].ThumbSticks.Left.Y + yMod1;

        //Buttons
        playerInputs[0].A_Btn       = xInput_Cur[0].Buttons.A == ButtonState.Pressed || Input.GetKey(p1_aKey);
        playerInputs[0].A_BtnUp     = (xInput_Cur[0].Buttons.A == ButtonState.Released && xInput_Prv[0].Buttons.A == ButtonState.Pressed) || Input.GetKeyUp(p1_aKey);
        playerInputs[0].A_BtnDown   = (xInput_Cur[0].Buttons.A == ButtonState.Pressed && 
                                       xInput_Prv[0].Buttons.A == ButtonState.Released) || Input.GetKeyDown(p1_aKey);

        playerInputs[0].B_Btn       = xInput_Cur[0].Buttons.B == ButtonState.Pressed || Input.GetKey(p1_bKey);
        playerInputs[0].B_BtnUp     = (xInput_Cur[0].Buttons.B == ButtonState.Released && xInput_Prv[0].Buttons.B == ButtonState.Pressed) || Input.GetKeyUp(p1_bKey);
        playerInputs[0].B_BtnDown   = (xInput_Cur[0].Buttons.B == ButtonState.Pressed && xInput_Prv[0].Buttons.B == ButtonState.Released) || Input.GetKeyDown(p1_bKey);

        playerInputs[0].StartDown = (xInput_Cur[0].Buttons.Start == ButtonState.Pressed && xInput_Prv[0].Buttons.Start == ButtonState.Released);
        playerInputs[0].XboxXDown = xInput_Cur[0].Buttons.Guide == ButtonState.Pressed && xInput_Prv[0].Buttons.Guide == ButtonState.Released;


        //P2
        //Movement
        if (xInput_Cur[1].DPad.Left == ButtonState.Pressed || Input.GetKey(p2_leftKey))
        {
            xMod2 = -1;
        }
        else if (xInput_Cur[1].DPad.Right == ButtonState.Pressed || Input.GetKey(p2_rightKey))
        {
            xMod2 = 1;
        }
        else
        {
            xMod2 = 0;
        }

        if (xInput_Cur[1].DPad.Down == ButtonState.Pressed || Input.GetKey(p2_downKey))
        {
            yMod2 = -1;
        }
        else if (xInput_Cur[1].DPad.Up == ButtonState.Pressed || Input.GetKey(p2_upKey))
        {
            yMod2 = 1;
        }
        else
        {
            yMod2 = 0;
        }

        playerInputs[1].MoveX       = xInput_Cur[1].ThumbSticks.Left.X + xMod2;
        playerInputs[1].MoveY       = xInput_Cur[1].ThumbSticks.Left.Y + yMod2;

        //Buttons
        playerInputs[1].A_Btn       = xInput_Cur[1].Buttons.A  == ButtonState.Pressed || Input.GetKey(p2_aKey);
        playerInputs[1].A_BtnUp     = (xInput_Cur[1].Buttons.A == ButtonState.Released  && xInput_Prv[1].Buttons.A == ButtonState.Pressed)  || Input.GetKeyUp(p2_aKey);
        playerInputs[1].A_BtnDown   = (xInput_Cur[1].Buttons.A == ButtonState.Pressed   && xInput_Prv[1].Buttons.A == ButtonState.Released) || Input.GetKeyDown(p2_aKey);

        playerInputs[1].B_Btn       = xInput_Cur[1].Buttons.B == ButtonState.Pressed                               || Input.GetKey(p2_bKey);
        playerInputs[1].B_BtnUp     = (xInput_Cur[1].Buttons.B == ButtonState.Released  && xInput_Prv[1].Buttons.B == ButtonState.Pressed)  || Input.GetKeyUp(p2_bKey);
        playerInputs[1].B_BtnDown   = (xInput_Cur[1].Buttons.B == ButtonState.Pressed && xInput_Prv[1].Buttons.B == ButtonState.Released) || Input.GetKeyDown(p2_bKey);

        playerInputs[1].StartDown = xInput_Cur[1].Buttons.Start == ButtonState.Pressed && xInput_Prv[1].Buttons.Start == ButtonState.Released;
        playerInputs[1].XboxXDown = xInput_Cur[1].Buttons.Guide == ButtonState.Pressed && xInput_Prv[1].Buttons.Guide == ButtonState.Released;

        //P3
        //Movement
        if (xInput_Cur[2].DPad.Left == ButtonState.Pressed || Input.GetKey(p3_leftKey))
        {
            xMod3 = -1;
        }
        else if (xInput_Cur[2].DPad.Right == ButtonState.Pressed || Input.GetKey(p3_rightKey))
        {
            xMod3 = 1;
        }
        else
        {
            xMod3 = 0;
        }

        if (xInput_Cur[2].DPad.Down == ButtonState.Pressed || Input.GetKey(p3_downKey))
        {
            yMod3 = -1;
        }
        else if (xInput_Cur[2].DPad.Up == ButtonState.Pressed || Input.GetKey(p3_upKey))
        {
            yMod3 = 1;
        }
        else
        {
            yMod3 = 0;
        }

        playerInputs[2].MoveX       = xInput_Cur[2].ThumbSticks.Left.X + xMod3;
        playerInputs[2].MoveY       = xInput_Cur[2].ThumbSticks.Left.Y + yMod3;

        //Buttons
        playerInputs[2].A_Btn       = xInput_Cur[2].Buttons.A == ButtonState.Pressed                               || Input.GetKey(p3_aKey);
        playerInputs[2].A_BtnUp     = (xInput_Cur[2].Buttons.A == ButtonState.Released     && xInput_Prv[2].Buttons.A == ButtonState.Pressed)  || Input.GetKeyUp(p3_aKey);
        playerInputs[2].A_BtnDown   = (xInput_Cur[2].Buttons.A == ButtonState.Pressed && xInput_Prv[2].Buttons.A == ButtonState.Released) || Input.GetKeyDown(p3_aKey);

        playerInputs[2].B_Btn       = xInput_Cur[2].Buttons.B == ButtonState.Pressed                               || Input.GetKey(p3_bKey);
        playerInputs[2].B_BtnUp     = (xInput_Cur[2].Buttons.B == ButtonState.Released     && xInput_Prv[2].Buttons.B == ButtonState.Pressed)  || Input.GetKeyUp(p3_bKey);
        playerInputs[2].B_BtnDown   = (xInput_Cur[2].Buttons.B == ButtonState.Pressed && xInput_Prv[2].Buttons.B == ButtonState.Released) || Input.GetKeyDown(p3_bKey);

        playerInputs[2].StartDown = xInput_Cur[2].Buttons.Start == ButtonState.Pressed && xInput_Prv[2].Buttons.Start == ButtonState.Released;
        playerInputs[2].XboxXDown = xInput_Cur[2].Buttons.Guide == ButtonState.Pressed && xInput_Prv[2].Buttons.Guide == ButtonState.Released;

        //P4
        //Movement
        if (xInput_Cur[3].DPad.Left == ButtonState.Pressed || Input.GetKey(p4_leftKey))
        {
            xMod4 = -1;
        }
        else if (xInput_Cur[3].DPad.Right == ButtonState.Pressed || Input.GetKey(p4_rightKey))
        {
            xMod4 = 1;
        }
        else
        {
            xMod4 = 0;
        }

        if (xInput_Cur[3].DPad.Down == ButtonState.Pressed || Input.GetKey(p4_downKey))
        {
            yMod4 = -1;
        }
        else if (xInput_Cur[3].DPad.Up == ButtonState.Pressed || Input.GetKey(p4_upKey))
        {
            yMod4 = 1;
        }
        else
        {
            yMod4 = 0;
        }

        playerInputs[3].MoveX = xInput_Cur[3].ThumbSticks.Left.X + xMod4;
        playerInputs[3].MoveY = xInput_Cur[3].ThumbSticks.Left.Y + yMod4;

        //Buttons
        playerInputs[3].A_Btn       = xInput_Cur[3].Buttons.A == ButtonState.Pressed || Input.GetKey(p4_aKey);
        playerInputs[3].A_BtnUp     = (xInput_Cur[3].Buttons.A == ButtonState.Released   && xInput_Prv[3].Buttons.A == ButtonState.Pressed)    || Input.GetKeyUp(p4_aKey);
        playerInputs[3].A_BtnDown   = (xInput_Cur[3].Buttons.A == ButtonState.Pressed && xInput_Prv[3].Buttons.A == ButtonState.Released)    || Input.GetKeyDown(p4_aKey);

        playerInputs[3].B_Btn       = xInput_Cur[3].Buttons.B == ButtonState.Pressed                               || Input.GetKey(p4_bKey);
        playerInputs[3].B_BtnUp     = (xInput_Cur[3].Buttons.B == ButtonState.Released   && xInput_Prv[3].Buttons.B == ButtonState.Pressed)    || Input.GetKeyUp(p4_bKey);
        playerInputs[3].B_BtnDown   = (xInput_Cur[3].Buttons.B == ButtonState.Pressed && xInput_Prv[3].Buttons.B == ButtonState.Released)    || Input.GetKeyDown(p4_bKey);

        playerInputs[3].StartDown = xInput_Cur[3].Buttons.Start     == ButtonState.Pressed && xInput_Prv[3].Buttons.Start == ButtonState.Released;
        playerInputs[3].XboxXDown = xInput_Cur[3].Buttons.Guide     == ButtonState.Pressed && xInput_Prv[3].Buttons.Guide == ButtonState.Released;


        //Bumper
        AnyLB_Down =
            (xInput_Cur[0].Buttons.LeftShoulder == ButtonState.Pressed && xInput_Prv[0].Buttons.LeftShoulder == ButtonState.Released) ||
            (xInput_Cur[1].Buttons.LeftShoulder == ButtonState.Pressed && xInput_Prv[1].Buttons.LeftShoulder == ButtonState.Released) ||
            (xInput_Cur[2].Buttons.LeftShoulder == ButtonState.Pressed && xInput_Prv[2].Buttons.LeftShoulder == ButtonState.Released) ||
            (xInput_Cur[3].Buttons.LeftShoulder == ButtonState.Pressed && xInput_Prv[3].Buttons.LeftShoulder == ButtonState.Released);

        AnyRB_Down =
            (xInput_Cur[0].Buttons.RightShoulder == ButtonState.Pressed && xInput_Prv[0].Buttons.RightShoulder == ButtonState.Released) ||
            (xInput_Cur[1].Buttons.RightShoulder == ButtonState.Pressed && xInput_Prv[1].Buttons.RightShoulder == ButtonState.Released) ||
            (xInput_Cur[2].Buttons.RightShoulder == ButtonState.Pressed && xInput_Prv[2].Buttons.RightShoulder == ButtonState.Released) ||
            (xInput_Cur[3].Buttons.RightShoulder == ButtonState.Pressed && xInput_Prv[3].Buttons.RightShoulder == ButtonState.Released);

        //Cache old GamePad state
        xInput_Prv[0] = xInput_Cur[0];
        xInput_Prv[1] = xInput_Cur[1];
        xInput_Prv[2] = xInput_Cur[2];
        xInput_Prv[3] = xInput_Cur[3];
    }
    #endregion
}