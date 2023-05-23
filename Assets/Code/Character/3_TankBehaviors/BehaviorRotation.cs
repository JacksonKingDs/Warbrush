using UnityEngine;
using System.Collections;

public class BehaviorRotation : BehaviorBase
{
    float rotationSpeed;

    //Constructor
    public BehaviorRotation(TankControllerBase tank, TankStateBase stateBase) : base(tank, stateBase)
    {
        rotationSpeed = SettingsAndPrefabRefs.TankStats[tank.modelName].RotationSpeed;
    }

    #region Base class functions
    public override void OnBehaviorEntry()
    {
        //tank.StartCoroutine(ObligatoryTaunt());
    }

    //IEnumerator ObligatoryTaunt ()
    //{
    //    yield return new WaitForSeconds(1f);
    //    tank.tauntM.Taunt();
    //}

    public override void OnUpdate()
    {
        if (tank.control.B_BtnDown)
        {
            tank.tauntM.Taunt();
        }
    }

    public override void OnFixedUpdate()
    {
        RotationUpdate();
    }

    public override void OnBehaviorExit()
    {
        tank.audio_rot.enabled = false;
        playingRotSound = false;
    }
    #endregion
    bool playingRotSound = false;
    void RotationUpdate()
    {
        float f = tank.control.MoveX;
        float f2 = tank.control.MoveY;

        //Rotation
        if (f != 0f)
        {
            tank.rb.angularVelocity = tank.control.MoveX * -rotationSpeed;
        }
        else
        {
            tank.rb.angularVelocity = 0f;
        }

        if (f != 0 && f2 == 0) //If rotating forward...
        {
            if (!playingRotSound) //... and havn't played sound yet.
            {
                tank.audio_rot.enabled = true;
                tank.audio_rot.Play();
                playingRotSound = true;
            }
        }
        else  //If not moving forward, and ...
        {
            if (playingRotSound) //...currently playing sound, then stop sound.
            {
                playingRotSound = false;
                tank.audio_rot.enabled = false;
            }
        }
    }
}