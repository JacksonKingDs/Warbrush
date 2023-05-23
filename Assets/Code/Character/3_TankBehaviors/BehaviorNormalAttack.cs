using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BehaviorNormalAttack : BehaviorBase
{
    //FIELD
    GameObject bulletPf;
    GameObject ultimatePf;

    ShootChargingEffect chargingPfx;
    Transform shootPoint;
    bool charging;
    float curCharge = 0f;

    //Animation
    int animation_default;
    int animation_blink;

    //State
    bool isFullCharge; //Bullet statge
    bool isHalfCharge;
    float maxCharge = 0.5f;
    float halfCharge = 0.2f;
    //const float minCharge = 0.2f;
    //const float shootCD = 0.5f;
    //float shootCDTimer = 0f;
    int maxBullets = 3;
    List<GameObject> bullets = new List<GameObject>();
    bool isDesert = false;
    bool isSpaceMode = false;

    FightScene_UIDebugText uidebug;

    public int activeBulletCount { get { return bullets.Count; } }

    //Constructor
    public BehaviorNormalAttack(TankControllerBase tank, TankStateBase stateBase) : base(tank, stateBase)
    {

        //Instantiate and reference prefabs
        chargingPfx = tank.chargingPfx;
        shootPoint = chargingPfx.transform;
        //muzzleFlash = GameObject.Instantiate(MuzzlePf, shootPoint.position, Quaternion.identity, shootPoint); //Instantiate muzzle flash pf as a child to tank gameobject.        
        //muzzleFlash.SetActive(false);

        bulletPf = refs.GetBulet(tank.modelName);
        ultimatePf = refs.GetUlti(tank.modelName);

        //if (tank.modelName == TankModelNames.BOUNCER)
        //{
        //    halfCharge = 0.3f;
        //    maxCharge = 0.7f;
        //}

        animation_default = Animator.StringToHash("Default");
        animation_blink = Animator.StringToHash("Blink");

        isDesert = GM.gameMode == GameMode.PVP_Desert;
        isSpaceMode = GM.gameMode == GameMode.Hanabi;

        //Temporary debug
        uidebug = FightScene_UIDebugText.instance;
    }

    //BASE CLASS METHODS
    public override void OnBehaviorEntry() { }

    public override void OnUpdate()
    {
        ShootUpdate();
    }

    public override void OnFixedUpdate() { }

    public override void OnBehaviorExit()
    {
        //muzzleFlash.SetActive(false);
        chargingPfx.StopAll();
        charging = false;
    }

    void ShootUpdate()
    {
        //Debug.Log("control.A_BtnUp: " + control.A_BtnUp);
        //Use this instead of button down bc in some scenarios it is not detected.
        if (control.A_Btn)
        {
            if (!charging) //This occurs when you pause the game while charging
            {
                tank.chargingPfx.stg1_BeginCharging();
                curCharge = 0f;
                charging = true;
                isFullCharge = false;
                isHalfCharge = false;
            }

            Color c = Color.white;

            if (!isFullCharge)
            {
                curCharge += Time.deltaTime;
                chargingPfx.SetPixelPfx(curCharge / maxCharge);

                if (!isHalfCharge)
                {
                    if (curCharge >= halfCharge)
                    {
                        isHalfCharge = true;
                        chargingPfx.stg2_ReachedHalfCharge();
                    }
                }

                if (curCharge >= maxCharge)
                {
                    isFullCharge = true;
                    chargingPfx.stg3_ReachedMaxCharge();
                }
            }
        }
        else if (control.A_BtnUp && charging)
        {
            ReleaseAttack();
        }

        //if (tank.index == 0)
        //    uidebug.DisplayTextA("Attack charge: " + curCharge);
    }

    protected void ReleaseAttack() //Pre 3 ability
    {
        //muzzleFlash.SetActive(true);
        //muzzleOn = true;

        FightSceneManager.attacks[tank.index]++;
        //Reset charging values
        charging = false;
        //shootCDTimer = shootCD;

        tank.chargingPfx.stg4_ShootBullet(isFullCharge);

        StandardAttacks();
    }

    #region Attacks
    GameObject prevBullet;
    void StandardAttacks()
    {
        if (!isHalfCharge)
        {
            return;
        }

        Vector3 p = TankUtil.WrapWorldPos(shootPoint.position);
        if (isFullCharge)
        {
            audioM.Spawn_shoot1();
            tank.anim.Play(tank.animState_blink, 0, 0f);

            prevBullet = GameObject.Instantiate(bulletPf, p, shootPoint.rotation) as GameObject;
            prevBullet.GetComponent<BulletBase>().Shoot(tank.index, this);

            //Recoil - no Seeker
            //Bullet tracking - only Rifle, bouncer, seeker. No Shotgun, no grenade
            switch (tank.modelName)
            {
                case TankModelNames.RIFLE:
                    AddBullet(prevBullet);
                    tank.DoRecoil(true);
                    break;
                case TankModelNames.SHOTGUN:
                    tank.DoRecoil(true);
                    break;
                case TankModelNames.GRENADE:
                    tank.DoRecoil(true);
                    break;
                case TankModelNames.BOUNCER:
                    AddBullet(prevBullet);
                    tank.DoRecoil(true);
                    break;
                case TankModelNames.SEEKER:
                    AddBullet(prevBullet);
                    if (isSpaceMode)
                        tank.DoRecoil(false);
                    break;
                default:
                    break;
            }

            if (isDesert)
                refs.SpawnSandCluster(p);
        }
        else //Shoot parry bullet
        {
            if (tank.modelName == TankModelNames.SHOTGUN)
            {
                prevBullet = GameObject.Instantiate(refs.Pf_BulletTinyDouble, p, shootPoint.rotation) as GameObject;
            }
            else
            {
                prevBullet = GameObject.Instantiate(refs.Pf_BulletTiny, p, shootPoint.rotation) as GameObject;
            }

            prevBullet.GetComponent<BulletBase>().Shoot(tank.index, this);
        }
    }

    public void AddBullet(GameObject bullet)
    {
        bullets.Add(prevBullet);
        if (bullets.Count > maxBullets)
        {
            if (bullets[0] != null)
            {
                UnityEngine.Object.Destroy(bullets[0]);
            }
            
            bullets.RemoveAt(0);
        }
    }

    public void RemoveBullet(GameObject bullet)
    {
        if (bullets.Contains(bullet))
        {
            bullets.Remove(bullet);
        }
    }
    #endregion
}