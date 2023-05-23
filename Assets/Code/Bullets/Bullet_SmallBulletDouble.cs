using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_SmallBulletDouble : BulletBase
{
    public GameObject pf_TinyBullet;
    public Transform[] Shootpositions;

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        GameObject bullet;
        for (int i = 0; i < Shootpositions.Length; i++)
        {
            bullet = GameObject.Instantiate(pf_TinyBullet, Shootpositions[i].position, Shootpositions[i].rotation) as GameObject;
            bullet.GetComponent<BulletBase>().Shoot(index, null);
        }
        DestroyBullet();
    }
}