using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TankModelStats
{
    public float MoveSpeed;
    public float RotationSpeed;    

    public TankModelStats(float moveSpeed, float rotationSpeed)
    {
        MoveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;        
    }
}

//Contain references for game objects within the scene and prefabs (bullets, sfx, pfx)
public class SettingsAndPrefabRefs : MonoBehaviour
{
    #region Fields
    public static SettingsAndPrefabRefs instance;

    //Public static: tank model stats
    public static Dictionary<TankModelNames, TankModelStats> TankStats;

    //Public static: physics layers
    //[HideInInspector] public static int layerPlayer;
    //[HideInInspector] public static int layerObstacle;
    //[HideInInspector] public static int layerEnemy;
    //[HideInInspector] public static int layerBullet;

    //[Header("Settings")]
    [Header("Tank components")] //References that are not P1 P2 P3 P4 specific, e.g. the shoot point of P1 is always the same, but the tank model differs on each game session
    public Sprite rifleTank_sprite;
    public Sprite shotgunTank_sprite;
    public Sprite grenadeTank_sprite;
    public Sprite bouncerTank_sprite;
    public Sprite lancerTank_sprite;

    [Space(20)]
    [Header("PFX")]
    public GameObject muzzleFlash_Bow;
    public GameObject muzzleFlash_BowDouble;
    public GameObject muzzleFlash_Behind;
    public GameObject Pfx_AttackCharge;
    public GameObject Pfx_HitAura;
    public GameObject Pfx_HitSparkA;
    public GameObject Pfx_HitSparkB_BlackVersion;
    public GameObject Pfx_HitSparkB_Shorter;
    public GameObject Pfx_SpawnGlitter;
    public GameObject Pfx_LeftRightExplode;
    public GameObject Pfx_BoxExplode;
    public GameObject Pfx_Box2Explode;
    public GameObject Pfx_EnemyExplode;
    public GameObject Pfx_WalkDust;
    public GameObject Pfx_CrescentExplode;
    List<GameObject> pool_walkDust;
    List<GameObject> active_walkDust;

    [Space(20)]
    [Header("OCEAN")]
    public GameObject pf_oceanLine;
    public GameObject pf_oceanRipple;
    List<GameObject> pool_oceanTrail;
    List<GameObject> active_OceanTrail;

    public GameObject pf_RainStroke;
    public GameObject pf_RainSplatter;
    List<GameObject> pool_rainStroke;
    List<GameObject> pool_rainSplatter;
    List<GameObject> active_rainStroke;
    List<GameObject> active_rainSplatter;

    [Space(20)]
    [Header("DESERT")]
    public GameObject pf_desertSand;
    List<GameObject> pool_desertSand;
    List<GameObject> active_desertSand;

    [Space(20)]
    [Header("SPACE")]
    public GameObject pf_spaceDust;
    List<GameObject> pool_spaceDust;
    List<GameObject> active_spaceDust;

    [Space(20)]
    [Header("Attacks")]
    public GameObject Pf_Bullet_Rifle;
    public GameObject Pf_Bullet_Shotgun;
    public GameObject Pf_Bullet_Bounce;
    public GameObject Pf_Bullet_Grenade;
    public GameObject Pf_Bullet_Lancer;
    public GameObject Pf_LancerMeleeCollider;
    public GameObject Pf_GrenadeExplode;
    public GameObject Pf_BulletTiny;
    public GameObject Pf_BulletTinyDouble;
    [Space(20)]
    public GameObject Pf_UltiRifle;
    public GameObject Pf_UltiShotgun;
    public GameObject Pf_UltiBouncer;
    public GameObject Pf_UltiGrenade;
    public GameObject Pf_UltiLancer;

    [Space(20)]
    [Header("Others")]
    public GameObject[] dead_Tanks;
    public GameObject pf_tankTaunter;

    //Pool
    Vector3 offscreen = new Vector3(-10f, -10f, 0f);

