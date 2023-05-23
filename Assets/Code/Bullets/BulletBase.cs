using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public SpriteRenderer rends;
    public Transform aura;
    public bool updateAura = false;

    //Bullet data
    [HideInInspector] public int index;

    //Bound
    protected float BG_Bound_minX;
    protected float BG_Bound_minY;
    protected float BG_Bound_maxX;
    protected float BG_Bound_maxY;

    //Class reference
    protected SettingsAndPrefabRefs refs;
    protected Camerashake camShake;

    //Cache
    public List<Transform> pointTransforms;

    protected EnemyManager enemyM;
    protected BGTextureManager BG_Painter;
    protected Rigidbody2D rb;
    protected Transform trans;
    protected GM gm;

    protected BehaviorNormalAttack bulletTracker;
    protected Vector3 arrivalLocation;

    protected bool isSpookyMode = false;
    protected bool isSpaceMode = false;
    protected bool isDesert = false;

    //Painting
    protected List<IntXY> painted = new List<IntXY>(); //Painted points
    
    protected void OnAwake()
    {
        //Reference
        gm = GM.instance;
        BG_Painter = BGTextureManager.instance;
        refs = SettingsAndPrefabRefs.instance;
        rb = GetComponent<Rigidbody2D>();
        trans = transform;
        camShake = Camerashake.instance;


        if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
        {
            enemyM = EnemyManager.instance;
        }
        isSpookyMode = GM.gameMode == GameMode.Coop_Torch;
        isDesert = GM.gameMode == GameMode.PVP_Desert;
        isSpaceMode = GM.gameMode == GameMode.Hanabi;

        
        //Initialize
        BG_Bound_minX = BGTextureManager.BG_Bound_minX;
        BG_Bound_minY = BGTextureManager.BG_Bound_minY;
        BG_Bound_maxX = BGTextureManager.BG_Bound_maxX;
        BG_Bound_maxY = BGTextureManager.BG_Bound_maxY;
    }

    public virtual void Shoot(int index, BehaviorNormalAttack behavior)
    {
        bulletTracker = behavior;
        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }
    }

    public virtual void Shoot(int index, Vector3 arrivalLocation)
    {
        this.arrivalLocation = arrivalLocation;
    }

    Vector3 initialAuraSize;
    Vector3 targetAuraSize;
    protected IEnumerator AuraPulse ()
    {

        initialAuraSize = aura.localScale;
        targetAuraSize = initialAuraSize * 1.3f;
        while (true)
        {
            for (float i = 0f; i < 1f; i += 0.2f)
            {
                aura.localScale = Vector3.Lerp(initialAuraSize, targetAuraSize, i);
                yield return null;
            }
            for (float i = 0f; i < 1f; i += 0.2f)
            {
                aura.localScale = Vector3.Lerp(targetAuraSize, initialAuraSize, i);
                yield return null;
            }
        }
    }

    //protected void InitializeRendererColor ()
    //{
    //    if (isSpaceMode)
    //    {
    //        rends.color = GM.pallet.Trans[index];
    //        rends.enabled = true;
    //    }
    //    else
    //    {
    //        rends.enabled = true;
    //    }
    //}
    
    public virtual void InstantDestroy ()
    {
        Destroy(gameObject);
    }

    public virtual void HitNPCEffect(GameObject hitGO, bool bigHit = true)
    {
        if (index >= 0 && index < 4)
        {
            FightSceneManager.landed[index]++; //Score
        }
        
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitAura, trans.position, Quaternion.identity); //Pfx
        //if (GM.gameMode == GameMode.Coop_Arcade)
        //{
        //    GameObject.Instantiate(refs.Pfx_HitSparkB_BlackVersion, trans.position, Quaternion.identity);
        //}
        //else
        Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);

        AudioManager.instance.Spawn_Hits1(); //Sfx

        if (bigHit) //Splatter only at max charge
        {
            SpawnSplatter(hitGO);
            camShake.DoSmallShake();  //Cam shake
        }
        //Instantiate(SettingsAndPrefabRefs.instance.Pfx_LeftRightExplode, pos, trans.rotation); //On hit explosion pfx
    }

    public virtual void HitBulletEffect (GameObject enemyBullet, float moveSpeed)
    {
        
        AudioManager.instance.Spawn_Hits2();
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitSparkA, trans.position, trans.rotation);

        BulletBase bullet = enemyBullet.GetComponent<BulletBase>();

        if (bullet.index != GM.enemyIndex)
        {
            //Deflect
            Vector2 dirAway = trans.position - enemyBullet.transform.position;
            rb.velocity = dirAway.normalized * moveSpeed;
            trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

            StartCoroutine(DelayedIndexChange(bullet.index));
        }
        else
        {
            DestroyBullet();
        }
    }

    //HIT OBSTACLES
    public virtual void HitObstacleEffect(GameObject go)
    {
        HitDeadTankEffect(go);
        go.GetComponent<IObstacle>().TakeDmg();
    }

    //public virtual void HitObstacleEffect_FixedOrientation(GameObject go, bool isHorizontal)
    //{
    //    HitDeadTankEffect_FixedOrientation(go, isHorizontal);
    //    go.GetComponent<BGObstacle_FightScene>().TakeDmg();
    //}

    public virtual void HitDeadTankEffect(GameObject go)
    {
        AudioManager.instance.Spawn_Hits2();
        SpawnSplatter(go, false);
        //Instantiate(SettingsAndPrefabRefs.instance.Pfx_LeftRightExplode, pos, trans.rotation);
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitAura, trans.position, Quaternion.identity);
    }

    //public virtual void HitDeadTankEffect_FixedOrientation(GameObject go, bool isHorizontal)
    //{
    //    AudioManager.instance.Spawn_Hits2();
    //    BG_Painter.PaintSplatterFlower(pos, pos - (Vector2)go.transform.position, index);
    //    if (isHorizontal)
    //        Instantiate(SettingsAndPrefabRefs.instance.Pfx_LeftRightExplode, pos, Quaternion.LookRotation(Vector3.forward, Vector3.up));
    //    else
    //        Instantiate(SettingsAndPrefabRefs.instance.Pfx_LeftRightExplode, pos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
    //}

    public virtual void HitSides()
    {
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitAura, trans.position, trans.rotation);
    }

    //SPLATTER
    public virtual void SpawnSplatter (GameObject hitGO, bool hitPlayer = true)
    {
        if (isSpaceMode)
        {
            if (hitPlayer)
                refs.SpawnHanabiSplatterBig(trans.position, index);
            else
                refs.SpawnHanabiSplatterSmall(trans.position, index);
        }
        else
        {
            BG_Painter.PaintSplatterFlower(trans.position, trans.position - hitGO.transform.position, index);

            if (isDesert)
                refs.SpawnSandCluster(trans.position);
        }
        
    }

    //public virtual void SpawnSplatterOther(GameObject go, int enemyIndex)
    //{
    //    if (isSpaceMode)
    //    {
    //        refs.SpawnSpaceDustRandomized(trans.position, index);
    //    }
    //    else
    //    {
    //        BG_Painter.PaintSplatterFlower(pos, pos - (Vector2)go.transform.position, enemyIndex);
    //    }
    //}

    protected IEnumerator DelayedIndexChange(int newIndex)
    {
        yield return null;
        index = newIndex;
        painted = new List<IntXY>();
    }

    public virtual void DestroyBullet()
    {
        if (bulletTracker != null)
            bulletTracker.RemoveBullet(gameObject);

        Destroy(gameObject);
    }
}