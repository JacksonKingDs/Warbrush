using UnityEngine;
using System.Collections;

public class BehaviorMove : BehaviorBase
{
    //Field 
    float moveSpeed;
    Vector3 pos;
    ulong delay;

    bool isSpace = false;

    //Constructor
    public BehaviorMove(TankControllerBase tank, TankStateBase stateBase) : base(tank, stateBase)
    {
        moveSpeed = tank.moveSpeed;
        delay = (ulong)Random.Range(0, 100);
        isSpace = GM.gameMode == GameMode.Hanabi;
    }

    #region Base class functions
    public override void OnBehaviorEntry()
    {
        //canStartGlide = false;
    }

    public override void OnUpdate()
    {        
        //GlidingUpdate();
    }

    public override void OnFixedUpdate()
    {
        MovementFixedUpdate();
    }

    public override void OnBehaviorExit()
    {
        tank.audio_move.enabled = false;
        playingMoveSound = false;
    }
    #endregion

    #region Movement
    bool playingMoveSound = false;
    float spawnTrailTimer;

    void MovementFixedUpdate()
    {
        //Warping
        trans.position = TankUtil.WrapWorldPosTankPos(trans.position);

        //Movement
        if (!tank.canSlide)
        {
            float inputY = tank.control.MoveY;
            tank.rb.velocity = inputY * moveSpeed * trans.up;

            if (inputY != 0) //If moving forward...
            {
                //Spawn ocean trail or walk dust
                if (GM.gameMode == GameMode.PVP_OceanMist)
                {
                    if (inputY > 0.5f || inputY < -0.5f) //Moving forward
                    {
                        spawnTrailTimer -= Time.deltaTime;
                        if (spawnTrailTimer <= 0f)
                        {
                            float x = tank.control.MoveX;
                            spawnTrailTimer = 0.05f;
                            if (inputY > 0.5f)
                            {
                                refs.Pop_OceanLine(tank.trailPointsBack[0].position, tank.trailPointsBack[0].rotation, inputY + x * 0.5f);
                                refs.Pop_OceanLine(tank.trailPointsBack[1].position, tank.trailPointsBack[1].rotation, inputY + x * -0.5f);
                            }
                            else
                            {
                                refs.Pop_OceanLine(tank.trailPointsFront[0].position, tank.trailPointsFront[0].rotation, -inputY + x * -0.5f);
                                refs.Pop_OceanLine(tank.trailPointsFront[1].position, tank.trailPointsFront[1].rotation, -inputY + x * 0.5f);
                            }
                        }
                    }
                }
                //else //Walk dust
                //{
                //    if (inputY > 0.5f || inputY < -0.5f)
                //    {
                //        spawnTrailTimer -= Time.deltaTime;
                //        if (spawnTrailTimer <= 0f)
                //        {
                //            float x = tank.control.MoveX;
                //            spawnTrailTimer = 0.2f;
                //            if (inputY > 0.5f)
                //            {
                //                refs.Pop_Walkdust(tank.trailPointsBack[0].position);
                //                refs.Pop_Walkdust(tank.trailPointsBack[1].position);
                //            }
                //            else
                //            {
                //                refs.Pop_Walkdust(tank.trailPointsFront[0].position);
                //                refs.Pop_Walkdust(tank.trailPointsFront[1].position);
                //            }
                //        }
                //    }
                //}

                //Toggle moving sound
                if (!playingMoveSound) //... and havn't played sound yet, then turn it on.
                {
                    tank.audio_move.enabled = true;
                    tank.audio_move.Play(delay);
                    playingMoveSound = true;
                }
            }
            else  //If not moving forward, and ...
            {
                if (playingMoveSound) //...currently playing sound, then stop sound.
                {
                    playingMoveSound = false;
                    tank.audio_move.enabled = false;
                }
            }
        }
        else
        {

            tank.rb.velocity = tank.storedVel;
            if (playingMoveSound) //...currently playing sound, then stop sound.
            {
                playingMoveSound = false;
                tank.audio_move.enabled = false;
            }
        }
    }

    //void GlidingUpdate ()
    //{
    //    if (tank.control.B_Btn)
    //    {
    //        glideMode = true;
    //    }

    //    if (tank.control.B_BtnDown)
    //    {
    //        tank.storedVel = tank.control.MoveY * trans.up;
    //    }
    //    else if (tank.control.B_BtnUp)
    //    {
    //        glideMode = false;
    //    }

    //    //if (tank.control.B_BtnDown)
    //    //{
    //    //    storedVelDir = tank.control.MoveY * trans.up;
    //    //    glideMode = true;
    //    //}
    //}
    #endregion
}