    BGTextureManager BG_Painter;

    Vector3 defaultCrescentScale;
    #endregion

    #region Initialization    
    void Awake()
    {
        instance = this;
        pool_oceanTrail = new List<GameObject>();
        active_OceanTrail = new List<GameObject>();
        pool_desertSand = new List<GameObject>();
        active_desertSand = new List<GameObject>();
        pool_spaceDust = new List<GameObject>();
        active_spaceDust = new List<GameObject>();

        pool_rainStroke = new List<GameObject>();
        pool_rainSplatter = new List<GameObject>();
        active_rainStroke = new List<GameObject>();
        active_rainSplatter = new List<GameObject>();

        pool_walkDust = new List<GameObject>();
        active_walkDust = new List<GameObject>();

        defaultCrescentScale = Pfx_CrescentExplode.transform.localScale;

        InitializeTankStats();
    }

    void InitializeTankStats()
    {
        TankStats = new Dictionary<TankModelNames, TankModelStats>();
        TankStats.Add(TankModelNames.RIFLE, new TankModelStats(
            2f,       //Movespeed
            140f));    //Rotationspeed
        TankStats.Add(TankModelNames.SHOTGUN, new TankModelStats(
            2.4f,       //Movespeed
            100f));    //Rotationspeed
        TankStats.Add(TankModelNames.GRENADE, new TankModelStats(
            1.8f,       //Movespeed
            200f));    //Rotationspeed
        TankStats.Add(TankModelNames.BOUNCER, new TankModelStats(
            2f,       //Movespeed
            120f));    //Rotationspeed
        TankStats.Add(TankModelNames.SEEKER, new TankModelStats(
            3f,       //Movespeed
            160f));    //Rotationspeed
    }

    private void Start()
    {
        BG_Painter = BGTextureManager.instance;
    }
    #endregion

