using UnityEngine;
using System.Collections;

//This base class only just references and to specify the public functions. 
public abstract class TankStateBase
{
    #region Field
    //Class references
    protected TankControllerBase tank;
    protected ControlScheme control;
    protected SettingsAndPrefabRefs refs;

    //Game settings
    protected TankModelNames modelName;

    //Object reference
    protected Transform trans;
    
    #endregion

    #region Constructor
    public TankStateBase(TankControllerBase tank)
    {
        this.tank = tank;

        //Reference
        control = tank.control;
        refs = SettingsAndPrefabRefs.instance;
        modelName = tank.modelName;
        trans = tank.transform;
    }
    #endregion

    #region Public interactions
    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();

    public abstract void OnStateEntry(); //For resetting the behavior, as if reinitialized the behavior.

    public abstract void OnStateExit(); //Can be call when exiting a state and need to clean up loose ends.

    // void OnDestroy() { } Used to Unsubscribe to events
    #endregion
}