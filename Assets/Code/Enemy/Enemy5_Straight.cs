using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy5_Straight : EnemyBase
{
    bool despawning = false;

    public override void Initialization()
    {
        BaseInitialization();
        moveSpeed = 0.8f;

        HP = MaxHP = 2;
    }

    protected override void ActivationAdditionalEffect()
    {
        despawning = false;
        rb.velocity = moveSpeed * trans.up;
    }

    public void Update()
    {
        pos = trans.position;
        DrawingUpdate();
        HitSideDestroyUpdate();
    }

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
        yield return new WaitForSeconds(2f);
        enemyM.ReturnToPool(gameObject);
    }
}