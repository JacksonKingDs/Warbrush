using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeTail : EnemyBase
{
    #region Fields
    public Transform leader;

    float keepDistance = 0.15f;

    //Current state
    Vector2 dirToLeader;

    Vector3 scale_Up = new Vector3(1, 1, 1);
    Vector3 scale_Down = new Vector3(-1, 1, 1);
    #endregion

    #region MonoBehaviour
    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 1f;
        rotSpeed = 50f;

        HP = MaxHP = 3;
    }

    public override void Activation(Vector3 pos, Quaternion rot, Transform leader, bool leftUp) //This one is for Centipede
    {
        HP = MaxHP;
        onScreenNow = false;
        trans.position = pos;
        trans.rotation = rot;
        ActivationAdditionalEffect();
        StartCoroutine(Spawning());

        this.leader = leader;
    }

    void Update()
    {
        DrawingUpdate();
    }

    void FixedUpdate()
    {
        if (leader != null)
        {
            TailMovement();
        }
        else
        {
            ReturnToPool();
        }
    }
    #endregion

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    void ReturnToPool ()
    {
        enemyM.ReturnToPool(gameObject);
    }

    #region Methods
    void TailMovement()
    {
        //Facing leader
        dirToLeader = leader.position - trans.position;
        if (dirToLeader.sqrMagnitude > keepDistance)
        {
            //Rotate lerp towards enemy
            trans.rotation = Quaternion.LookRotation(Vector3.forward, dirToLeader);

            //Always moving towards enemy
            rb.velocity = dirToLeader.normalized * moveSpeed;
        }
        else
        {
            rb.velocity = trans.up * moveSpeed;
        }
    }
    #endregion
}

/* Original smooth lerp rotation
 * 
     void FixedUpdate()
    {
        dirToLeader = leader.position - trans.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, dirToLeader), 0.1f);
        //Always moving towards enemy
        transform.position = leader.position + (trans.position - leader.position).normalized * keepDistance;        
    }
     */
