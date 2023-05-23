using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Pixel : BulletBase
{
    Vector3 vel;

    float movespeed = 5f;

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;
        bulletTracker = behavior;

        vel = movespeed * transform.up;
        //trans.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        //InitializeRendererColor();

        rb.velocity = vel;

        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }
    }

    void Update()
    {
        //Paint light
        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogMid(trans.position);

        //Painting trail
        if (!isSpaceMode)
        {
            List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
            foreach (Transform t in pointTransforms)
            {
                IntXY pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
                if (!painted.Contains(pixel))
                {
                    painted.Add(pixel);
                    _toPaint.Add(pixel);
                }
            }
            BG_Painter.PaintBulletPoints(_toPaint, index);
        }
        else
        {
            BG_Painter.AddHanabiTrailShort(trans.position, index);
            //refs.SpawnSpaceDustPinPoint(transform.position, index);
        }
    }
    
    void FixedUpdate()
    {
        Vector3 pos = trans.position;

        //Destroy when hitting border
        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            HitSides();
            DestroyBullet();
        }
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
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>();

                if (enemyPlayer.index != index)
                {
                    HitNPCEffect(go, true);
                    if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch || GM.gameMode == GameMode.Campaign)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                    }
                    else
                    {
                        enemyPlayer.GetsHitByAttack(trans.position, index);
                    }
                    DestroyBullet();
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

                DestroyBullet();
            }
            else if (go.layer == GM.layerBullet)
            {
                HitBulletEffect(go, movespeed);
            }
            else if (go.layer == GM.layerEnemy)
            {
                HitNPCEffect(go, true);

                go.GetComponent<IEnemy>().TakeDamage(index, 3);
                DestroyBullet();
            }
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
        }
    }
}