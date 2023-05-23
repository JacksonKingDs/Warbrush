using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy4_SmallArrow : EnemyBase
{
    Vector3 targetDir;
    Transform enemyTrans;

    public override void Initialization()
    {
        BaseInitialization();

        //Initialize
        moveSpeed = 1f;
        rotSpeed = 0.6f;
        HP = MaxHP = 2;
        //rb.velocity = moveSpeed * trans.up;

        //Ref
        sceneM = FightSceneManager.instance;
    }

    protected override void ActivationAdditionalEffect()
    {
        rb.velocity = moveSpeed * trans.up;
        
        StartCoroutine(AdjustDirection());
    }

    public void Update()
    {
        pos = trans.position;
        DrawingUpdate();}

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    IEnumerator AdjustDirection()
    {
        while(true)
        {
            //If no enemy, just move forward and bounce off walls.
            if (sceneM.validPlayers.Count <= 0)
            {
                CheckOutOfBounds();
                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                //Find cloest enemy
                float shortestDist = float.MaxValue;
                foreach (int i in sceneM.validPlayers)
                {
                    Vector2 dir = sceneM.tanksTrans[i].position - pos;
                    float d = dir.magnitude;
                    if (d < shortestDist)
                    {
                        targetDir = dir;
                        shortestDist = d;
                        enemyTrans = sceneM.tanksTrans[i];
                    }
                }

                targetDir = Vector3.RotateTowards(trans.up, targetDir, rotSpeed, 0.0f);
                trans.rotation = Quaternion.LookRotation(Vector3.forward, targetDir);
                rb.velocity = moveSpeed * trans.up;

                //Only do this update once per sec, otherwise too expensive.
                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void CheckOutOfBounds ()
    {
        rb.velocity = moveSpeed * trans.up;
        Vector3 vel = rb.velocity;
        if (pos.x > BG_Bound_maxX && vel.x > 0) //Hits right
        {
            vel.x = -vel.x;
            SetNewVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX && vel.x < 0) //Hits left
        {
            vel.x = -vel.x;
            SetNewVelocity(vel);
        }
        //Vel y positive = moving up. BG_Bound_maxY is positive value
        else if (pos.y > BG_Bound_maxY && vel.y > 0) //Hits top
        {
            vel.y = -vel.y;
            SetNewVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY && vel.y < 0) //Hits bot
        {
            vel.y = -vel.y;
            SetNewVelocity(vel);
        }
    }

    void SetNewVelocity (Vector3 vel)
    {
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
}

//Maybe just rotate straight towards the player instead?

/*
 IEnumerator AdjustDirection()
{
    while(true)
    {
        //If no enemy, just move forward and bounce off walls.
        if (sceneM.validPlayers.Count <= 0)
        {
            CheckOutOfBounds();
            continue;
        }

        //Find cloest enemy
        float shortestDist = float.MaxValue;
        foreach (int i in sceneM.validPlayers)
        {
            Vector2 dir = sceneM.tanksTrans[i].position - pos;
            float d = dir.magnitude;
            if (d < shortestDist)
            {
                //targetDir = dir;
                shortestDist = d;
                enemyTrans = sceneM.tanksTrans[i];
            }
        }

        //Find which dir: left, right, up, down leads to the enemy
        Vector3 enemyPos = enemyTrans.position;

        bool enemyAbove = enemyPos.y > pos.y;
        bool enemyRight = enemyPos.x > pos.x;
        bool goHorizontal = Mathf.Abs(enemyPos.x - pos.x) > Mathf.Abs(enemyPos.y - pos.y);

        if (goHorizontal)
        {
            if (enemyRight)
            {
                targetDir = Vector3.right;
            }
            else
            {
                targetDir = Vector3.left;
            }
        }
        else
        {
            if (enemyAbove)
            {
                targetDir = Vector3.up;
            }
            else
            {
                targetDir = Vector3.down;
            }
        }
        targetDir = Vector3.RotateTowards(trans.up, targetDir, rotSpeed, 0.0f);
        trans.rotation = Quaternion.LookRotation(Vector3.forward, targetDir);
        rb.velocity = moveSpeed * trans.up;

        //Only do this update once per sec, otherwise too expensive.
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
    }
}
 */
