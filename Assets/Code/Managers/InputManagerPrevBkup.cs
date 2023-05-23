using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#region Input Classes
//[System.Serializable]
public class ControlScheme2
{
    public float MoveX;
    public float MoveY;

    public bool A_BtnDown;
    public bool A_Btn;
    public bool A_BtnUp;

    public bool B_BtnDown;
    public bool B_Btn;
    public bool B_BtnUp;

    public bool Start;
}
#endregion

/*    
    This class knows nothing about other classes. 
    It detects and maps analog inputs (AnalogJoypad) to ControlScheme, and other classes access this. 
*/

public class InputManagerBkup_ : MonoBehaviour
{
    #region Fields
    //Singleton
    public static InputManagerBkup_ Instance;
    [HideInInspector] public bool AnyStart_Down;
    [HideInInspector] public bool AnyA_Down;
    [HideInInspector] public bool AnyB_Down;
    [HideInInspector] public bool AnyLeft_Down;
    [HideInInspector] public bool AnyRight_Down;
    [HideInInspector] public bool AnyUp_Down;
    [HideInInspector] public bool AnyDown_Down;

    [HideInInspector] public bool AnyLB_Down;
    [HideInInspector] public bool AnyRB_Down;
    //[HideInInspector] public bool AnyLT_Down;
    //[HideInInspector] public bool AnyRT_Down;

    //Control Scheme Buttons
    [HideInInspector]
    public ControlScheme2[] playerInputs = 
    {
        new ControlScheme2(),new ControlScheme2(),new ControlScheme2(),new ControlScheme2()
    };

    //Axis once-down
    [HideInInspector] public bool[] leftBtn_OnceDown = { false, false, false, false };
    [HideInInspector] public bool[] rightBtn_OnceDown = { false, false, false, false };
    [HideInInspector] public bool[] upBtn_OnceDown = { false, false, false, false };
    [HideInInspector] public bool[] downBtn_OnceDown = { false, false, false, false };
    [HideInInspector] public  bool LT_OnceDown = true;
    [HideInInspector] public bool RT_OnceDown = true;

    [HideInInspector] public bool[] leftReset  = {true, true, true, true};
    [HideInInspector] public bool[] rightReset = {true, true, true, true};
    [HideInInspector] public bool[] upReset    = {true, true, true, true};
    [HideInInspector] public bool[] downReset  = {true, true, true, true};
    [HideInInspector] public bool LT_Reset = true;
    [HideInInspector] public bool RT_Reset = true;
    #endregion

    #region Start
    void Awake()
    {
        //Singleton
        Instance = this;
    }
    #endregion

