using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note 1. TWIRLING PATTERN, should be an off axis paint point. 2. Sprite (the graphics) is a seperate transform component. 
public class Bullet_Bounce : BulletBase
{
    public Transform spriteTrans;

    float movespeed = 2.5f; //5
    int bouncesRemain = 3;

    Vector3 stg2_scale = new Vector3(0.10f, 0.10f, 0.10f);
    Vector3 stg1_scale = new Vector3(0.10f, 0.10f, 0.10f);

    FightSceneManager sceneM;
    int recentHitPlayerIndex = -1;
    GameObject recentHitEnemy;
    int enemyCount;

    int paintCircleCountdown = 10;
    bool canHitAnything = true;
    //bool canHitObstacles = true;

    #region Initialize
    //PUBLIC
    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();

        this.index = index;
        bulletTracker = behavior;

        trans.localScale = stg1_scale;
        SetVelocity(movespeed * transform.up);


        //InitializeRendererColor();

        sceneM = FightSceneManager.instance;

        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }
    }
    #endregion
    
    Vector3 velocity;
    Vector3 pos;
    void Update()
    {
        velocity = rb.velocity;
        pos = trans.position;
        //RaycastCollisionCheck();
        if (!isSpaceMode)
        {
            List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
            IntXY pixel = new IntXY();
            foreach (Transform t in pointTransforms)
            {
                pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
                if (!painted.Contains(pixel))
                {
                    painted.Add(pixel);
                    _toPaint.Add(pixel);
                }
            }
            BG_Painter.PaintBulletPoints(_toPaint, index);
        }
        else
        {
            BG_Painter.AddHanabiTrailShort(trans.position, index);
            //refs.SpawnSpaceDustPinPoint(transform.position, index);
        }

        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogSml(trans.position);
    }
    
    void FixedUpdate()
    {
        OutOfBoundsCheck();
        spriteTrans.Rotate(new Vector3(0f, 0f, 600f * Time.deltaTime));
    }

    void OutOfBoundsCheck()
    {
        
        if ((pos.x > BG_Bound_maxX && velocity.x > 0) ||
            (pos.x < BG_Bound_minX && velocity.x < 0) ||
            (pos.y > BG_Bound_maxY && velocity.y > 0) ||
            (pos.y < BG_Bound_minY && velocity.y < 0))
        {
            ResetRecentEnemy();
            HitSides();
            TurnToClosestEnemy(null);

            StartCoroutine(IncrementBounce());
            //SetVelocity(velocity);
        }
    }

    #region Collision
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null && canHitAnything)
        {
            GameObject go = col.gameObject;

            //If collided with an enemy player
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>(); //Ref enemy script

                if (enemyPlayer.index != index) //Don't hit owner
                {
                    HitNPCEffect(go, true);

                    recentHitPlayerIndex = enemyPlayer.index;
                    recentHitEnemy = null;

                    if (GM.gameMode == GameMode.Coop_Arcade)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                        GameObject.Instantiate(refs.Pfx_HitSparkB_BlackVersion, trans.position, Quaternion.identity);
                    }
                    else if (GM.gameMode == GameMode.Coop_Torch)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                        GameObject.Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
                    }
                    else if (GM.gameMode == GameMode.Campaign)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                        GameObject.Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
                    }
                    else
                    {
                        enemyPlayer.GetsHitByAttack(trans.position, index);
                        GameObject.Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
                    }

                    TurnToClosestEnemy(go);
                    StartCoroutine(IncrementBounce());
                }
            }
            //If collided with an obstacle
            else if (go.layer == GM.layerDeadTank)
            {
                HitDeadTankEffect(go);
                TurnToClosestEnemy(go);
                ResetRecentEnemy();

                StartCoroutine(IncrementBounce());
            }
            else if (go.layer == GM.layerObstacle)
            {
                HitObstacleEffect(go);
                //DestroyBullet();
                //return;
                TurnToClosestEnemy(go);

                ResetRecentEnemy();

                StartCoroutine(IncrementBounce());
            }
            else if (go.layer == GM.layerBullet)
            {
                HitBulletEffect(go, movespeed);

                ResetRecentEnemy();

                //Vector2 dirAway = pos - (Vector2)col.transform.position;
                //SetVelocity(dirAway.normalized * movespeed);

                StartCoroutine(IncrementBounce());
            }
            else if (go.layer == GM.layerEnemy)
            {
                //Debug.Log("Bounce bullet hit Enemy");
                HitNPCEffect(go, true);
                recentHitEnemy = go;

                TurnToClosestEnemy(go);
                go.GetComponent<IEnemy>().TakeDamage(index, 1);

                StartCoroutine(IncrementBounce());
            }
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
            
        }
    }
    #endregion

    #region Bounce and aim
    //Hits wall / obstacle / enemy>

    //First check if there is an enemy > if not, check if there is an object > if not then check if there is border

    Vector2 targetDir;
    void TurnToClosestEnemy(GameObject go)
    {
        float shortestDist = float.MaxValue;
        Vector3 v;

        //Co-op
        if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
        {
            enemyCount = enemyM.activeEnemies.Count;

            if (enemyCount <= 0)
            {
                NoMoreEnemies_StandardBounce(go);

                return;
            }

            foreach (GameObject e in enemyM.activeEnemies)
            {
                if (e == recentHitEnemy) //Ignore recent hit enemy and self. If no more enemies, then forget finding next enemy and just bounce back
                {
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        NoMoreEnemies_StandardBounce(go);
                        return;
                    }
                    continue;
                }

                Vector2 dir = e.transform.position - trans.position;
                float d = dir.sqrMagnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                }
                //Debug.DrawLine(trans.position, e.transform.position, Color.yellow, 10f);
            }
            //Debug.DrawRay(trans.position, targetDir, Color.red, 10f);
            //Debug.Break();
            //Debug.Log("active e: " + enemyCount + ", shortest: " + shortestDist);
        }
        else if (GM.gameMode == GameMode.Campaign)
        {
            enemyCount = CampaignEnemyBase.enemies.Count;

            if (CampaignEnemyBase.enemies.Count <= 0)
            {
                NoMoreEnemies_StandardBounce(go);

                return;
            }

            foreach (GameObject e in CampaignEnemyBase.enemies)
            {
                if (e == recentHitEnemy) //Ignore recent hit enemy and self. If no more enemies, then forget finding next enemy and just bounce back
                {
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        NoMoreEnemies_StandardBounce(go);
                        return;
                    }
                    continue;
                }

                Vector2 dir = e.transform.position - transform.position;
                float d = dir.magnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                    //enemyIndex = i;
                }
            }
        }
        //PVP
        else
        {
            enemyCount = sceneM.validPlayers.Count;
            //If ran out of enemy (last man standing) then normal deflect, or if there is only 1 enemy and the bullet just bounced off him
            if (sceneM.validPlayers.Count <= 0)
            {
                NoMoreEnemies_StandardBounce(go);
                return;
            }

            foreach (int i in sceneM.validPlayers)
            {
                if (i == recentHitPlayerIndex || i == index) //Ignore recent hit enemy and self. If no more enemies, then forget finding next enemy and just bounce back
                {
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        NoMoreEnemies_StandardBounce(go);
                        return;
                    }
                    continue;
                }

                Vector2 dir = sceneM.tanksTrans[i].position - trans.position;
                float d = dir.sqrMagnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                }
            }
        }

        //Rigidbody velocity
        v = targetDir.normalized * movespeed;
        SetVelocity(v);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "Enemy count: " + enemyM.activeEnemies.Count);
        GUI.Label(new Rect(20, 40, 200, 20), "my bullets: " + bulletTracker.activeBulletCount);
    }

    void NoMoreEnemies_StandardBounce (GameObject go)
    {
        if (go != null)
        {
            SquareReflect_isVerticalBounce(go);
        }
        else
        {
            StandardWallBounce();
        }
    }


    bool SquareReflect_isVerticalBounce(GameObject go) //No enemy seeking
    {
        Vector3 otherPos = go.transform.position;
        Vector3 vel = rb.velocity;
        float xDiff = otherPos.x - trans.position.x;
        float yDiff = otherPos.y - trans.position.y;
        //Debug.Log("xDiff: " + xDiff + ", yDiff: " + yDiff + ", vel: " + vel);
        //Debug.Break();
        //Debug.DrawLine(trans.position, otherPos, Color.red, 5f);

        if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
        {
            vel.y = -vel.y;
            SetVelocity(vel);
            return true;
        }
        else
        {
            vel.x = -vel.x;
            SetVelocity(vel);
            return false;
        }
    }

    void StandardWallBounce () //No enemy seeking
    {
        
        if ((pos.x > BG_Bound_maxX && velocity.x > 0) || (pos.x < BG_Bound_minX && velocity.x < 0))
        {
            ResetRecentEnemy();

            velocity.x = -velocity.x;
            //Debug.Log("hits left or right");

            HitSides();
            StartCoroutine(IncrementBounce());
            SetVelocity(velocity);
        }
        else if ((pos.y > BG_Bound_maxY && velocity.y > 0) || (pos.y < BG_Bound_minY && velocity.y < 0))
        {
            ResetRecentEnemy();

            velocity.y = -velocity.y;
            //Debug.Log("hits top or bot");

            HitSides();
            StartCoroutine(IncrementBounce());
            SetVelocity(velocity);
        }
    }
    #endregion

    #region Util
    void SetVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    bool canBounce = true;
    IEnumerator IncrementBounce()
    {
        if (canBounce)
        {
            canBounce = false;

            bouncesRemain--;
            if (bouncesRemain <= 0)
            {
                DestroyBullet();
            }
            yield return new WaitForSeconds(0.1f);
            canBounce = true;
        }
    }

    void ResetRecentEnemy ()
    {
        recentHitPlayerIndex = -1;
        recentHitEnemy = null;
    }
    #endregion
}

