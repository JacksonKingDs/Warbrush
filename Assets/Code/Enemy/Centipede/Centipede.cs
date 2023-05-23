using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : EnemyBase 
{
    #region Fields
    public Transform leader;
    public Transform left;
    public Transform right;
    int legStage = 0;

    //Settings
    bool isHead = false;

    //Current state
    Vector2 dirToLeader;
    GameObject leaderGO;

    //Const
    const float keepDistance = 0.19f;
    Vector3 Left_Up = new Vector3(1, 1, 1);
    Vector3 Left_Down = new Vector3(1, -1, 1);
    Vector3 Right_Up = new Vector3(-1, 1, 1);
    Vector3 Right_Down = new Vector3(-1, -1, 1);
    Quaternion Left_RotDown = Quaternion.Euler(0, 0, 10);
    Quaternion Right_RotDown = Quaternion.Euler(0, 0, -10);
    #endregion

    #region MonoBehaviour
    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 2f;
        rotSpeed = 50f;

        HP = MaxHP = 3;
    }

    public override void Activation(Vector3 pos, Quaternion rot, Transform leader, bool leftUp) //This one is for Centipede
    {
        HP = MaxHP;
        onScreenNow = false;
        
        trans.rotation = rot;
        trans.position = pos;
        ActivationAdditionalEffect();
        legStage = leftUp ? 1 : 2;

        LegInitialize();
        StartCoroutine(MoveLeg());
        StartCoroutine(Spawning());        

        this.leader = leader;
        if (leader == null)
        {
            Debug.Log("ERROR, centipede no leader!");
        }
        leaderGO = leader.gameObject;
        isHead = false;
    }

    void LegInitialize ()
    {
        switch (legStage)
        {
            case 0:
                left.localRotation = Quaternion.identity;
                right.localRotation = Right_RotDown;
                left.localScale = Left_Up;
                right.localScale = Right_Down;
                break;
            case 1:
                left.localScale = Left_Down;
                right.localScale = Right_Up;
                left.localRotation = Quaternion.identity;
                right.localRotation = Right_RotDown;
                break;
            case 2:
                left.localRotation = Left_RotDown;
                right.localRotation = Quaternion.identity;
                left.localScale = Left_Down;
                right.localScale = Right_Up;
                break;
            default:
                left.localScale = Left_Up;
                right.localScale = Right_Down;
                left.localRotation = Left_RotDown;
                right.localRotation = Quaternion.identity;
                break;
        }
    }

    IEnumerator MoveLeg ()
    {
        while (true)
        {
            switch (legStage)
            {
                case 0:
                    left.localRotation = Quaternion.identity;
                    right.localRotation = Right_RotDown;
                    break;
                case 1:
                    left.localScale = Left_Down;
                    right.localScale = Right_Up;
                    break;
                case 2:
                    left.localRotation = Left_RotDown;
                    right.localRotation = Quaternion.identity;
                    break;
                default:
                    left.localScale = Left_Up;
                    right.localScale = Right_Down;
                    legStage = -1;
                    break;
            }
            legStage++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        DrawingUpdate();
    }

    void FixedUpdate()
    {
        if (!isHead)
        {
            try
            {
                if (!leaderGO.activeSelf)
                {
                    isHead = true;
                    StartCoroutine(RandomizeRotation());
                }
                else
                {
                    TailMovement();
                }
            }
            catch
            {
                Debug.Log("centipede has no body");
                //Debug.Break();
                isHead = true;
                StartCoroutine(RandomizeRotation());
            }

        }
        else
        {
            OutOfBoundReflect();
        }
    }
    #endregion

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    #region Movement
    IEnumerator RandomizeRotation()
    {
        while (true)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 10 * Random.Range(-12, 13));
            rb.velocity = trans.up * moveSpeed;

            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }

    void TailMovement ()
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

    #region Out of bounds reflect
    protected void OutOfBoundReflect()
    {
        pos = transform.position;
        if (pos.x > BG_Bound_maxX) //Right
        {
            //UnityEngine.Debug.Log("right");
            Vector3 vel = rb.velocity;
            vel.x = -Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX) //Left
        {
            //UnityEngine.Debug.Log("Lt");
            Vector3 vel = rb.velocity;
            vel.x = Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y > BG_Bound_maxY) //Up
        {
            //UnityEngine.Debug.Log("Up");
            Vector3 vel = rb.velocity;
            vel.y = -Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY) //Down
        {
            //UnityEngine.Debug.Log("Dn");
            Vector3 vel = rb.velocity;
            vel.y = Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
    }

    void SetRigidbodyVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
    #endregion
}
/* DIrect look
 * void TailMovement ()
    {
        //Facing leader
        dirToLeader = leader.position - trans.position;
        if (dirToLeader.sqrMagnitude > catchUpDistance)
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
 
     */
