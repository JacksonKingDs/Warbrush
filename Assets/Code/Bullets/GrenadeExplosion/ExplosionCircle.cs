using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionCircle : BulletBase
{
    public ExplosionCircleDetector detectorCollider;
    //State

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        this.index = index;

        trans.localScale = trans.localScale * 0.8f;

        detectorCollider.Shoot(index, behavior);
        //InitializeRendererColor();

        StartCoroutine(DelayedDestroy());
    }

    void Paint()
    {
        //Paint perfect circle
        List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
        IntXY pixel = BGTextureManager.WorldPosToPixelPos_BG(trans.position);

        IntXY[] offsets = CircularOffset.Ring7;
        //IntXY[] inner = reachedFull ? CircularOffset.InnerRing7 : CircularOffset.InnerRing6; //Ignore the inner ones
        //foreach (IntXY t in inner)
        //{
        //    IntXY p = pixel + t;
        //    painted.Add(p);
        //}

        foreach (IntXY t in offsets)
        {
            IntXY p = pixel + t;
            //Debug.Log("p: " + p);
            if (!painted.Contains(p))
            {
                //Debug.Log("hi");
                painted.Add(p);
                _toPaint.Add(p);
            }
        }
        if (!isSpaceMode)
            BG_Painter.PaintBulletPoints(_toPaint, index);

        if (isSpookyMode)
            BG_Painter.Bullet_ClearSpookyFogLarge(trans.position);

        if (isDesert)
            BG_Painter.DesertGrenadeExplosion(trans.position);
    }

    public void IndirectTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>(); 

                if (enemyPlayer.index != index && !hitEnemies.Contains(enemyPlayer.index))
                {
                    hitEnemies.Add(enemyPlayer.index);
                    HitNPCEffect(go, true);
                    if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch || GM.gameMode == GameMode.Campaign)
                    {
                        enemyPlayer.GetsHitByAttackNoDmg(trans.position);
                    }
                    else
                    {
                        enemyPlayer.GetsHitByAttack(trans.position, index);
                    }
                }
            }
            else if (go.layer == GM.layerEnemy)
            {
                HitNPCEffect(go, true);
                go.GetComponent<IEnemy>().TakeDamage(index, 2);
                //Destroy(gameObject);
            }

            else if (canHitObstacle && go.layer == GM.layerObstacle)
            {
                canHitObstacle = false;
                go.GetComponent<IObstacle>().TakeDmg();
                //Destroy(gameObject);
            }
            ////If collided with a bullet
            //else if (go.layer == SettingsAndPrefabRefs.layerBullet)
            //{
            //    //If not the same layer as me, then destroy
            //    BulletBase bullet = go.GetComponent<BulletBase>();
            //    if (bullet.index != index)
            //    {
            //        .InstantDestroy();
            //    }
            //}
            else if (go.layer == GM.layerProp)
            {
                go.GetComponent<IProps>().PropInteraction(index);
            }
        }
    }
    bool canHitObstacle = true;
    List<int> hitEnemies = new List<int>();

    IEnumerator DelayedDestroy()
    {
        //Auto destroy self
        //Debug.Break();
        yield return new WaitForSeconds(0.2f);
        Paint();
        
        Destroy(gameObject);
    }
}