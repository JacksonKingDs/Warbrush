using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Move and shoot
public class TankStateKnockback : TankStateBase
{
    #region Field
    //Behavior modules
    BehaviorKnockback behaviorKnockback;
    BehaviorRotation behaviorRotation;
    Vector3 pos;
    #endregion

    #region Constructor
    public TankStateKnockback(TankControllerBase tank) : base(tank)
    {
        //Reference behavior modules
        behaviorKnockback = new BehaviorKnockback(tank, this);
        //behaviorRotation = new BehaviorRotation(tank, this);
    }
    #endregion

    #region Base class functions
    public override void OnUpdate()
    {
        behaviorKnockback.OnUpdate();
        //behaviorRotation.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        behaviorKnockback.OnFixedUpdate();
        //behaviorRotation.OnFixedUpdate();
    }

    public override void OnStateEntry()
    {
        behaviorKnockback.OnBehaviorEntry();
        //behaviorRotation.OnBehaviorEntry();
    }

    public override void OnStateExit()
    {
        //if (tank.index == 0)
        //    Debug.Log("Exit KB");
        behaviorKnockback.OnBehaviorExit();
        //behaviorRotation.OnBehaviorExit();
    }
    #endregion
}