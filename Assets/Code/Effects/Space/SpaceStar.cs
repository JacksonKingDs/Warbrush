using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStar : MonoBehaviour 
{
    //public SpaceStarsManager starsManager;
    public SpriteRenderer rend;
    BGTextureManager BG_Painter;

    float shiningDuration;
    Color clearColor;
    Color startingColor;
    Color targetColor; //Lerping

    private void Awake()
    {
        startingColor = rend.color;
        clearColor = startingColor;
        clearColor.a = 0;
        rend.color = clearColor;
    }

    private void Start()
    {
        BG_Painter = BGTextureManager.instance;
    }

    private void Update()
    {
        if(shiningDuration > 0)
        {
            //Tick timer
            shiningDuration -= Time.deltaTime;
            if (shiningDuration < 0f)
            {
                shiningDuration = 0f;
                targetColor = clearColor;                
            }
        }

        rend.color = Color.Lerp(rend.color, targetColor, Time.deltaTime);
    }

    #region Public
    public void LightUp()
    {
        shiningDuration = 10f;
        targetColor = startingColor;
    }
    #endregion


    #region OnTrigger
    void OnTriggerStay2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerPlayer)
            {
                LightUp();
                
                TankControllerBase player = go.GetComponent<TankControllerBase>();
                //player.GetComponent<TankControllerBase>().DisableSlide();
                //targetColor = GM.pallet.Trans[player.index];

                //See if it can make a constellation connection
                if (player.recentStar != this)
                {
                    if (player.recentStar != null)
                    {
                        BG_Painter.DrawConstelationLine(player.recentStar.transform.position,
                            transform.position);
                        player.recentStar.LightUp();
                    }

                    player.recentStar = this;
                }
            }
        }
    }

    //void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col != null)
    //    {
    //        GameObject go = col.gameObject;

    //        if (go.layer == GM.layerPlayer)
    //        {
    //            go.GetComponent<TankControllerBase>().EnableSlide();
    //        }
    //    }
    //}
    #endregion
}
