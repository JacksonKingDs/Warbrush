using UnityEngine;
using System.Collections;

public class TankStateStandby : TankStateBase
{
    #region Field
    BehaviorMove behaviorMove;
    BehaviorRotation behaviorRotation;
    #endregion

    #region Constructor
    public TankStateStandby(TankControllerBase tank) : base(tank)
    {
        behaviorMove = new BehaviorMove(tank, this);
        behaviorRotation = new BehaviorRotation(tank, this);
    }
    #endregion

    #region Base class functions
    public override void OnUpdate()
    {
        if (waitCount >0)
        {
            waitCount -= Time.deltaTime;
        }
        else
        {
            behaviorMove.OnUpdate();
        }
        
        behaviorRotation.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        if (waitCount <= 0)
            behaviorMove.OnFixedUpdate();
        behaviorRotation.OnFixedUpdate();
    }

    float waitCount = 10;

    public override void OnStateEntry()
    {
        waitCount = 0.2f;
        behaviorRotation.OnBehaviorEntry();
    }

    public override void OnStateExit()
    {
        behaviorMove.OnBehaviorExit();
        behaviorRotation.OnBehaviorExit();
    }
    #endregion
}