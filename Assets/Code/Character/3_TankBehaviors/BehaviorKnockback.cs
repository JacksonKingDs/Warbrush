using UnityEngine;
using System.Collections;

public class BehaviorKnockback : BehaviorBase
{
    BGTextureManager painter;
    int index;
    Transform[] skidTrans;
    bool isSpaceMode;

    //Ctor
    public BehaviorKnockback(TankControllerBase tank, TankStateBase stateBase) : base(tank, stateBase)
    {
        painter = BGTextureManager.instance;
        index = tank.index;
        skidTrans = tank.skidPoints;
        isSpaceMode = GM.gameMode == GameMode.Hanabi;
    }

    #region Base class functions
    public override void OnBehaviorEntry()
    {
        tank.storedVel = tank.knockbackDir * tank.knockbackForce * 1.5f;
    }

    public override void OnUpdate()
    {}

    public override void OnFixedUpdate()
    {
        if (tank.knockbackTimer > 0f)
        {
            tank.knockbackTimer -= Time.deltaTime;
            tank.rb.velocity = tank.knockbackDir * tank.knockbackForce;
            if (!isSpaceMode)
            {
                foreach (var i in skidTrans)
                {
                    painter.PaintSkid(i.position, index);
                }
            }
        }
        else
        {
            tank.GoToNormalState();
        }
    }

    public override void OnBehaviorExit()
    {
    }
    #endregion
}