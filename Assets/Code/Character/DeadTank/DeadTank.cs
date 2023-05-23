using UnityEngine;
using System.Collections;

public class DeadTank : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform[] skidPoints;
    public SpriteRenderer rend;

    public Color combatColor;
    public Color nightColor;
    public Color oceanColor;
    public Color desertColor;
    public Color arcadeColor;
    public Color spookyColor;
    public Color spaceColor;
    public Color beltColor;


    const float topBound = TankUtil.topBound;
    const float rightBound = TankUtil.rightBound;
    const float warpHeight = TankUtil.warpHeight;
    const float warpWidth = TankUtil.warpWidth;

    //Cache
    Transform trans;
    Vector3 pos;

    float knockbackDuration = 1f;
    float knockbackForce = 3f;
    bool paintingSkid = true;
    int skidIntervalCounter;

    BGTextureManager BG_Painter;

    void Awake()
    {
        trans = transform;
        if (GM.gameMode == GameMode.PVP_Combat)
        {
            rend.color = combatColor;
        }
        else if (GM.gameMode == GameMode.PVP_Night)
        {
            rend.color = nightColor;
        }
        else if (GM.gameMode == GameMode.PVP_OceanMist)
        {
            rend.color = oceanColor;
        }
        else if (GM.gameMode == GameMode.PVP_Desert)
        {
            rend.color = desertColor;
        }
        else if (GM.gameMode == GameMode.Coop_Arcade)
        {
            rend.color = arcadeColor;
        }
        else if (GM.gameMode == GameMode.Coop_Torch)
        {
            rend.color = spookyColor;
        }
        else if (GM.gameMode == GameMode.Hanabi)
        {
            rend.color = spaceColor;
        }
        else if (GM.gameMode == GameMode.Campaign)
        {
            rend.color = combatColor;
            //if (GM.campaignMapIndex < 7) //PVP_Combat
            //{
            //    rend.color = combatColor;
            //}
            //else if (GM.campaignMapIndex < 14) //Night
            //{
            //    rend.color = nightColor;
            //}
            //else if (GM.campaignMapIndex < 19) //PVP_OceanMist
            //{
            //    rend.color = oceanColor;
            //}
            //else if (GM.campaignMapIndex < 24) //PVP_Desert
            //{
            //    rend.color = desertColor;
            //}
            //else if (GM.campaignMapIndex < 30)  //Torch
            //{
            //    rend.color = spookyColor;
            //}
            //else if (GM.campaignMapIndex < 35) //Hanabi
            //{
            //    rend.color = spaceColor;
            //}
            //else //Coop_Arcade
            //{
            //    rend.color = arcadeColor;
            //}
        }
    }

    SettingsAndPrefabRefs refs;
    IEnumerator Start()
    {
        BG_Painter = BGTextureManager.instance;

        refs = SettingsAndPrefabRefs.instance;

        for (int i = 0; i < 3; i++)
        {
            refs.SpawnCrescentExplode(trans.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator RestoreDrag()
    {
        yield return new WaitForSeconds(0.6f);
        if (GM.gameMode == GameMode.Hanabi)
        {
            rb.drag = 0f;
            rb.angularDrag = 0f;
            if (rb.angularVelocity > 10f)
                rb.angularVelocity = 10f;
            rb.velocity = rb.velocity * 0.5f;
        }
        else
        {
            rb.angularDrag = 20;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = 0f;
        
        //yield return new WaitForSeconds(2f);
        //paintingSkid = false;
    }

    public void KnockBack(Vector2 knockbackDir)
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = knockbackDir * knockbackForce;
        rb.angularVelocity = 1500;

        StartCoroutine(RestoreDrag());
    }

    void FixedUpdate()
    {
        //skidIntervalCounter--;

        //if (skidIntervalCounter <= 0)
        //{
        //    foreach (var p in skidPoints)
        //    {
        //        BG_Painter.PaintDirt(p.position);
        //    }
        //}
        //else
        //{
        //    skidIntervalCounter = 3;
        //}


        //Warping
        pos = trans.position;
        if (pos.x < -rightBound) //Warp left to right
            trans.position = new Vector3(pos.x + warpWidth, pos.y, pos.z);
        else if (pos.x > rightBound) //Warp left to right
            trans.position = new Vector3(pos.x - warpWidth, pos.y, pos.z);
        else if (pos.y < -topBound) //Warp left to right
            trans.position = new Vector3(pos.x, pos.y + warpHeight, pos.z);
        else if (pos.y > topBound) //Warp left to right
            trans.position = new Vector3(pos.x, pos.y - warpHeight, pos.z);
    }

    //IEnumerator KnockBack()
    //{
    //    inKnockback = true;
    //    yield return new WaitForSeconds(knockbackDuration);
    //    inKnockback = false;
    //}
}
