using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_EnemeyArrow : BulletBase
{
    public Transform spriteTrans;
    float movespeed = 0.5f; 
    Vector3 vel;

    #region Init

    void Update()
    {
        //BG_Painter.Bullet_ClearSpookyFogSml(trans.position);
    }

    void FixedUpdate()
    {
        spriteTrans.Rotate(new Vector3(0f, 0f, 8f));
    }

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;

        rb.velocity = movespeed * transform.up;
        StartCoroutine(OutOfBoundsCheck());
    }
    #endregion

    IEnumerator OutOfBoundsCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Vector3 pos = trans.position;
            vel = rb.velocity;

            if ((pos.x > BG_Bound_maxX && vel.x > 0) ||
                (pos.x < BG_Bound_minX && vel.x < 0) ||
                (pos.y > BG_Bound_maxY && vel.y > 0) ||
                (pos.y < BG_Bound_minY && vel.y < 0))
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>(); //Ref enemy script

                HitNPCEffect(go, true);

                enemyPlayer.GetsHitByAttack(trans.position, index);

                Destroy(gameObject);
            }
            //If collided with an obstacle
            else if (go.layer == GM.layerObstacle)
            {
                HitObstacleEffect(go);
                Destroy(gameObject);
            }
            else if (go.layer == GM.layerDeadTank)
            {
                HitDeadTankEffect(go);
                Destroy(gameObject);
            }
            else if (go.layer == GM.layerBullet)
            {
                //HitBulletEffect(go);
                Destroy(gameObject);
            }
        }
    }
}