using UnityEngine;
using System.Collections;

public abstract class BehaviorBase
{
    protected Transform trans;

    //Class reference
    protected TankControllerBase tank;
    protected TankStateBase stateBase;

    protected ControlScheme control;
    protected SettingsAndPrefabRefs refs;
    protected AudioManager audioM;
    
    public BehaviorBase(TankControllerBase tank, TankStateBase stateBase)
    {
        this.tank = tank;
        this.stateBase = stateBase;

        //Class reference
        control = tank.control;
        refs = SettingsAndPrefabRefs.instance;
        audioM = AudioManager.instance;

        //Object reference
        trans = tank.trans;
    }

    public abstract void OnBehaviorEntry();

    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();

    public abstract void OnBehaviorExit();
}
