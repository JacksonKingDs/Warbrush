using UnityEngine;
using System.Collections;

public class Enemy_Bat : EnemyBase
{
    //Rotation
    Vector3 curDir;

    public override void Initialization()
    {
        BaseInitialization();
        enemyColor = BG_Painter.enemy_body;
        spriteRend.color = enemyColor;

        moveSpeed = 1.2f;
        rotSpeed = 0.7f;
        HP = MaxHP = 3;
    }

    protected override void ActivationAdditionalEffect()
    {
        StartCoroutine(RotationChange());
        rb.velocity = moveSpeed * trans.up;
        curDir = trans.up;
    }

    public void Update()
    {
        pos = trans.position;
        DrawingUpdate();
    }

    void OutOfBoundsCheck()
    {
        if (pos.x <= BG_Bound_minX)
        {
            curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.right;
        }
        else if (pos.x >= BG_Bound_maxX)
        {
            curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.left;
        }
        else if (pos.y <= BG_Bound_minY)
        {
            curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.up;
        }
        else if (pos.y >= BG_Bound_maxY)
        {
            curDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector3.down;
        }
    }

    IEnumerator RotationChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            //Debug.Log("pre" + rb.velocity);
            curDir = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * trans.up;
            OutOfBoundsCheck();
            SetNewVelocity(curDir * moveSpeed);
            //Debug.Log(rb.velocity);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    { DefaultTriggerEnter(col); }

    void SetNewVelocity(Vector3 vel)
    {
        rb.velocity = vel;
        trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }


    public override void TakeDamage(int index, int dmg = 1)
    {
        if (!invulnerable)
        {
            if (index >= 0 && index < 4)
            {
                FightSceneManager.landed[index]++;
            }

            HP -= dmg;
            if (HP <= 0)
            {
                //Dead
                if (index >= 0 && index < 4)
                    FightSceneManager.AddKillScore(index);
                trans.position = offscreen;
                enemyM.ReturnToPool(gameObject);
            }
            else
            {
                Vector3 p = trans.position.normalized;
                p.z = 0f;
                SetNewVelocity(p * moveSpeed);
                StartCoroutine(GetHitBlink());
            }
        }
    }
}