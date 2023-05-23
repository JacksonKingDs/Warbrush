using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDesert_Wurm: MonoBehaviour, IEnemy
{
    //Reference
    public List<Transform> paintPoints;

    //Cache
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform trans;

    //Stats
    float moveSpeed;
    float rotSpeed;

    //Current state
    float rotDir = 0f; //Rotating left or right etc.
    List<IntXY> painted = new List<IntXY>();
    Vector3 pos;

    //Refs
    GM gm;
    BGTextureManager BG_Painter;
    FightSceneManager sceneM;
    AudioManager audioM;
    Camerashake camShake;

    //Bound
    float BG_Bound_minX;
    float BG_Bound_minY;
    float BG_Bound_maxX;
    float BG_Bound_maxY;

    void Start()
    {
        gm = GM.instance;
        BG_Painter = BGTextureManager.instance;
        sceneM = FightSceneManager.instance;
        audioM = AudioManager.instance;

        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        camShake = Camerashake.instance;

        BG_Bound_minX = BGTextureManager.BG_Bound_minX;
        BG_Bound_minY = BGTextureManager.BG_Bound_minY;
        BG_Bound_maxX = BGTextureManager.BG_Bound_maxX;
        BG_Bound_maxY = BGTextureManager.BG_Bound_maxY;

        moveSpeed = 1f;
        rotSpeed = 100f;

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        StartCoroutine(RandomizeRotation());

        //yield return new WaitForSeconds(5f);
        //Destroy(gameObject);
    }

    public void Update()
    {
        pos = transform.position;
        DrawingUpdate();
    }

    public void FixedUpdate()
    {
        //Rotation: constant random
        //OutOfBoundReflect();
        trans.position = TankUtil.WrapWorldPosTankPos(trans.position);
        rb.angularVelocity = rotDir * rotSpeed;

        //Move forward
        rb.velocity = moveSpeed * trans.up;
    }

    IEnumerator RandomizeRotation()
    {
        while (true)
        {
            rotDir = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    protected void OutOfBoundReflect()
    {
        pos = transform.position;
        if (pos.x > BG_Bound_maxX) //Right
        {
            //UnityEngine.Debug.Log("right");
            Vector3 vel = rb.velocity;
            vel.x = -Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX) //Left
        {
            //UnityEngine.Debug.Log("Lt");
            Vector3 vel = rb.velocity;
            vel.x = Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y > BG_Bound_maxY) //Up
        {
            //UnityEngine.Debug.Log("Up");
            Vector3 vel = rb.velocity;
            vel.y = -Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY) //Down
        {
            //UnityEngine.Debug.Log("Dn");
            Vector3 vel = rb.velocity;
            vel.y = Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
    }

    void SetRigidbodyVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    #region Public get hit
    public virtual void TakeDamage(int index, int dmg = 1)
    {
        if (index >= 0 && index < 4)
            FightSceneManager.landed[index]++;
    }

    #endregion

    #region Hits Player Effect
    void OnTriggerEnter2D (Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase targetPlayer = go.GetComponent<TankControllerBase>();

                HitsPlayerEffect(go);

                targetPlayer.GetsHitByAttack(trans.position, GM.enemyIndex);

                //Deflect velocity
                SetRigidbodyVelocity((pos - go.transform.position).normalized * moveSpeed);
            }
            else if (go.layer == GM.layerObstacle)
            {
                //Deflect velocity
                SetRigidbodyVelocity((pos - go.transform.position).normalized * moveSpeed);
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
        BG_Painter.PaintSplatterFlower(pos, pos - go.transform.position, GM.enemyIndex);
    }
    #endregion

    protected void DrawingUpdate()
    {
        foreach (var p in paintPoints)
        {
            BG_Painter.AddWurmRipple(p.position);
        }
    }
}