    void Update()
    {
        MapInputToControlScheme();

        //Directions once down
        UpdateAxisOnceDown(0);
        UpdateAxisOnceDown(1);
        UpdateAxisOnceDown(2);
        UpdateAxisOnceDown(3);

        UpdateTriggerOnceDown();

        //Any player direction
        AnyUp_Down    = (upBtn_OnceDown[0]   || upBtn_OnceDown[1]   || upBtn_OnceDown[2]   || upBtn_OnceDown[3]) ? true : false;
        AnyDown_Down  = (downBtn_OnceDown[0] || downBtn_OnceDown[1] || downBtn_OnceDown[2] || downBtn_OnceDown[3]) ? true : false;
        AnyLeft_Down  = (leftBtn_OnceDown[0] || leftBtn_OnceDown[1] || leftBtn_OnceDown[2] || leftBtn_OnceDown[3]) ? true : false;
        AnyRight_Down = (rightBtn_OnceDown[0]|| rightBtn_OnceDown[1]|| rightBtn_OnceDown[2]|| rightBtn_OnceDown[3]) ? true : false;

        //if (AnyDown_Down == true)
            //Debug.Log("Anydown");

        //Bumper
        AnyLB_Down = Input.GetButtonDown("Joystick 1 Button 4") || Input.GetButtonDown("Joystick 2 Button 4") || Input.GetButtonDown("Joystick 3 Button 4") || Input.GetButtonDown("Joystick 4 Button 4");
        AnyRB_Down = Input.GetButtonDown("Joystick 1 Button 5") || Input.GetButtonDown("Joystick 2 Button 5") || Input.GetButtonDown("Joystick 3 Button 5") || Input.GetButtonDown("Joystick 4 Button 5");        

        //Any button
        AnyStart_Down   = playerInputs[0].Start     || playerInputs[1].Start    || playerInputs[2].Start    || playerInputs[3].Start;
        AnyA_Down       = playerInputs[0].A_BtnDown || playerInputs[1].A_BtnDown|| playerInputs[2].A_BtnDown|| playerInputs[3].A_BtnDown || Input.GetKeyDown(KeyCode.Return);
        AnyB_Down       = playerInputs[0].B_BtnDown || playerInputs[1].B_BtnDown|| playerInputs[2].B_BtnDown|| playerInputs[3].B_BtnDown;
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

    void UpdateTriggerOnceDown ()
    {
        if (Input.GetAxisRaw("Joysticks LT") > 0.3f)
        {
            if (LT_Reset)
            {
                LT_Reset = false;
                LT_OnceDown = true;
            }
            else
            {
                LT_OnceDown = false;
            }
        }
        else
        {
            LT_Reset = true;
            LT_OnceDown = false;
        }

        if (Input.GetAxisRaw("Joysticks RT") > 0.3f)
        {
            if (RT_Reset)
            {
                RT_Reset = false;
                RT_OnceDown = true;
            }
            else
            {
                RT_OnceDown = false;
            }
        }
        else
        {
            RT_Reset = true;
            RT_OnceDown = false;
        }
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
    void MapInputToControlScheme()
    {
        //Keyboard to input - Player 1
        playerInputs[0].MoveX       = Input.GetAxisRaw("Horizontal1") + Input.GetAxisRaw("Joystick 1 axis X");
        playerInputs[0].MoveY       = Input.GetAxisRaw("Vertical1") + Input.GetAxisRaw("Joystick 1 axis Y");
        


        playerInputs[0].A_Btn       = Input.GetButton("Joystick 1 Button 0") || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.Z);
        playerInputs[0].A_BtnUp     = Input.GetButtonUp("Joystick 1 Button 0") || Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.Z);
        playerInputs[0].A_BtnDown   = Input.GetButtonDown("Joystick 1 Button 0") || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z);

