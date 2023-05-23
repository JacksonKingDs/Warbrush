using UnityEngine;
using System.Collections;

public class TankState_nullDummy : TankStateBase
{
    #region Field
    #endregion

    #region Constructor
    public TankState_nullDummy(TankControllerBase tank) : base(tank)
    {
    }
    #endregion

    #region Base class functions
    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnStateEntry()
    {
    }

    public override void OnStateExit()
    {
    }

    #endregion
}
