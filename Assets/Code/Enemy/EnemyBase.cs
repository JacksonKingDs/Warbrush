using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    #region Fields
    //Reference
    public SpriteRenderer spriteRend;
    public List<Transform> paintPoints;
    public EnemyType enemyType;

    //Cache
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform trans;
    [HideInInspector] public PolygonCollider2D playerCol;

    //Stats
    protected int HP = 3;
    protected int MaxHP = 3;
    protected bool invulnerable = false;
    //protected Color color_hp2;
    //protected Color color_hp1;
    protected bool onScreenNow = false;
    protected List<IntXY> painted = new List<IntXY>();
    protected Vector3 pos;
    protected Color enemyColor = Color.black;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float rotSpeed;

    //Refs
    protected GM gm;
    //protected InputManager inputM;
    //protected SettingsAndPrefabRefs refs;
    //protected UIManager uiManager;
    protected BGTextureManager BG_Painter;
    protected FightSceneManager sceneM;
    protected AudioManager audioM;
    protected EnemyManager enemyM;
    protected Camerashake camShake;
    protected SettingsAndPrefabRefs refs;

    //Bound
    protected float BG_Bound_minX;
    protected float BG_Bound_minY;
    protected float BG_Bound_maxX;
    protected float BG_Bound_maxY;

    protected Vector3 offscreen = new Vector3(-10f, -10f, 0f);
    #endregion

    #region Initialization
    //Instantiation: 1. Initialization, BaseInitialization 2. Activation, activation additional effect
    //Object pool: 1. Activation, activation additional effect

    //Called on first time instantiation.
    protected void BaseInitialization() 
    {
        gm = GM.instance;
        //inputM = InputManager.Instance;
        //uiManager = UIManager.instance;
        BG_Painter = BGTextureManager.instance;
        sceneM = FightSceneManager.instance;
        audioM = AudioManager.instance;
        enemyM = EnemyManager.instance;
        refs = SettingsAndPrefabRefs.instance;

        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<PolygonCollider2D>();
        camShake = Camerashake.instance;

        //color_hp1 = gm.color_enemy_1;
        //color_hp2 = gm.color_enemy_2;

        BG_Bound_minX = BGTextureManager.BG_Bound_minX;
        BG_Bound_minY = BGTextureManager.BG_Bound_minY;
        BG_Bound_maxX = BGTextureManager.BG_Bound_maxX;
        BG_Bound_maxY = BGTextureManager.BG_Bound_maxY;

        onScreenNow = false;
    }

    public abstract void Initialization();

    //Called after re-enabling from object pool.
    public virtual void Activation(Vector3 pos, Quaternion rot)
    {
        HP = MaxHP;
        trans.position = pos;
        trans.rotation = rot;
        ActivationAdditionalEffect();
        StartCoroutine(Spawning());
    }

    public virtual void Activation(Vector3 pos, Quaternion rot, Transform leader, bool leftUp) //This one is for Centipede
    {
        HP = MaxHP;        
        trans.position = pos;
        trans.rotation = rot;
        ActivationAdditionalEffect();
        StartCoroutine(Spawning());
    }

    protected virtual IEnumerator Spawning ()
    {
        onScreenNow = false;
        yield return new WaitForSeconds(1.2f);
        onScreenNow = true;
    }

    protected virtual void ActivationAdditionalEffect() {}
    #endregion

    #region Public get hit
    public virtual void TakeDamage(int index, int dmg = 1)
    {
        if (!invulnerable)
        {
            if (index >= 0 && index < 4)
            {
                FightSceneManager.landed[index]++;
            }
            
            HP -= dmg;
            if (HP <= 0)
            {
                //GameObject g = Instantiate(refs.Pfx_EnemyExplode, trans.position, trans.rotation) as GameObject;
                //g.transform.localScale = transform.localScale;
                 
                //Dead
                if (index >= 0 && index < 4)
                    FightSceneManager.AddKillScore(index);
                trans.position = offscreen;
                enemyM.ReturnToPool(gameObject);

            }
            else
            {
                StartCoroutine(GetHitBlink());
            }
        }
    }

    protected virtual IEnumerator GetHitBlink()
    {
        invulnerable = true;

        //Do black white blinks
        spriteRend.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.red;
        yield return null;
        spriteRend.color = Color.white;
        yield return new WaitForSeconds(0.05f);

        //Do transparent blinks
        bool isWhite = false;
        for (int i = 0; i < 2; i++)
        {
            if (isWhite)
            {
                spriteRend.color = Color.black;
            }
            else
            {
                spriteRend.color = Color.white;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }

        invulnerable = false;
        spriteRend.color = enemyColor;
        //if (HP == 2)
        //{
        //    spriteRend.color = color_hp2;
        //}
        //else
        //{
        //    spriteRend.color = color_hp1;
        //}
    }
    #endregion

    #region Hits Player Effect
    protected virtual void DefaultTriggerEnter(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase targetPlayer = go.GetComponent<TankControllerBase>();

                HitsPlayerEffect(go);

                targetPlayer.GetsHitByAttack(trans.position, GM.enemyIndex);
                TakeDamage(targetPlayer.index, 2);

                if (GM.gameMode == GameMode.Coop_Arcade)
                {
                    GameObject.Instantiate(refs.Pfx_HitSparkB_BlackVersion, trans.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
                }
            }
            else if (go.layer == GM.layerProp)
            {
                IProps prop = go.GetComponent<IProps>();
                prop.PropInteraction(GM.enemyIndex);
            }
        }
    }

    protected virtual void DefaultCollisionEnter(Collision2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase targetPlayer = go.GetComponent<TankControllerBase>();

                HitsPlayerEffect(go);

                targetPlayer.GetsHitByAttack(trans.position, GM.enemyIndex);
                TakeDamage(targetPlayer.index, 2);
            }
            else if (go.layer == GM.layerProp)
            {
                IProps prop = go.GetComponent<IProps>();
                prop.PropInteraction(GM.enemyIndex);
            }
        }
    }

    protected virtual void HitsPlayerEffect(GameObject go)
    {
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_BoxExplode, (trans.position + go.transform.position)/2f, Quaternion.identity); //Pfx
        AudioManager.instance.Spawn_Hits1(); //Sfx

        SpawnSplatter(go);
        camShake.DoSmallShake();  //Cam shake
    }

    protected virtual void SpawnSplatter(GameObject go)
    {
        BG_Painter.PaintSplatterFlower(pos, pos - go.transform.position, GM.enemyIndex);
        // BG_Painter.PaintSplatter(pos, pos - go.transform.position, GM.enemyIndex);
    }
    #endregion

    protected void DrawingUpdate()
    {
        BG_Painter.PaintFG_ArcadeEnemy_Points(paintPoints);
    }
}