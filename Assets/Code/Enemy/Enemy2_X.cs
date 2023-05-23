using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2_X : EnemyBase
{
    public GameObject bullet;

    //Rotation
    Vector3 targetDir;
    Vector3 curDir;

    //Shooting
    Vector3 shootDir;

    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 1f;
        rotSpeed = 0.7f;
        HP = MaxHP = 3;
    }

    protected override void ActivationAdditionalEffect()
    {
        curDir = targetDir = trans.up;
        StartCoroutine(IntervalUpdate());
        rb.velocity = moveSpeed * trans.up;
    }

    public void Update()
    {
        pos = trans.position;
        DrawingUpdate();
    }

    public void FixedUpdate()
    {
        OutOfBoundsUpdate();

        //Move forward
        if (onScreenNow)
        {
            //Target direction
            curDir = Vector3.RotateTowards(curDir, targetDir, rotSpeed, 0.0f);

            //Rotate
            //trans.rotation = Quaternion.Euler(curDir);
            trans.rotation = Quaternion.LookRotation(Vector3.forward, curDir);
            rb.velocity = moveSpeed * trans.up;
            //rb.angularVelocity = rotAmount * rotSpeed * Time.deltaTime;
        }
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 80, 200, 20), "curDir: " + curDir);
    //    GUI.Label(new Rect(20, 100, 200, 20), "targetDir: " + targetDir);
    //}

    void OutOfBoundsUpdate ()
    {
        if (pos.x <= BG_Bound_minX)
        {
            targetDir = Vector3.right;
            curDir = targetDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * targetDir;
        }
        else if (pos.x >= BG_Bound_maxX)
        {
            targetDir = Vector3.left;
            curDir = targetDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * targetDir;
        }
        else if (pos.y <= BG_Bound_minY)
        {
            targetDir = Vector3.up;
            curDir = targetDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * targetDir;
        }
        else if (pos.y >= BG_Bound_maxY)
        {
            targetDir = Vector3.down;
            curDir = targetDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * targetDir;
        }
    }

    IEnumerator IntervalUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 4f));

            //Random angle
            targetDir = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * targetDir;

            if (Random.value > 0.8f)
            {
                //Shooting
                //Find cloest enemy
                float shortestDist = float.MaxValue;
                pos = trans.position;
                bool hasTarget = false;

                foreach (int i in sceneM.validPlayers)
                {
                    Vector2 dir = sceneM.tanksTrans[i].position - pos;
                    float dist = dir.magnitude;
                    if (dist < shortestDist)
                    {
                        shootDir = dir;
                        shortestDist = dist;
                        hasTarget = true;
                    }
                }

                if (hasTarget)
                {
                    Instantiate(bullet, pos, Quaternion.LookRotation(Vector3.forward, shootDir)).GetComponent<BulletBase>().Shoot(GM.enemyIndex, null);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    { DefaultTriggerEnter(col); }
}