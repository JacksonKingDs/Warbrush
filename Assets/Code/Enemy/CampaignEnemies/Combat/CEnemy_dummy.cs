using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemy_dummy : CampaignEnemyBase
{
    public LayerMask rayHitLayer;

    void Awake()
    {
        base.OnAwake();
    }

    IEnumerator Start()
    {
        base.OnStart();

        moveSpeed = 2f;
        MaxHP = 3;
        HP = MaxHP;


        yield return new WaitForSeconds(3f);

        StartCoroutine(IntervalUpdate());
        gameStarted = true;
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

    bool hasTarget;
    bool visionClear;
    IEnumerator IntervalUpdate()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            //Shooting
            //Find cloest enemy
            float shortestDist = float.MaxValue;
            pos = trans.position;
            hasTarget = false;

            foreach (int i in sceneM.validPlayers)
            {
                Vector2 dir = sceneM.tanksTrans[i].position - pos;
                float dist = dir.magnitude;
                if (dist < shortestDist)
                {
                    tgtDir = dir;
                    shortestDist = dist;
                    hasTarget = true;
                }
            }

            if (hasTarget)
            {
                //If direction is clear
                RaycastHit2D hit = Physics2D.Raycast(pos, tgtDir, 15f, rayHitLayer);
                if (hit.collider != null && hit.collider.gameObject.layer == GM.layerPlayer)
                {
                    visionClear = true;
                    rb.velocity = moveSpeed * tgtDir.normalized;
                }
                else
                {
                    visionClear = false;
                    rb.velocity = Vector3.zero;
                }
            }
            else
            {
                visionClear = false;
                rb.velocity = Vector3.zero;
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