/*
 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note 1. TWIRLING PATTERN, should be an off axis paint point. 2. Sprite (the graphics) is a seperate transform component. 
public class Bullet_Bounce : BulletBase
{
    public Transform spriteTrans;

    float movespeed = 2.6f; //5

    int maxBounce = 2;
    int curBounce = 0;

    Vector3 stg2_scale = new Vector3(0.10f, 0.10f, 0.10f);
    Vector3 stg1_scale = new Vector3(0.10f, 0.10f, 0.10f);

    Vector3 vel;

    FightSceneManager sceneM;
    int recentHitPlayerIndex = -1;
    GameObject recentHitEnemy;
    int enemyCount;

    int paintCircleCountdown = 10;
    bool canBounce = true;

    void Awake()
    {
        base.OnAwake();
        sceneM = FightSceneManager.instance;
    }

    //IEnumerator TickCanBounceCD()
    //{
    //    canBounce = false;
    //    yield return new WaitForSeconds(0f);
    //    canBounce = true;
    //}

    IEnumerator DelayedDestroy()
    {
        //Auto destroy self
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

    void Update()
    {
        List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
        IntXY pixel = new IntXY();
        foreach (Transform t in pointTransforms)
        {
            pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
            if (!painted.Contains(pixel))
            {
                painted.Add(pixel);
                _toPaint.Add(pixel);
            }
        }

        BG_Painter.PaintBulletPoints(_toPaint, index);
    }
    
    void FixedUpdate()
    {
        vel = rb.velocity;

        WallBounceUpdate();
        spriteTrans.Rotate(new Vector3(0f, 0f, 600f * Time.deltaTime));
    }

    void WallBounceUpdate()
    {
        pos = trans.position;
        //Hit right or hits left
        if ((pos.x > BG_Bound_maxX && vel.x > 0) ||
            (pos.x < BG_Bound_minX && vel.x < 0) ||
            (pos.y > BG_Bound_maxY && vel.y > 0) ||
            (pos.y < BG_Bound_minY && vel.y < 0))
        {
            recentHitPlayerIndex = -1;
            recentHitEnemy = null;
            HitSides();
            TurnToClosestEnemy(null);
            trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        }
    }

    Vector2 targetDir;
    void TurnToClosestEnemy (GameObject go, bool squareObstacleBounce = false)
    {
        //Debug.Log("TurnToClosestEnemy: " + go + ", valid enemies: " + sceneM.validEnemies.Count);
        StartCoroutine(IncrementBounce());
        float shortestDist = float.MaxValue;
        Vector3 v;      

        //Find the enemy that you are closest to
        if (GM.gameMode != GameMode.Coop_Arcade)
        {
            enemyCount = sceneM.validPlayers.Count;
            //If ran out of enemy (last man standing) then normal deflect, or if there is only 1 enemy and the bullet just bounced off him
            if (sceneM.validPlayers.Count <= 0)
            {
                if (squareObstacleBounce)
                {
                    SquareObstacleReflect(go);
                }
                else
                {
                    StandardCircularReflect(go);
                }                
                return;
            }
            //else if (sceneM.validEnemies.Count == 1 && sceneM.validEnemies[0] == hitEnemyIndex)
            //{
            //    Debug.Log("hib");
            //    StandardDeflect(go);
            //    return;
            //}
            
            foreach (int i in sceneM.validPlayers)
            {
                if (i == recentHitPlayerIndex || i == index) //Ignore recent hit enemy and self. If no more enemies, then forget finding next enemy and just bounce back
                {
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        if (squareObstacleBounce)
                        {
                            SquareObstacleReflect(go);
                        }
                        else
                        {
                            StandardCircularReflect(go);
                        }
                        return;
                    }
                    continue;
                }

                Vector2 dir = sceneM.tanksTrans[i].position - trans.position;
                float d = dir.magnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                    //enemyIndex = i;
                }
            }
        }
        else
        {
            enemyCount = enemyM.activeEnemies.Count;

            if (enemyCount <= 0)
            {
                StandardCircularReflect(go);
                return;
            }

            foreach (GameObject e in enemyM.activeEnemies)
            {                
                if (e == recentHitEnemy) //Ignore recent hit enemy and self. If no more enemies, then forget finding next enemy and just bounce back
                {
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        StandardCircularReflect(go);
                        return;
                    }
                    continue;
                }
                //Debug.DrawLine(e.transform.position, transform.position, Color.red, 5f);
                //Debug.Log(enemyCount);
                Vector2 dir = e.transform.position - trans.position;
                float d = dir.magnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                    //enemyIndex = i;
                }
            }
        }

        //Rigidbody velocity
        v = targetDir.normalized * movespeed;
        rb.velocity = v;
        //Rotate transform
        //float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //StartCoroutine(IncrementBounce());
    }

    void StandardCircularReflect (GameObject go) 
    {
        //Debug.Log("other exists? " + go);
        //Debug.Log("BG_Bound_maxY: " + BG_Bound_maxY);
        //Debug.Log("BG_Bound_minY: " + BG_Bound_minY);
        //Debug.Log("vel.y: " + vel.y);

        if (go != null)
        {
            //V2 reflect must have normalized normal, otherwise it is unreliable
            vel = Vector2.Reflect(vel, (trans.position -go.transform.position).normalized).normalized * movespeed;
            //Debug.Break();
        }
        else
        {
            if (pos.x > BG_Bound_maxX && vel.x > 0) //Hits right
            {
                //Debug.Log("a");
                vel.x = -vel.x;
            }
            else if (pos.x < BG_Bound_minX && vel.x < 0) //Hits left
            {
                //Debug.Log("b");
                vel.x = -vel.x;
            }
            //Vel y positive = moving up. BG_Bound_maxY is positive value
            else if (pos.y > BG_Bound_maxY && vel.y > 0) //Hits top
            {
                //Debug.Log("c");
                vel.y = -vel.y;
            }
            else if (pos.y < BG_Bound_minY && vel.y < 0) //Hits bot
            {
                //Debug.Log("d");
                vel.y = -vel.y;
            }
        }
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    void SquareObstacleReflect (GameObject go)
    {
        Vector3 otherPos = go.transform.position;
        float xDiff = otherPos.x - trans.position.x;
        float yDiff = otherPos.y - trans.position.y;
        //Debug.Log("xDiff: " + xDiff + ", yDiff: " + yDiff + ", vel: " + vel);
        //Debug.Break();
        //Debug.DrawLine(trans.position, otherPos, Color.red, 5f);

        if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
        {
            vel.y = -vel.y;
        }
        else
        {
            vel.x = -vel.x;
        }
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        return;
        //Other is above / below
        if (yDiff > 0 && vel.y > 0 || yDiff < 0 && vel.y < 0) 
        {
            //If xDiff is shorter, then this is approaching from up/down
            if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
            {
                vel.y = -vel.y;
            }
            else
            {
                vel.x = -vel.x;
            }
        }
        else if (xDiff > 0 && vel.x > 0 || xDiff < 0 && vel.x < 0) //Other is on the right
        {
            //If xDiff is shorter, then this is approaching from up/down
            if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
            {
                vel.y = -vel.y;
            }
            else
            {
                vel.x = -vel.x;
            }
        }

        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    //PUBLIC
    public override void Shoot(int index)
    {
        this.index = index;

        maxBounce = 3;

        rb.velocity = movespeed * transform.up;
        trans.localScale = stg1_scale;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null && canBounce)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>(); //Ref enemy script

                if (enemyPlayer.index != index) //Don't hit owner
                {
                    HitNPCEffect(go, true);

                    recentHitPlayerIndex = enemyPlayer.index;
                    if (GM.gameMode == GameMode.Coop_Arcade)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                    }
                    else
                    {
                        enemyPlayer.GetsHitByAttack(trans.position, index);
                    }
                    if (GM.gameMode == GameMode.Coop_Arcade)
                    {
                        GameObject.Instantiate(refs.Pfx_HitSparkB_BlackVersion, trans.position, Quaternion.identity);
                    }
                    else
                        GameObject.Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
                    TurnToClosestEnemy(go);
                    trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

                    //StartCoroutine(TickCanBounceCD());
                }
            }
            //If collided with an obstacle
            else if (go.layer == GM.layerDeadTank)
            {
                HitDeadTankEffect(go, true);

                recentHitPlayerIndex = -1;
                TurnToClosestEnemy(go);
                trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

                //StartCoroutine(TickCanBounceCD());
            }
            else if (go.layer == GM.layerObstacle)
            {
                HitObstacleEffect(go, true);

                recentHitPlayerIndex = -1;
                TurnToClosestEnemy(go, true);
                trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

                //StartCoroutine(TickCanBounceCD());
            }
            else if (go.layer == GM.layerBullet)
            {
                HitBulletEffect(go);

                recentHitPlayerIndex = -1;
                Vector2 dirAway = pos - (Vector2)col.transform.position;
                rb.velocity = dirAway.normalized * movespeed;
                trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
                //StandardCircularReflect(go);
                //StartCoroutine(TickCanBounceCD());
            }
            else if (go.layer == GM.layerEnemy)
            {
                //Debug.Log("Bounce bullet hit Enemy");
                HitNPCEffect(go, true);
                recentHitEnemy = go;
                //Debug.Log("recentHitEnemy" + recentHitEnemy + "index" + index);
                recentHitEnemy.GetComponent<EnemyBase>().TakeDamage(index, 1);
                TurnToClosestEnemy(go);
                trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

                //StartCoroutine(TickCanBounceCD());
            }
        }
    }

    void IncrementBounce ()
    {
        curBounce++;
        if (curBounce > maxBounce)
        {
            Destroy(gameObject);
        }
    }
}
     */
