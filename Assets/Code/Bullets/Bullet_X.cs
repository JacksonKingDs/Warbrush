using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_X : BulletBase
{
    public Transform spriteTrans;

    float movespeed = 1f; //5

    Vector3 vel;

    #region Init
    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;

        rb.velocity = movespeed * transform.up;
        StartCoroutine(WallBounceUpdate());

        //Rotate
        //Vector3 v = rb.velocity;
        //float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

    void Update()
    {
        List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
        IntXY pixel = new IntXY();
        foreach (Transform t in pointTransforms)
        {
            pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
            if (!painted.Contains(pixel))
            {
                painted.Add(pixel);
                _toPaint.Add(pixel);
            }
        }

        BG_Painter.PaintBulletPoints(_toPaint, index);
    }

    void FixedUpdate()
    {
        spriteTrans.Rotate(new Vector3(0f, 0f, 10f));
    }

    IEnumerator WallBounceUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            Vector3 pos = trans.position;
            vel = rb.velocity;
            //Hit right or hits left
            if ((pos.x > BG_Bound_maxX && vel.x > 0) ||
                (pos.x < BG_Bound_minX && vel.x < 0) ||
                (pos.y > BG_Bound_maxY && vel.y > 0) ||
                (pos.y < BG_Bound_minY && vel.y < 0))
            {
                //HitSides();
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
                //if (go.GetComponent<BulletBase>().index != index)
                //    Destroy(gameObject);
                //HitBulletEffect(go);
                Destroy(gameObject);
            }
        }
    }
}
