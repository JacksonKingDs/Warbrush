using UnityEngine;
using System.Collections;

public class TankStateInitialStandby : TankStateBase
{
    #region Field
    //BehaviorMove behaviorMove;
    BehaviorRotation behaviorRotation;
    #endregion

    #region Constructor
    public TankStateInitialStandby(TankControllerBase tank) : base(tank)
    {
        //behaviorMove = new BehaviorMove(tank, this);
        behaviorRotation = new BehaviorRotation(tank, this);
    }
    #endregion

    #region Base class functions
    public override void OnUpdate()
    {
        //if (waitTime >0)
        //{
        //    waitTime-= Time.deltaTime;
        //}
        //else
        //{
        //    behaviorMove.OnUpdate();
        //}
        
        behaviorRotation.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        //if (waitTime <= 0)
        //{
        //    behaviorMove.OnFixedUpdate();
        //}
            
        behaviorRotation.OnFixedUpdate();
    }

    float waitTime = 2.5f;

    public override void OnStateEntry()
    {
        waitTime = 2.5f;
        behaviorRotation.OnBehaviorEntry();
    }

    public override void OnStateExit()
    {
        //behaviorMove.OnBehaviorExit();
        behaviorRotation.OnBehaviorExit();
    }
    #endregion
}