        playerInputs[0].B_Btn       = Input.GetButton("Joystick 1 Button 1") || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.X);
        playerInputs[0].B_BtnUp     = Input.GetButtonUp("Joystick 1 Button 1") || Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.X);
        playerInputs[0].B_BtnDown   = Input.GetButtonDown("Joystick 1 Button 1") || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X);

        playerInputs[0].Start       =  Input.GetButtonDown("Joystick 1 Button 7") || Input.GetKeyDown(KeyCode.Escape);

        //Keyboard to input - Player 2
        playerInputs[1].MoveX       = Input.GetAxisRaw("Horizontal2") + Input.GetAxisRaw("Joystick 2 axis X");
        playerInputs[1].MoveY       = Input.GetAxisRaw("Vertical2") + Input.GetAxisRaw("Joystick 2 axis Y");

        playerInputs[1].A_Btn       = Input.GetButton("Joystick 2 Button 0") || Input.GetKey(KeyCode.RightShift);
        playerInputs[1].A_BtnUp     = Input.GetButtonUp("Joystick 2 Button 0") || Input.GetKeyUp(KeyCode.RightShift);
        playerInputs[1].A_BtnDown   = Input.GetButtonDown("Joystick 2 Button 0") || Input.GetKeyDown(KeyCode.RightShift);
         
        playerInputs[1].B_Btn       = Input.GetButton("Joystick 2 Button 1") || Input.GetKey(KeyCode.RightControl);
        playerInputs[1].B_BtnUp     = Input.GetButtonUp("Joystick 2 Button 1") || Input.GetKeyUp(KeyCode.RightControl);
        playerInputs[1].B_BtnDown   = Input.GetButtonDown("Joystick 2 Button 1") || Input.GetKeyDown(KeyCode.RightControl);
         

        //Analogue controller to input - Player 3
        playerInputs[2].MoveX       = Input.GetAxisRaw("Joystick 3 axis X")+ Input.GetAxisRaw("Horizontal3");
        playerInputs[2].MoveY       = Input.GetAxisRaw("Joystick 3 axis Y") + Input.GetAxisRaw("Vertical3");

        playerInputs[2].A_Btn       = Input.GetButton("Joystick 3 Button 0") || Input.GetKey(KeyCode.V);
        playerInputs[2].A_BtnUp     = Input.GetButtonUp("Joystick 3 Button 0") || Input.GetKeyUp(KeyCode.V);
        playerInputs[2].A_BtnDown   = Input.GetButtonDown("Joystick 3 Button 0") || Input.GetKeyDown(KeyCode.V);

        playerInputs[2].B_Btn       = Input.GetButton("Joystick 3 Button 1") || Input.GetKey(KeyCode.B);
        playerInputs[2].B_BtnUp     = Input.GetButtonUp("Joystick 3 Button 1") || Input.GetKeyUp(KeyCode.B);
        playerInputs[2].B_BtnDown   = Input.GetButtonDown("Joystick 3 Button 1") || Input.GetKeyDown(KeyCode.B);

        playerInputs[2].Start       = Input.GetButtonDown("Joystick 3 Button 7");

        //Analogue controller to input - Player 4
        playerInputs[3].MoveX       = Input.GetAxisRaw("Joystick 4 axis X") + Input.GetAxisRaw("Horizontal4");
        playerInputs[3].MoveY       = Input.GetAxisRaw("Joystick 4 axis Y") + Input.GetAxisRaw("Vertical4");

        playerInputs[3].A_Btn       = Input.GetButton("Joystick 4 Button 0") || Input.GetKey(KeyCode.KeypadPlus);
        playerInputs[3].A_BtnUp     = Input.GetButtonUp("Joystick 4 Button 0") || Input.GetKeyUp(KeyCode.KeypadPlus);
        playerInputs[3].A_BtnDown   = Input.GetButtonDown("Joystick 4 Button 0") || Input.GetKeyDown(KeyCode.KeypadPlus);

        playerInputs[3].B_Btn       = Input.GetButton("Joystick 4 Button 1") || Input.GetKey(KeyCode.KeypadEnter);
        playerInputs[3].B_BtnUp     = Input.GetButtonUp("Joystick 4 Button 1") || Input.GetKeyUp(KeyCode.KeypadEnter);
        playerInputs[3].B_BtnDown   = Input.GetButtonDown("Joystick 4 Button 1") || Input.GetKeyDown(KeyCode.KeypadEnter);

        playerInputs[3].Start       = Input.GetButtonDown("Joystick 4 Button 7");
    }
    #endregion
}


