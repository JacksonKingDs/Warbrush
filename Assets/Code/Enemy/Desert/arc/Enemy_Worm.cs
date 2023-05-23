using UnityEngine;
using System.Collections;

public class Enemy_Worm : EnemyBase
{
    //Rotation
    float rotAmount = 0f;

    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 1f;
        rotSpeed = 0.7f;
        HP = MaxHP = 100;
    }

    void InitialRandomPosition()
    {
        //Random pos
        float x = Random.Range(5f, 6f);
        float y = Random.Range(3f, 4f);
        if (Random.value > 0.5f)
            x = -x;
        if (Random.value > 0.5f)
            y = -y;
        trans.position = new Vector3(x, y, trans.position.z);

        //Random dir
        Vector3 curDir = Quaternion.Euler(0, 0, Random.Range(-180f, 180f)) * trans.up;
        SetNewVelocity(curDir * moveSpeed);
    }

    protected override void ActivationAdditionalEffect()
    {
        StartCoroutine(RandomRotationUpdate());
        rb.velocity = moveSpeed * trans.up;
    }

    void Update()
    {
        //BG_Painter.PaintWorm(trans.position);
    }

    void FixedUpdate()
    {
        //OutOfBoundsCheck();
        rb.angularVelocity = rotAmount * rotSpeed;
        rb.velocity = moveSpeed * trans.up;
    }

    IEnumerator RandomRotationUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
            rotAmount = (Random.value < 0.5f) ? -0.2f : 0.2f;
            OutOfBoundsCheck();
        }
    }

    #region Collision
    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);

        DeflectAway(col.transform.position);
    }


    public override void TakeDamage(int index, int dmg = 1)
    {
        if (!invulnerable)
        {
            if (index >= 0 && index < 4)
            {
                FightSceneManager.landed[index]++;
            }
            DeflectAway(sceneM.tanksTrans[index].position);
        }
    }

    void OutOfBoundsCheck()
    {
        pos = transform.position;
        if (pos.x > BG_Bound_maxX) //Right
        {
            //UnityEngine.Debug.Log("right");
            Vector3 vel = rb.velocity;
            vel.x = -Mathf.Abs(vel.x);
            SetNewVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX) //Left
        {
            //UnityEngine.Debug.Log("Lt");
            Vector3 vel = rb.velocity;
            vel.x = Mathf.Abs(vel.x);
            SetNewVelocity(vel);
        }
        else if (pos.y > BG_Bound_maxY) //Up
        {
            //UnityEngine.Debug.Log("Up");
            Vector3 vel = rb.velocity;
            vel.y = -Mathf.Abs(vel.y);
            SetNewVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY) //Down
        {
            //UnityEngine.Debug.Log("Dn");
            Vector3 vel = rb.velocity;
            vel.y = Mathf.Abs(vel.y);
            SetNewVelocity(vel);
        }

        //pos = trans.position;
        //if (pos.x <= BG_Bound_minX)
        //{
        //    curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.right;
        //}
        //else if (pos.x >= BG_Bound_maxX)
        //{
        //    curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.left;
        //}
        //else if (pos.y <= BG_Bound_minY)
        //{
        //    curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.up;
        //}
        //else if (pos.y >= BG_Bound_maxY)
        //{
        //    curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.down;
        //}
    }
    #endregion
    
    #region Deflect
    void DeflectAway(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - trans.position).normalized;
        dir.z = 0f;
        SetNewVelocity(dir * moveSpeed);
    }
    #endregion

    #region Util
    void SetNewVelocity(Vector3 vel)
    {
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
    #endregion
}

/*
    //Reference
    public List<Transform> paintPoints;

    //Cache
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform trans;
    [HideInInspector] public Collider2D playerCol;

    //Stats
    float moveSpeed;
    float rotSpeed;

    //Current state
    int HP = 10;
    bool invulnerable = false;
    float rotDir = 0f; //Rotating left or right etc.
    List<IntXY> painted = new List<IntXY>();
    Vector3 pos;

    //Refs
    GM gm;
    BGTextureManager BG_Painter;
    FightSceneManager sceneM;
    AudioManager audioM;
    Camerashake camShake;
    SettingsAndPrefabRefs refs;

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
        refs = SettingsAndPrefabRefs.instance;

        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
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
        OutOfBoundReflect();
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
        yield return new WaitForSeconds(0.2f);
        invulnerable = false;
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
}*/