    #region Update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(TestSpawn());
        }
    }
    #endregion

    #region Spawn Pool Object
    IEnumerator TestSpawn ()
    {
        yield return null;
    }
    #endregion

    #region One time Pfx
    public void SpawnCrescentExplode (Vector3 location)
    {
        Vector3 posMod = Random.insideUnitSphere * 0.2f;
        Transform t = Instantiate(Pfx_CrescentExplode, location + new Vector3(posMod.x, posMod.y, -0.5f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))).transform;
        t.localScale = defaultCrescentScale *= Random.Range(0.9f, 1.5f);
    }
    #endregion

    #region Object pool - Desert sand
    public void SpawnSandCluster(Vector3 pos)
    {
        for(int i = 0; i < 10; i++)
        {
            Vector3 sp = new Vector3(pos.x + Random.Range(-0.5f, 0.5f), pos.y + Random.Range(-0.5f, 0.5f), -0.2f);
            //Quaternion rot = Quaternion.LookRotation(Vector3.forward, sp - pos);
            Pop_DesertSand(sp);
            //Debug.DrawLine(pos, sp, Color.red, 10f);
        }
    }

    public GameObject Pop_DesertSand(Vector3 pos)
    {
        foreach (GameObject item in pool_desertSand)
        {
            if (!item.activeSelf)
            {
                active_desertSand.Add(item);
                item.SetActive(true);
                item.GetComponent<DesertSandOrigin>().Activation(pos);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(pf_desertSand, pos, Quaternion.identity);
        e.GetComponent<DesertSandOrigin>().Initialize();
        e.GetComponent<DesertSandOrigin>().Activation(pos);
        active_desertSand.Add(e);
        pool_desertSand.Add(e);
        return e;
    }

    public void Push_DesertSand(GameObject go)
    {
        go.SetActive(false);
        active_desertSand.Remove(go);
    }
    #endregion

    #region Object pool - Space dust
    public void SpawnHanabiSplatterSmall(Vector3 pos, int index)
    {
        for (int i = 0; i < 10; i++)
        {
            Pop_HanabiExplosion(pos, Quaternion.Euler(0, 0, i * 36f + Random.Range(-2f, 2f)), index, true);
            //Pop_SpaceDust(pos, Quaternion.Euler(0, 0, i * 36f + Random.Range(-18f, 18f)), index);
            //GameObject go = Pop_SpaceDust(pos, Quaternion.Euler(0, 0, i * 36f), index);
        }
    }

    public void SpawnHanabiSplatterBig(Vector3 pos, int index)
    {
        for (int i = 0; i < 10; i ++)
        {
            Pop_HanabiExplosion(pos, Quaternion.Euler(0, 0, i * 36f + Random.Range(-2f, 2f)), index);
            //Pop_SpaceDust(pos, Quaternion.Euler(0, 0, i * 36f + Random.Range(-18f, 18f)), index);
            //GameObject go = Pop_SpaceDust(pos, Quaternion.Euler(0, 0, i * 36f), index);
        }
        BG_Painter.AddHanabiHeart(pos, index);

        //Pop_SpaceDust(new Vector3(pos.x + Random.Range(-0.1f, 0.1f), pos.y + Random.Range(-0.1f, 0.1f), -0.2f), index);

        //for (int i = 0; i < 5; i++)
        //{
        //    Vector3 sp = new Vector3(pos.x + Random.Range(-0.5f, 0.5f), pos.y + Random.Range(-0.5f, 0.5f), -0.2f);
        //    //Quaternion rot = Quaternion.LookRotation(Vector3.forward, sp - pos);
        //    Pop_SpaceDust(sp);
        //    //Debug.DrawLine(pos, sp, Color.red, 10f);
        //}
    }   

    public GameObject Pop_HanabiExplosion(Vector3 pos, Quaternion rot, int index, bool small = false)
    {
        foreach (GameObject item in pool_spaceDust)
        {
            if (!item.activeSelf)
            {
                active_spaceDust.Add(item);
                item.SetActive(true);
                item.GetComponent<SpaceDustOrigin>().Activation(pos, rot, index, small);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(pf_spaceDust, pos, Quaternion.identity);
        e.GetComponent<SpaceDustOrigin>().Initialize();
        e.GetComponent<SpaceDustOrigin>().Activation(pos, rot, index, small);
        active_spaceDust.Add(e);
        pool_spaceDust.Add(e);
        return e;
    }

    public void Push_SpaceDu(GameObject go)
    {
        go.SetActive(false);
        active_spaceDust.Remove(go);
    }
    #endregion

    #region Object pool - OceanLine
    public GameObject Pop_OceanLine(Vector3 pos, Quaternion rot, float strengthPerc)
    {
        foreach (GameObject item in pool_oceanTrail)
        {
            if (!item.activeSelf)
            {
                active_OceanTrail.Add(item);
                item.SetActive(true);
                item.GetComponent<OceanLineOrigin>().Activation(pos, rot, strengthPerc);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(pf_oceanLine, pos, rot);
        e.GetComponent<OceanLineOrigin>().Initialize();
        e.GetComponent<OceanLineOrigin>().Activation(pos, rot, strengthPerc);
        active_OceanTrail.Add(e);
        pool_oceanTrail.Add(e);
        return e;
    }

    public void Push_OceanLine(GameObject go)
    {
        go.SetActive(false);
        active_OceanTrail.Remove(go);
    }
    #endregion

    #region Object pool - Rain Stroke
    public GameObject Pop_RainStroke(Vector3 pos)
    {
        foreach (GameObject item in pool_rainStroke)
        {
            if (!item.activeSelf)
            {
                active_rainStroke.Add(item);
                item.SetActive(true);
                item.GetComponent<OceanRainStroke>().Activation(pos);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(pf_RainStroke, pos, Quaternion.identity);
        e.GetComponent<OceanRainStroke>().Initialize();
        e.GetComponent<OceanRainStroke>().Activation(pos);
        active_rainStroke.Add(e);
        pool_rainStroke.Add(e);
        return e;
    }

    public void Push_RainStroke(GameObject go)
    {
        go.SetActive(false);
        active_rainStroke.Remove(go);
    }
    #endregion

    #region Object pool - Rain Splatter
    public GameObject Pop_RainSplatter (Vector3 pos)
    {
        foreach (GameObject item in pool_rainSplatter)
        {
            if (!item.activeSelf)
            {
                active_rainSplatter.Add(item);
                item.SetActive(true);
                item.GetComponent<OceanRainSplatter>().Activation(pos);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(pf_RainSplatter, pos, Quaternion.identity);
        e.GetComponent<OceanRainSplatter>().Initialize();
        e.GetComponent<OceanRainSplatter>().Activation(pos);
        active_rainSplatter.Add(e);
        pool_rainSplatter.Add(e);
        return e;
    }

    public void Push_RainSplatter (GameObject go)
    {
        go.SetActive(false);
        active_rainSplatter.Remove(go);
    }
    #endregion

    #region Object pool - WalkDust
    public GameObject Pop_Walkdust(Vector3 pos)
    {
        foreach (GameObject item in pool_walkDust)
        {
            if (!item.activeSelf)
            {
                active_walkDust.Add(item);
                item.SetActive(true);
                item.GetComponent<IPooledItem>().Activate(pos);
                return item;
            }
        }

        //Instantiate new object if not in pool.
        GameObject e = Instantiate(Pfx_WalkDust, pos, Quaternion.identity);
        e.GetComponent<IPooledItem>().Initialize(this);
        e.GetComponent<IPooledItem>().Activate(pos);
        active_walkDust.Add(e);
        pool_walkDust.Add(e);
        return e;
    }

    public void Push_WalkDust(GameObject go)
    {
        go.SetActive(false);
        active_walkDust.Remove(go);
    }
    #endregion

    #region Util
    public Sprite GetTankModel_Sprite(TankModelNames model)
    {
        switch (model)
        {
            case TankModelNames.RIFLE:
                return rifleTank_sprite;
            case TankModelNames.SHOTGUN:
                return shotgunTank_sprite;
            case TankModelNames.GRENADE:
                return grenadeTank_sprite;
            case TankModelNames.BOUNCER:
                return bouncerTank_sprite;
            case TankModelNames.SEEKER:
            default:
                return lancerTank_sprite;
        }
    }

    public GameObject GetUlti(TankModelNames model)
    {
        switch (model)
        {
            case TankModelNames.RIFLE:
                return Pf_UltiRifle;
            case TankModelNames.GRENADE:
                return Pf_UltiGrenade;
            case TankModelNames.BOUNCER:
                return Pf_UltiBouncer;
            case TankModelNames.SHOTGUN:
                return Pf_UltiShotgun;
            case TankModelNames.SEEKER:
            default:
                return Pf_UltiLancer;
        }
    }

    public GameObject GetBulet(TankModelNames model)
    {
        switch (model)
        {

            case TankModelNames.SHOTGUN:
                return Pf_Bullet_Shotgun;
            case TankModelNames.SEEKER:
                return Pf_Bullet_Lancer;
            case TankModelNames.GRENADE:
                return Pf_Bullet_Grenade;
            case TankModelNames.BOUNCER:
                return Pf_Bullet_Bounce;
            case TankModelNames.RIFLE:
            default:
                return Pf_Bullet_Rifle;
        }
    }

    public GameObject GetDeadTank(TankModelNames model)
    {
        switch (model)
        {
            case TankModelNames.RIFLE:
                return dead_Tanks[0];
            case TankModelNames.SHOTGUN:
                return dead_Tanks[1];
            case TankModelNames.GRENADE:
                return dead_Tanks[2];
            case TankModelNames.BOUNCER:
                return dead_Tanks[3];
            case TankModelNames.SEEKER:
            default:
                return dead_Tanks[4];
        }
    }
    #endregion
}