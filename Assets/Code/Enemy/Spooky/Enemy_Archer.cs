using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Archer : EnemyBase
{
    public GameObject pf_arrow;
    Transform enemyTrans;

    public override void Initialization()
    {
        BaseInitialization();
        enemyColor = BG_Painter.enemy_body;
        spriteRend.color = enemyColor;

        //Initialize
        moveSpeed = 0.2f;
        HP = MaxHP = 2;
    }

    protected override void ActivationAdditionalEffect()
    {
        rb.velocity = moveSpeed * trans.up;
        StartCoroutine(IntervalUpdate());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    Vector3 curDir;
    IEnumerator IntervalUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            pos = trans.position;
            ////Debug.DrawRay(trans.position, trans.up, Color.green, 5f);
            if (sceneM.validPlayers.Count <= 0)
            {
                CheckOutOfBounds();
            }
            else
            {
                UpdateDirToClosestEnemy();
                rb.velocity = Vector3.zero;
                
                yield return new WaitForSeconds(2f);
                Shoot();
                yield return new WaitForSeconds(3f);
            }

            SetNewVelocity(trans.up * moveSpeed);
        }
        Debug.Log("quits");
    }

    void Shoot ()
    {
        Instantiate(pf_arrow, pos, Quaternion.LookRotation(Vector3.forward, curDir)).GetComponent<BulletBase>().Shoot(GM.enemyIndex, null);
    }

    void UpdateDirToClosestEnemy ()
    {
        float shortestDist = float.MaxValue;
        foreach (int i in sceneM.validPlayers)
        {
            Vector2 dir = sceneM.tanksTrans[i].position - pos;
            float d = dir.magnitude;
            if (d < shortestDist)
            {
                curDir = dir;
                shortestDist = d;
                enemyTrans = sceneM.tanksTrans[i];
            }
        }
    }

    void CheckOutOfBounds ()
    {
        //rb.velocity = moveSpeed * trans.up;
        Vector3 vel = rb.velocity;
        if (pos.x > BG_Bound_maxX && vel.x > 0) //Hits right
        {
            vel.x = -vel.x;
            curDir = vel;
            //SetNewVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX && vel.x < 0) //Hits left
        {
            vel.x = -vel.x;
            curDir = vel;
            //SetNewVelocity(vel);
        }
        //Vel y positive = moving up. BG_Bound_maxY is positive value
        else if (pos.y > BG_Bound_maxY && vel.y > 0) //Hits top
        {
            vel.y = -vel.y;
            curDir = vel;
            //SetNewVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY && vel.y < 0) //Hits bot
        {
            vel.y = -vel.y;
            curDir = vel;
            //SetNewVelocity(vel);
        }
    }

    void SetNewVelocity (Vector3 vel)
    {
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, vel);
    }
}