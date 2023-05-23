using UnityEngine;
using System.Collections;

public class Enemy3_Moon : EnemyBase
{
    public GameObject bullet;

    //When Moon is beyond the x/y limit, it will check for the wider side and the turn in that direction
    float xlimit;
    float ylimit;

    //Rotation
    Vector3 targetDir;
    Vector3 curDir;
    bool clockwise;

    public override void Initialization()
    {
        BaseInitialization();

        RandomizeLimits();
        moveSpeed = 1f;
        rotSpeed = 0.02f;
        HP = MaxHP = 4;
        clockwise = Random.value > 0.5f ? true : false;

        //curDir = targetDir = trans.up;
    }

    protected override void ActivationAdditionalEffect()
    {
        curDir = targetDir = trans.up;
        StartCoroutine(IntervalUpdate());
        rb.velocity = moveSpeed * trans.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    private void Update()
    {
        pos = trans.position;
        DrawingUpdate();
    }

    public void FixedUpdate()
    {
        //Move forward
        

        //Rotation
        if (onScreenNow)
        {
            //Target direct
            curDir = Vector3.RotateTowards(curDir, targetDir, rotSpeed, 0.0f);

            //Rotate
            trans.rotation = Quaternion.LookRotation(Vector3.forward, curDir);
            rb.velocity = moveSpeed * trans.up;
        }
    }

    Vector3 vel;
    IEnumerator IntervalUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            vel = rb.velocity;

            if (vel.x < -0.05f && pos.x <= -xlimit) //Hits left
            {
                targetDir = clockwise ? Vector3.up : Vector3.down;
            }
            else if (vel.x > 0.05f && pos.x >= xlimit) //Hits Right
            {
                targetDir = clockwise ? Vector3.down : Vector3.up;
            }
            else if (vel.y < -0.05f && pos.y <= -ylimit) //Hits Bot
            {
                targetDir = clockwise ? Vector3.left : Vector3.right;
            }
            else if (vel.y > 0.05f && pos.y >= ylimit) //Hits top
            {
                targetDir = clockwise ? Vector3.right : Vector3.left;
            }
            RandomizeLimits();
        }
    }

    void RandomizeLimits ()
    {
        ylimit = Random.Range(0.5f, 3.5f);
        xlimit = ylimit * 1.5f;
    }
}