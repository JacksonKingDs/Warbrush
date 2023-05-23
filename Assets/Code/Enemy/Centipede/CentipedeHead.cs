using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeHead : EnemyBase
{
    #region MonoBehaviour
    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 2f;
        rotSpeed = 50f;

        HP = MaxHP = 4;
    }

    protected override void ActivationAdditionalEffect()
    {
        StartCoroutine(RandomizeRotation());
    }

    IEnumerator RandomizeRotation ()
    {
        rb.velocity = trans.up * moveSpeed;
        yield return new WaitForSeconds(1f); //Delay
        while (true)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 10 * Random.Range(-12, 13));
            rb.velocity = trans.up * moveSpeed;
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }

    void Update()
    {
        DrawingUpdate();
    }

    void FixedUpdate()
    {
        if (onScreenNow)
        {
            OutOfBoundReflect();
        }

        
    }
    #endregion

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }


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

//if (Random.value < 0.1f) //Suddenly change direction
//{
//    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
//}
//else
//{
//    transform.rotation *= Quaternion.Euler(0, 0, 10 * Random.Range(-3, 4));
//steering = Random.Range(-100f, 100f);
//}

//rb.angularVelocity = steering;