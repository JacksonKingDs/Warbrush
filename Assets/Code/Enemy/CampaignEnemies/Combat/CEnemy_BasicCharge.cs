using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemy_BasicCharge : CampaignEnemyBase
{
    public LayerMask rayHitLayer;
    public bool stationaryAfterDiscover = false;
    Vector3 lastSeenPos;

    void Awake()
    {
        base.OnAwake();
    }

    IEnumerator Start()
    {
        base.OnStart();

        patrolIndex = 0;
        currentPatrolDestination = patrolPoints[0];
        patrolSpeed = 0.8f;

        moveSpeed = 1.5f;        
        MaxHP = 3;
        HP = MaxHP;
        rotSpeed = 0.5f;

        curDir = currentPatrolDestination - (Vector2)trans.position;

        yield return new WaitForSeconds(3f);
        gameStarted = true;
        StartCoroutine(IntervalUpdate());
    }

    void Update()
    {
        if (!gameStarted)
            return;

        List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
        IntXY pixel = new IntXY();
        foreach (Transform t in paintPoints)
        {
            pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
            if (!painted.Contains(pixel))
            {
                painted.Add(pixel);
                _toPaint.Add(pixel);
            }
        }

        BG_Painter.PaintBulletPoints(_toPaint, GM.enemyIndex);
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        //If no target, then patrol.
        if (!hasTarget)
        {
            if (discovered)
            {
                if (Vector2.Distance(transform.position, lastSeenPos) < 0.2f)
                {
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    rb.velocity = (lastSeenPos - transform.position).normalized * moveSpeed;
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    bool hasTarget = false;
    bool discovered = false;
    IEnumerator IntervalUpdate()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            hasTarget = false;
            float shortestDist = float.MaxValue;
            pos = trans.position;
            Vector3 enemyPos;

            //Raycast towards all available players and find the closest one.
            foreach (int i in sceneM.validPlayers)
            {
                enemyPos = sceneM.tanksTrans[i].position;
                Vector2 dir = enemyPos - pos;

                //If direction is clear, THEN calculate if this enemy is closest
                RaycastHit2D hit = Physics2D.Raycast(pos, dir, 10f, rayHitLayer);
                if (hit.collider != null && hit.collider.gameObject.layer == GM.layerPlayer)
                {
                    float dist = dir.sqrMagnitude;
                    if (dist < shortestDist)
                    {
                        tgtDir = dir;
                        shortestDist = dist;
                        hasTarget = true;
                        rb.velocity = moveSpeed * tgtDir.normalized;
                        lastSeenPos = enemyPos;
                        discovered = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        DefaultCollisionEnter(col);
    }
}

enum EnemyStates_Patroller
{
    Patrol,
    Chase,
    Still
}