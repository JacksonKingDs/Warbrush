using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Shotgun : BulletBase
{
    public GameObject subBullet;
    public Transform[] Shootpositions;


    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        OnAwake();
        GameObject bullet;
        for (int i = 0; i < 4; i++)
        {
            bullet = GameObject.Instantiate(subBullet, Shootpositions[i].position, Shootpositions[i].rotation) as GameObject;
            bullet.GetComponent<BulletBase>().Shoot(index, null);
        }
        Destroy(gameObject);
    }    
}