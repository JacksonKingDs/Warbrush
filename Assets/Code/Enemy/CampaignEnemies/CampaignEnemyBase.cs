using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignEnemyBase : MonoBehaviour, IEnemy
{
    #region Field
    public static int count = 0;
    public static List<GameObject> enemies = new List<GameObject>();

    public List<Vector2> patrolPoints;

    //Component reference
    public SpriteRenderer spriteRend;
    public List<Transform> paintPoints;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform trans;
    [HideInInspector] public PolygonCollider2D playerCol;

    //Stats
    protected int HP = 3;
    protected int MaxHP = 3;
    protected bool invulnerable = false;
    protected List<IntXY> painted = new List<IntXY>();
    protected Vector3 pos;
    protected Color enemyColor = Color.black;

    protected bool gameStarted = false;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float rotSpeed;

    //Patrol
    protected Vector3 tgtDir;
    protected Vector3 curDir;
    protected float patrolSpeed = 0.8f;
    protected int patrolIndex;
    protected Vector2 currentPatrolDestination;

    //Refs
    protected GM gm;
    //protected InputManager inputM;
    //protected SettingsAndPrefabRefs refs;
    //protected UIManager uiManager;
    protected BGTextureManager BG_Painter;
    protected FightSceneManager sceneM;
    protected AudioManager audioM;
    protected Camerashake camShake;
    protected SettingsAndPrefabRefs refs;
    bool doPaint;
    
    #endregion

    #region MonoBehavior
    protected void OnAwake ()
    {
        count = 0;
    }

    protected void OnStart()
    {
        count++;
        enemies.Add(gameObject);
        gm = GM.instance;
        BG_Painter = BGTextureManager.instance;
        sceneM = FightSceneManager.instance;
        audioM = AudioManager.instance;
        refs = SettingsAndPrefabRefs.instance;

        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<PolygonCollider2D>();
        camShake = Camerashake.instance;

        HP = MaxHP;
        doPaint = sceneM.activeLevelInfo.canPaint;
        enemyColor = BG_Painter.enemy_body;
        spriteRend.color = enemyColor;
    }

    #endregion

    #region Public get hit
    bool dead = false;
    public virtual void TakeDamage(int index, int dmg = 1)
    {
        if (!invulnerable)
        {
            if (index >= 0 && index < 4)
            {
                FightSceneManager.landed[index]++;
            }

            HP -= dmg;
            if (HP <= 0 && !dead)
            {
                dead = true;
                //GameObject g = Instantiate(refs.Pfx_EnemyExplode, trans.position, trans.rotation) as GameObject;
                //g.transform.localScale = transform.localScale;

                //Dead
                if (index >= 0 && index < 4)
                    FightSceneManager.AddKillScore(index);
                Die(index);
                Destroy(gameObject);
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
    }

    protected void Die(int playerIndex)
    {
        count--;
        CampaignLevelInfo.TryRemoveEnemy(transform);

        if (count <= 0)
        {
            sceneM.AllEnemiesDead(playerIndex);
        }
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
                //TakeDamage(targetPlayer.index, 2);
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
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_BoxExplode, (trans.position + go.transform.position) / 2f, Quaternion.identity); //Pfx
        AudioManager.instance.Spawn_Hits1(); //Sfx

        SpawnSplatter(go);
        camShake.DoSmallShake();  //Cam shake
    }

    protected virtual void SpawnSplatter(GameObject go)
    {
        //For desert enemies, override this to call something else.
        //For space enemies, override this to paint explosion instead
        pos = transform.position;
        BG_Painter.PaintSplatterFlower(pos, pos - go.transform.position, GM.enemyIndex);
        // BG_Painter.PaintSplatter(pos, pos - go.transform.position, GM.enemyIndex);
    }
    #endregion

    protected void Patrol()
    {
        pos = trans.position;

        //If have arrived at destination:
        if (Vector2.Distance(pos, currentPatrolDestination) < 0.05f)
        {
            //Increment patrol index and update new patrol destination
            ++patrolIndex;
            if (patrolIndex == patrolPoints.Count)
            {
                patrolIndex = 0;
            }

            currentPatrolDestination = patrolPoints[patrolIndex];
        }

        //If have not, then move towards it
        curDir = Vector3.RotateTowards(curDir, currentPatrolDestination - (Vector2)pos, rotSpeed, 0.0f);

        rb.velocity = patrolSpeed * curDir.normalized;

        //Debug.DrawLine(pos, currentPatrolDestination, Color.yellow, 0.5f);
        //Debug.DrawRay(pos, curDir, Color.red, 0.5f);
    }



    protected void DrawingUpdate()
    {
        BG_Painter.PaintFG_ArcadeEnemy_Points(paintPoints);
    }
}
