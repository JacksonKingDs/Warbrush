using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_SmallBullet : BulletBase
{
    Vector3 stg1_scale = new Vector3(0.06f, 0.06f, 0.06f);

    Vector3 vel;

    float movespeed = 3f;
    float lifeTime = 0.3f;

    IEnumerator DelayedDestroy()
    {
        //Auto destroy self
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        Vector3 pos = trans.position;

        //Destroy when hitting border
        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            HitSides();
            Destroy(gameObject);
        }
    }

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;

        //InitializeRendererColor();

        vel = movespeed * transform.up;
        rb.velocity = vel;

        StartCoroutine(DelayedDestroy());
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            //GM.gameMode == GameMode.Brawl && 
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase otherPlayer = go.GetComponent<TankControllerBase>();

                if (otherPlayer.index != index)
                {
                    HitNPCEffect(go, false);
                    if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch || GM.gameMode == GameMode.Campaign)
                    {
                        otherPlayer.GetsHitByAttackNoDmg(trans.position);
                    }
                    else
                    {
                        otherPlayer.GetsHitByAttack(trans.position, index);
                    }
                    Destroy(gameObject);
                }
            }
            //If collided with an obstacle
            else if (go.layer == GM.layerObstacle)
            {
                //HitObstacleEffect(go, false);

                Destroy(gameObject);
            }
            else if (go.layer == GM.layerDeadTank)
            {
                //HitDeadTankEffect(go, false);

                Destroy(gameObject);
            }
            else if (go.layer == GM.layerBullet)
            {
                HitBulletEffect(go, movespeed);
                Destroy(gameObject);
            }
            else if (go.layer == GM.layerEnemy)
            {
                HitNPCEffect(go, false);

                go.GetComponent<IEnemy>().TakeDamage(index, 3);
                Destroy(gameObject);
            }
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
        }
    }
}