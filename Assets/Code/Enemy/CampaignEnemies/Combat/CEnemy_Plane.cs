using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemy_Plane : CampaignEnemyBase
{
    public GameObject pf_bullet;
    Transform targetPlayer;

    void Awake()
    {
        base.OnAwake();
    }

    //The plane is always rotating towards a random player
    //When it is close enough, attack, and change target to a new player


    IEnumerator Start()
    {
        base.OnStart();

        moveSpeed = 1.5f;
        MaxHP = 3 ;
        rotSpeed = 0.008f;
        HP = MaxHP;

        Update();
        curDir = trans.up;

        yield return new WaitForSeconds(3f);
        gameStarted = true;
        //StartCoroutine(ShortIntervalUpdate());
        //StartCoroutine(LongIntervalUpdate());
    }


    void Update()
    {
        if (!gameStarted)
            return;

        List<IntXY> _toPaint = new List<IntXY>(); //Pixels to paint in this frame of update
        IntXY pixel = new IntXY();
        foreach (Transform t in paintPoints)
        {
            pixel = BGTextureManager.WorldPosToPixelPos_BG(t.position);
            if (!painted.Contains(pixel))
            {
                painted.Add(pixel);
                _toPaint.Add(pixel);
            }
        }

        BG_Painter.PaintBulletPoints(_toPaint, GM.enemyIndex);
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        //Move and rotate
        if (targetPlayer == null)
        {
            rb.velocity = moveSpeed * trans.up;
            FindRandomPlayer();
        }
        else
        {
            pos = trans.position;

            tgtDir = targetPlayer.position - pos;
            curDir = Vector3.RotateTowards(trans.up, tgtDir, rotSpeed, 0.0f).normalized;
            rb.velocity = trans.up * moveSpeed;
            trans.rotation = Quaternion.LookRotation(Vector3.forward, curDir);
        }
    }

    IEnumerator LongIntervalUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (targetPlayer != null && inRange)
                Instantiate(pf_bullet, trans.position, Quaternion.identity).GetComponent<BulletBase>().Shoot(GM.enemyIndex, targetPlayer.position);
        }
    }

    bool inRange;
    IEnumerator ShortIntervalUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (targetPlayer != null)
            {
                if (tgtDir.sqrMagnitude < 25f)
                {
                    inRange = true;
                }
                else
                {
                    inRange = false;
                }
            }
        }
    }

    void FindRandomPlayer()
    {
        targetPlayer = null;

        if (sceneM.validPlayers.Count > 0)
        {
            int tankIndex = sceneM.validPlayers[Random.Range(0, sceneM.validPlayers.Count)]; //Get a random Valid index.
            targetPlayer = sceneM.tanksTrans[tankIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }
}