using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Ghost : EnemyBase
{
    Vector3 targetDir;
    Transform enemyTrans;
    Collider2D col;

    float fadeSpeed = 8f;
    float maxScale = 1.28f;

    public override void Initialization()
    {
        BaseInitialization();

        enemyColor = BG_Painter.enemy_body;
        spriteRend.color = enemyColor;
        col = GetComponent<Collider2D>();

        //Initialize
        moveSpeed = 0.2f;
        HP = MaxHP = 10;
        rotSpeed = 50f;
    }

    protected override void ActivationAdditionalEffect()
    {
        //trans.rotation = Quaternion.identity;
        rb.velocity = moveSpeed * trans.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DefaultTriggerEnter(col);
    }

    private void FixedUpdate()
    {
        pos = trans.position;

        if (sceneM.validPlayers.Count <= 0)
        {
            CheckOutOfBounds();
        }
        else
        {
            //Find cloest enemy
            float shortestDist = float.MaxValue;
            foreach (int i in sceneM.validPlayers)
            {
                Vector2 dir = sceneM.tanksTrans[i].position - pos;
                float d = dir.magnitude;
                if (d < shortestDist)
                {
                    targetDir = dir;
                    shortestDist = d;
                    enemyTrans = sceneM.tanksTrans[i];
                }
            }

            targetDir = Vector3.RotateTowards(trans.up, targetDir, rotSpeed, 0.0f);
            trans.rotation = Quaternion.LookRotation(Vector3.forward, targetDir);
            rb.velocity = moveSpeed * trans.up;
        }
    }
    
    void CheckOutOfBounds()
    {
        rb.velocity = moveSpeed * trans.up;
        Vector3 vel = rb.velocity;
        if (pos.x > BG_Bound_maxX && vel.x > 0) //Hits right
        {
            vel.x = -vel.x;
            SetNewVelocity(vel);
        }
        else if (pos.x < BG_Bound_minX && vel.x < 0) //Hits left
        {
            vel.x = -vel.x;
            SetNewVelocity(vel);
        }
        //Vel y positive = moving up. BG_Bound_maxY is positive value
        else if (pos.y > BG_Bound_maxY && vel.y > 0) //Hits top
        {
            vel.y = -vel.y;
            SetNewVelocity(vel);
        }
        else if (pos.y < BG_Bound_minY && vel.y < 0) //Hits bot
        {
            vel.y = -vel.y;
            SetNewVelocity(vel);
        }
    }

    void SetNewVelocity(Vector3 vel)
    {
        rb.velocity = vel;
        //trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
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
                //GameObject g = Instantiate(refs.Pfx_EnemyExplode, trans.position, trans.rotation) as GameObject;
                //g.transform.localScale = transform.localScale;

                //Dead
                if (index >= 0 && index < 4)
                    FightSceneManager.AddKillScore(index);
                trans.position = offscreen;
                enemyM.ReturnToPool(gameObject);
            }
            else
            {
                StartCoroutine(GetHitBlink());
            }
        }
    }

    
    protected override IEnumerator GetHitBlink()
    {
        invulnerable = true;

        //Do black white blinks
        spriteRend.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.red;
        yield return null;
        spriteRend.color = Color.white;
        yield return new WaitForSeconds(0.05f);

        //Do transparent blinks
        bool isWhite = false;
        for (int i = 0; i < 2; i++)
        {
            if (isWhite)
            {
                spriteRend.color = Color.black;
            }
            else
            {
                spriteRend.color = Color.white;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }

        //Fade out
        Color c = enemyColor;
        c = Color.white * 0.5f;
        spriteRend.color = c;
        col.enabled = false;

        //Teleport
        float x = Random.Range(5f, 6f);
        float y = Random.Range(3f, 4f);
        if (Random.value > 0.5f)
            x = -x;
        if (Random.value > 0.5f)
            y = -y;
        Vector3 tgtPos = new Vector3(x, y, trans.position.z);

        //float t = 0f;
        //while (t < 1f)
        //{
            
        //    t = t + Time.deltaTime / 3f;
        //    trans.position = Vector3.Lerp(trans.position, tgtPos, t);
        //    yield return null;
        //}

        float dist = Vector3.Distance(trans.position, tgtPos);
        for (float i = 0f; i < 1f; i += (10 * Time.deltaTime) / dist)
        {
            transform.position = Vector3.Lerp(trans.position, tgtPos, i);
            yield return null;
        }

        trans.position = tgtPos;

        //Fade back
        spriteRend.color = enemyColor;
        col.enabled = true;

        invulnerable = false;
        //spriteRend.color = enemyColor;
    }
}

/*
 //Fade out
        Color c = enemyColor;
        while (c.a > 0.05f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            spriteRend.color = c;
            yield return null;
        }
        c.a = 0f;
        spriteRend.color = c;

        //Teleport
        float x = Random.Range(5f, 6f);
        float y = Random.Range(3f, 4f);
        if (Random.value > 0.5f)
            x = -x;
        if (Random.value > 0.5f)
            y = -y;
        trans.position = new Vector3(x, y, trans.position.z);

        //Fade back in
        while (c.a < 0.95f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            spriteRend.color = c;
            yield return null;
        }
        c.a = 1f;
        spriteRend.color = c;

        invulnerable = false;
        //spriteRend.color = enemyColor;
     */
