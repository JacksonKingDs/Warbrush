using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Grenade : BulletBase
{
    Vector3 stg2_scale = new Vector3(0.10f, 0.10f, 0.10f);
    Vector3 stg1_scale = new Vector3(0.05f, 0.05f, 0.05f);

    float movespeed = 4f;
    Vector3 vel;

    const float stg1_explosionInterval = 0.2f;
    const float stg2_explosionInterval = 0.4f;
    float autoExplodeInterval;
    float autoExplodeCounter;
    int explosions;

    const float TERRAIN_COL_INTERVAL = 0.1f; //Do not allow for continuous collision with terrain
    float terrainColCounter = 0f;

    AudioManager audioM;
    FightSceneManager sceneM;
    InputManager input;

    List<int> hitTargets = new List<int>();


    //Only draw line in between 2 circles
    //int paintTick;
    //int paintStartTick = 8;
    //int paintEndTick = 14;

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();

        this.index = index;
        bulletTracker = behavior;

        sceneM = FightSceneManager.instance;
        input = InputManager.Instance;
        audioM = AudioManager.instance;

        explosions = 4;
        vel = movespeed * transform.up;
        trans.localScale = stg2_scale;
        autoExplodeInterval = stg2_explosionInterval;
        autoExplodeCounter = autoExplodeInterval;

        rb.velocity = vel;

        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }

        //InitializeRendererColor();
    }

    private void Update()
    {
        if (autoExplodeCounter > 0f)
        {
            autoExplodeCounter -= Time.deltaTime;
        }
        else
        {
            Explode();
        }

        if (terrainColCounter >= 0f)
        {
            terrainColCounter -= Time.deltaTime;
        }

        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogSml(trans.position);

        if (isSpaceMode)
            BG_Painter.AddHanabiTrailShort(trans.position, index);
    }

    void RefreshExplodeCounter ()
    {
        autoExplodeCounter = autoExplodeInterval;
        terrainColCounter = TERRAIN_COL_INTERVAL;
    }

    void FixedUpdate()
    {
        Vector3 pos = trans.position;
        //Debug.Log(pos);

        //Destroy when hitting border
        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            HitSides();
            Destroy(gameObject);
        }
    }


    void Explode()
    {
        RefreshExplodeCounter();

        audioM.Spawn_explode2();

        GameObject ring = Instantiate(SettingsAndPrefabRefs.instance.Pf_GrenadeExplode, trans.position, trans.rotation) as GameObject;

        ring.GetComponent<ExplosionCircle>().Shoot(index, bulletTracker);
                
        explosions--;
        if (explosions <= 0)
        {
            Destroy(gameObject);
        }

        AudioManager.instance.Spawn_Hits3();

        //Change rotation based on parent
        if (index >= 0 && index < 4)
        {
            Transform t = sceneM.tanksTrans[index];
            if (t.gameObject.activeSelf)
            {
                trans.rotation = t.rotation;
                vel = movespeed * t.up;
                rb.velocity = vel;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (terrainColCounter <= 0f && col != null)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>();

                if (enemyPlayer.index != index)
                {
                    if (!hitTargets.Contains(enemyPlayer.index))
                    {
                        hitTargets.Add(enemyPlayer.index);
                        Explode();
                        HitNPCEffect(go, false);
                    }
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
                Explode();
            }
            else if (go.layer == GM.layerEnemy)
            {
                // Grenade does not need to play the full effects !
                HitNPCEffect(go, false);
                Explode();
            }

            else if (go.layer == GM.layerBullet)
            {
                HitBulletEffect(go, movespeed);
                Explode();
                //////Deflect
                //Vector2 dirAway = pos - (Vector2)col.transform.position;
                //rb.velocity = dirAway.normalized * movespeed;
                //trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

                //Debug.Break();
                //Debug.DrawRay(pos, dirAway * 10f, Color.red, 10f);

                //Vector2 dirAway = pos - (Vector2)col.transform.position;
                //rb.velocity = dirAway.normalized * movespeed;
            }
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
        }
    }
}