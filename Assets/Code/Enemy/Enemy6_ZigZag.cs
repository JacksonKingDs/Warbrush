using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy6_ZigZag : EnemyBase
{
    bool despawning = false;
    int countToDirChange;
    const int countMax = 30;

    public override void Initialization()
    {
        BaseInitialization();
        moveSpeed = 1.5f;

        HP = MaxHP = 3;
    }

    protected override IEnumerator Spawning()
    {
        onScreenNow = false;
        yield return new WaitForSeconds(0.6f);
        onScreenNow = true;
    }

    protected override void ActivationAdditionalEffect()
    {
        //Dir = 0;
        countToDirChange = countMax;
        despawning = false;
        rb.velocity = moveSpeed * trans.up;
    }

    public void Update()
    {
        pos = trans.position;
        DrawingUpdate();
        HitSideDestroyUpdate();
    }

    //public void FixedUpdate()
    //{
    //    if (onScreenNow)
    //    {
    //        //Move forward
    //        if (countToDirChange > 0)
    //        {
    //            countToDirChange--;
    //        }
    //        else
    //        {
    //            DirectionUpdate();
    //            countToDirChange = countMax;
    //        }
    //    }
    //}

    void OnTriggerEnter2D(Collider2D col)
    { DefaultTriggerEnter(col); }    
   
    protected void HitSideDestroyUpdate()
    {
        if (despawning || !onScreenNow)
            return;

        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            despawning = true;
            StartCoroutine(Despawning());
        }
    }

    IEnumerator Despawning()
    {
        yield return new WaitForSeconds(1f);
        enemyM.ReturnToPool(gameObject);
    }

    //int Dir = 0;
    //void DirectionUpdate ()
    //{
    //    switch (Dir)
    //    {
    //        case 0: //
    //            transform.rotation *= Quaternion.Euler(0, 0, -90);
    //            break;
    //        case 1:
    //            transform.rotation *= Quaternion.Euler(0, 0, 90);
    //            break;
    //        case 2:
    //            transform.rotation *= Quaternion.Euler(0, 0, 90);
    //            break;
    //        case 3:
    //        default:
    //            transform.rotation *= Quaternion.Euler(0, 0, -90);
    //            break;
    //    }

    //    Dir ++;
    //    if (Dir >= 4)
    //    {
    //        Dir = 0;
    //    }

    //    rb.velocity = moveSpeed * trans.up;
    //}

    public override void TakeDamage(int index, int dmg = 1)
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
                //Dead
                if (index >= 0 && index < 4)
                    FightSceneManager.AddKillScore(index);
                trans.position = offscreen;
                enemyM.ReturnToPool(gameObject);
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    transform.rotation *= Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    transform.rotation *= Quaternion.Euler(0, 0, -90);
                }
                rb.velocity = moveSpeed * trans.up;

                StartCoroutine(GetHitBlink());
            }
        }
    }
}