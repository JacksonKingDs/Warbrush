using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_ShotgunSubbullet : BulletBase
{
    Vector3 vel;

    float movespeed = 13f;

    float startingScale = 0f; //starting size
    float targetScale = 0.2f;
    float lerpValue = 0f;

    float lifeTime = 0.26f;

    bool canPaint = false;

    float waitBeforePaint = 0.075f;
    float bulletReflectCD;

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        base.OnAwake();
        trans.localScale = new Vector3(startingScale, startingScale, startingScale);
        this.index = index;

        vel = movespeed * transform.up;

        rb.velocity = vel;

        //InitializeRendererColor();

        StartCoroutine(AutoKill());
        StartCoroutine(DelayBeforePaint());

        if (updateAura)
        {
            StartCoroutine(AuraPulse());
        }
    }

    IEnumerator AutoKill()
    {
        //Auto destroy self
        yield return new WaitForSeconds(lifeTime);
        DestroyBullet();
    }

    IEnumerator DelayBeforePaint()
    {
        yield return new WaitForSeconds(waitBeforePaint);
        canPaint = true;
    }

    void Update()
    {
        //Painting
        if (canPaint && !isSpaceMode)
        {
            //Loop through paint-points, see which pixel is still not yet painted, then send them through to painter to paint. 
            List<IntXY> _toPaint = new List<IntXY>();
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

        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogSml(trans.position);

        if (isSpaceMode)
        {
            BG_Painter.AddHanabiTrailShort(trans.position, index);
        }

        if (bulletReflectCD > 0f)
        {
            bulletReflectCD -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        //Expand
        lerpValue = lerpValue + Time.deltaTime / lifeTime ;
        float curScale = Mathf.Lerp(startingScale, targetScale, lerpValue);
        trans.localScale = new Vector3(curScale, curScale, curScale);

        //Destroy when hitting border
        Vector3 pos = trans.position;
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
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>(); //Ref enemy player's control script

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
            else if (go.layer == GM.layerBullet && bulletReflectCD <= 0f)
            {
                BulletBase otherBullet = go.GetComponent<BulletBase>();
                bulletReflectCD = 0.2f;

                if (otherBullet.index != index)
                {
                    HitBulletEffect(go, movespeed);

                    //Deflect
                    //Vector2 dirAway = pos - (Vector2)col.transform.position;
                    //rb.velocity = dirAway.normalized * movespeed;
                    //trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
                }
            }
            else if (go.layer == GM.layerEnemy)
            {
                HitNPCEffect(go, true);

                go.GetComponent<IEnemy>().TakeDamage(index, 2);
                DestroyBullet();
            }
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
        }
    }
}