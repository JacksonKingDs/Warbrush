using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_EnemyCombatRedX : BulletBase
{
    public Transform spriteTrans;

    public float movespeed = 1f; //5

    Vector3 vel;

    #region Init
    public override void Shoot(int index, Vector3 arrivalLocation)
    {
        OnAwake();
        this.index = index;
        this.arrivalLocation = arrivalLocation;

        rb.velocity = movespeed * (arrivalLocation - trans.position).normalized;
        rends.color = Color.red;
    }
    #endregion

    void Update()
    {
        //Destroy upon arriving at location
        if (Vector2.Distance(trans.position, arrivalLocation) < 0.2f)
        {
            Instantiate(SettingsAndPrefabRefs.instance.Pfx_HitAura, trans.position, Quaternion.identity); //Pfx
            Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        spriteTrans.Rotate(new Vector3(0f, 0f, 10f));
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
            else if (go.layer == GM.layerBullet)
            {
                if (go.GetComponent<BulletBase>().index != index)
                    Destroy(gameObject);
            }
        }
    }
}