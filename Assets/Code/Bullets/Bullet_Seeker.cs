using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Seeker : BulletBase
{
    float moveSpeed = 2f;
    float rotSpeed = 0.15f;
    float lifeTime = 8f;

    float bulletReflectCD = 0f;

    FightSceneManager sceneM;

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;
        bulletTracker = behavior;

        sceneM = FightSceneManager.instance;
        curDir = targetDir = transform.up;

        //InitializeRendererColor();

        StartCoroutine(DelayedDestroy());

        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }
    }

    IEnumerator DelayedDestroy()
    {
        //Auto destroy self
        yield return new WaitForSeconds(lifeTime);
        HitSides();
        DestroyBullet();
    }

    void Update()
    {
        DrawingUpdate();
        if (bulletReflectCD > 0f)
        {
            bulletReflectCD -= Time.deltaTime;
        }

        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogMid(trans.position);
    }

    int EnemyUpdateCounter = 0;

    //private void OnGUI()
    //{
        
    //    GUI.Label(new Rect(20, 20, 200, 20), "EnemyUpdateCounter: " + EnemyUpdateCounter);
    //    GUI.Label(new Rect(20, 40, 200, 20), "GM.gameMode: " + GM.gameMode);
    //}

    void FixedUpdate()
    {
        Vector2 pos = trans.position;

        //Destroy when hitting border
        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            HitSides();
            DestroyBullet();
        }

        EnemyUpdateCounter--; //
        if (EnemyUpdateCounter <= 0)
        {
            EnemyUpdateCounter = 10;

            FindDirToEnemy();
            //Debug.DrawRay(transform.position, curDir, Color.yellow, 1f);
            //Debug.DrawRay(transform.position, targetDir, Color.red, 1f);

            //Target direct
            curDir = Vector3.RotateTowards(curDir, targetDir, rotSpeed, 0.0f);

            //Moveforward
            //Vector2 rotatingDir = Vector2.rot
            rb.velocity = moveSpeed * curDir.normalized;

            //Rotate
            //trans.rotation = Quaternion.Euler(curDir);
            trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        }
    }

    void DrawingUpdate ()
    {
        if (!isSpaceMode)
        {
            //PAINTING
            List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update

            foreach (Transform t in pointTransforms)
            {
                IntXY pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);

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
            BG_Painter.AddHanabiTrailLong(trans.position, index);
        }
    }

    Vector2 targetDir;
    Vector2 curDir;
    //int enemyIndex;
    
    void FindDirToEnemy ()
    {
        float shortestDist = float.MaxValue;
        //int shortestIndex = 0;

        if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
        {
            foreach (GameObject e in enemyM.activeEnemies)
            {
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
        else if (GM.gameMode == GameMode.Campaign)
        {
            foreach (GameObject e in CampaignEnemyBase.enemies)
            {
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
        else
        {
            foreach (int i in sceneM.validPlayers)
            {
                if (i != index)
                {
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
        }
    }

  

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        //If collided with a player and they are not the same index as self...
        if (go.layer == GM.layerPlayer)
        {
            TankControllerBase hitPlayer = go.GetComponent<TankControllerBase>();
            if (hitPlayer.index != index)
            {
                HitNPCEffect(go, true);
                if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
                {
                    hitPlayer.GetsHitByAttackNoDmg(trans.position);
                }
                else
                {
                    hitPlayer.GetsHitByAttack(trans.position, index);
                }
                DestroyBullet();
            }
        }
        //If collided with an obstacle
        else if (go.layer == GM.layerObstacle)
        {
            HitObstacleEffect(go);
            DestroyBullet();
        }
        else if (go.layer == GM.layerDeadTank)
        {
            HitDeadTankEffect(go);
            DestroyBullet();
        }
        else if (go.layer == GM.layerEnemy)
        {
            HitNPCEffect(go, true);

            go.GetComponent<IEnemy>().TakeDamage(index, 3);
            DestroyBullet();
        }
        //else if (go.layer == GM.layerBullet && bulletReflectCD <= 0f)
        else if (go.layer == GM.layerBullet && bulletReflectCD <= 0f)
        {
            //Custom reflect logic
            AudioManager.instance.Spawn_Hits2();
            Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitSparkA, trans.position, trans.rotation);

            BulletBase bullet = go.GetComponent<BulletBase>();
            if (bullet.index != GM.enemyIndex)
            {
                //Deflect
                rb.velocity = -rb.velocity;
                curDir = rb.velocity;
                trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
                

                StartCoroutine(DelayedIndexChange(bullet.index));
            }

            bulletReflectCD = 0.2f;

            //AudioManager.instance.Spawn_Hits2();
            //Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitSparkA, pos, trans.rotation);

            ////Deflect
            //Vector2 dirAway = pos - (Vector2)col.transform.position;
            //curDir = targetDir = dirAway;
            //trans.rotation = Quaternion.LookRotation(curDir, rb.velocity);

            //rb.velocity = dirAway.normalized * moveSpeed;
            //trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        }
        else if (go.layer == GM.layerProp)
        {
            go.GetComponent<IProps>().PropInteraction(index);
        }
    }
}