#region archive
//	#region Test if all keys are working correctly
//		void _TestInputPrintToConsole ()
//		{
//			if (p1.LStickX 	> 0 	|| p1.LStickX 	< 0)print("p1 LaxisX");
//			if (p1.LStickY 	> 0 	|| p1.LStickY 	< 0)print("p1 LaxisY");
//			if (p1.RStickX 	> 0 	|| p1.RStickX 	< 0)print("p1 RaxisX");
//			if (p1.RStickY 	> 0 	|| p1.RStickY 	< 0)print("p1 RaxisY");
//			if (p1.Triggers > 0 	|| p1.Triggers 	< 0)print("p1 Triggers");
//			if (p1.DPadX 	> 0		|| p1.DPadX 	< 0)print("p1 DPadX");
//			if (p1.DPadY 	> 0 	|| p1.DPadY 	< 0)print("p1 DPadY");
//			if (p1.Button0)								print("p1 button0");
//			if (p1.Button1)								print("p1 button1");
//			if (p1.Button2) 							print("p1 button2");
//			if (p1.Button3) 							print("p1 button3");
//			if (p1.Button4) 							print("p1 button4");
//			if (p1.Button5) 							print("p1 button5");
//			if (p1.Button6) 							print("p1 button6");
//			if (p1.Button7) 							print("p1 button7");
//	}
//			
//			if (p2.LStickX 	> 0 	|| p2.LStickX 	< 0)print("p2 LaxisX");
//			if (p2.LStickY 	> 0 	|| p2.LStickY 	< 0)print("p2 LaxisY");
//			if (p2.RStickX 	> 0 	|| p2.RStickX 	< 0)print("p2 RaxisX");
//			if (p2.RStickY 	> 0 	|| p2.RStickY 	< 0)print("p2 RaxisY");
//			if (p2.Triggers > 0 	|| p2.Triggers 	< 0)print("p2 Triggers");
//			if (p2.DPadX 	> 0 	|| p2.DPadX 	< 0)print("p2 DPadX");
//			if (p2.DPadY 	> 0		|| p2.DPadY 	< 0)print("p2 DPadY");
//			if (p2.Button0) 							print("p2 button0");
//			if (p2.Button1)								print("p2 button1");
//			if (p2.Button2) 							print("p2 button2");
//			if (p2.Button3) 							print("p2 button3");
//			if (p2.Button4) 							print("p2 button4");
//			if (p2.Button5) 							print("p2 button5");
//			if (p2.Button6) 							print("p2 button6");
//			if (p2.Button7) 							print("p2 button7");
//			
//			if (p3.LStickX 	> 0 	|| p3.LStickX 	< 0)print("p3 LaxisX");
//			if (p3.LStickY 	> 0		|| p3.LStickY 	< 0)print("p3 LaxisY");
//			if (p3.RStickX 	> 0		|| p3.RStickX 	< 0)print("p3 RaxisX");
//			if (p3.RStickY 	> 0 	|| p3.RStickY 	< 0)print("p3 RaxisY");
//			if (p3.Triggers > 0 	|| p3.Triggers 	< 0)print("p3 Triggers");
//			if (p3.DPadX 	> 0 	|| p3.DPadX 	< 0)print("p3 DPadX");
//			if (p3.DPadY 	> 0 	|| p3.DPadY 	< 0)print("p3 DPadY");
//			if (p3.Button0) 							print("p3 button0");
//			if (p3.Button1)								print("p3 button1");
//			if (p3.Button2) 							print("p3 button2");
//			if (p3.Button3) 							print("p3 button3");
//			if (p3.Button4) 							print("p3 button4");
//			if (p3.Button5) 							print("p3 button5");
//			if (p3.Button6) 							print("p3 button6");
//			if (p3.Button7) 							print("p3 button7");
//			
//			if (p4.LStickX 	> 0 	|| p4.LStickX 	< 0)print("p4 LaxisX");
//			if (p4.LStickY 	> 0 	|| p4.LStickY 	< 0)print("p4 LaxisY");
//			if (p4.RStickX 	> 0 	|| p4.RStickX 	< 0)print("p4 RaxisX");
//			if (p4.RStickY 	> 0 	|| p4.RStickY 	< 0)print("p4 RaxisY");
//			if (p4.Triggers > 0 	|| p4.Triggers 	< 0)print("p4 Triggers");
//			if (p4.DPadX 	> 0 	|| p4.DPadX 	< 0)print("p4 DPadX");
//			if (p4.DPadY 	> 0 	|| p4.DPadY 	< 0)print("p4 DPadY");
//			if (p4.Button0) 							print("p4 button0");
//			if (p4.Button1)								print("p4 button1");
//			if (p4.Button2) 							print("p4 button2");
//			if (p4.Button3) 							print("p4 button3");
//			if (p4.Button4) 							print("p4 button4");
//			if (p4.Button5) 							print("p4 button5");
//			if (p4.Button6) 							print("p4 button6");
//			if (p4.Button7) 							print("p4 button7");
//
//		}
//	#endregion

#endregion
