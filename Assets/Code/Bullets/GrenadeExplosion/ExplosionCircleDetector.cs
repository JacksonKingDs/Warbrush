using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCircleDetector : BulletBase
{
    #region Fields
    public ExplosionCircle circle;
    #endregion

    public override void Shoot(int index, BehaviorNormalAttack behavior)
    {
        this.index = index;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        circle.IndirectTriggerEnter2D(col);
    }
}