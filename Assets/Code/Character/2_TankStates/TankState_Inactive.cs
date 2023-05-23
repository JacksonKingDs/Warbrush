using UnityEngine;
using System.Collections;

public class TankStateInactiveHidden : TankStateBase
{
    #region Field

    #endregion

    #region Constructor
    public TankStateInactiveHidden(TankControllerBase tank) : base(tank)
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
        tank.tauntM.StopTaunt_PublicHook();
    }

    public override void OnStateExit()
    {
    }
    #endregion

    #region Public

    #endregion
}