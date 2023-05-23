using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Torch : MonoBehaviour, IProps
{
    public SpriteRenderer spriteRend;

    //Ref
    BGTextureManager BG_Painter;
    Transform trans;
    int index = -20;

    //State
    //bool torchOn = false;
    //float torchDuration = 0f;
    Color tgtColor = Color.grey;
    bool invulnerable = false;
    //int ticksToRefresh = 0;
    bool initialized = false;

    void Start()
    {
        trans = transform;
        BG_Painter = BGTextureManager.instance;
        initialized = true;
    }
    
    void Update()
    {
        //Tick down torch
        //if (torchOn)
        //{
        //    torchDuration -= Time.deltaTime;
        //    ticksToRefresh = ticksToRefresh - 1;
        //    if (ticksToRefresh < 0)
        //    {
        //        ticksToRefresh = 10;
        //        BG_Painter.Bullet_ClearSpookyFogTorch(trans.position, index);
        //    }
            
        //    if (torchDuration < 0f)
        //    {
        //        torchOn = false;
        //        spriteRend.color = Color.grey;
        //    }
        //}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null && initialized && !invulnerable)
        {
            GameObject go = col.gameObject;
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase targetPlayer = go.GetComponent<TankControllerBase>();
                PropInteraction(targetPlayer.index);
            }
        }
    }

    public void PropInteraction(int otherIndex)
    {
        if (otherIndex >= 0 && otherIndex < 4)
        {
            //FightSceneManager.landed[otherIndex]++;
            if (index != otherIndex)
            {
                index = otherIndex;
                tgtColor = GM.pallet.Tank[otherIndex];
                BG_Painter.Bullet_ClearSpookyFogTorch(trans.position, index);
                StartCoroutine(GetHitBlink());
            }
        }
        else if (otherIndex == GM.enemyIndex)
        {
            index = otherIndex;
            tgtColor = Color.grey;
            BG_Painter.Spooky_TurnOffTorch(trans.position);
            StartCoroutine(GetHitBlink());
        }
    }

    IEnumerator GetHitBlink()
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

        invulnerable = false;
        spriteRend.color = tgtColor;
    }

}