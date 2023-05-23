using UnityEngine;
using System.Collections;

public class Enemy1_Plane : EnemyBase
{
    float rotAmount = 0f;

    public override void Initialization()
    {
        BaseInitialization();

        moveSpeed = 1f;
        rotSpeed = 50f;

        HP = MaxHP = 2;
    }

    protected override void ActivationAdditionalEffect()
    {
        StartCoroutine(RandomBehaviorUpdate());
    }

    public void Update()
    {
        pos = transform.position;
        DrawingUpdate();
    }

    public void FixedUpdate()
    {
        //Rotation: constant random
        if (onScreenNow)
        {
            OutOfBoundReflect();
            rb.angularVelocity = rotAmount * rotSpeed;
        }

        //Move forward
        rb.velocity = moveSpeed * trans.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    IEnumerator RandomBehaviorUpdate()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 3f));
        //Random rotation
        rotAmount = (Random.value < 0.5f) ? -0.2f : 0.2f;
    }

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
}