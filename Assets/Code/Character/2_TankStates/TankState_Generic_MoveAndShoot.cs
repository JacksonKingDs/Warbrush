using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script does two things, move and shoot, bc this is what the tank does 90% of the time, so I just put them together

//Move and shoot
public class TankState_Generic_MoveAndShoot : TankStateBase
{
    #region Field
    //Behavior modules
    BehaviorMove behaviorMove;
    BehaviorRotation behaviorRotation;
    BehaviorNormalAttack behaviorChargeAttack;

    //Shooting
    Transform[] shootPoint;
    bool charging = false;    
    #endregion

    #region Constructor
    public TankState_Generic_MoveAndShoot(TankControllerBase tank) : base(tank)
    {
        behaviorMove = new BehaviorMove(tank, this);
        behaviorRotation = new BehaviorRotation(tank, this);
        behaviorChargeAttack = new BehaviorNormalAttack(tank, this);
    }
    #endregion

    #region Base class functions
    public override void OnUpdate()
    {
        behaviorMove.OnUpdate();
        behaviorRotation.OnUpdate();
        behaviorChargeAttack.OnUpdate();        
    }

    public override void OnFixedUpdate()
    {        
        behaviorMove.OnFixedUpdate();
        behaviorRotation.OnFixedUpdate();
        behaviorChargeAttack.OnFixedUpdate();
    }

    public override void OnStateEntry()
    {
        behaviorMove.OnBehaviorEntry();
        behaviorRotation.OnBehaviorEntry();
        behaviorChargeAttack.OnBehaviorEntry();
    }

    public override void OnStateExit()
    {
        behaviorMove.OnBehaviorExit();
        behaviorRotation.OnBehaviorExit();
        behaviorChargeAttack.OnBehaviorExit();
    }
    #endregion
}