using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootChargingEffect : MonoBehaviour 
{
    #region Fields
    public ParticleSystem chargingPfx1; //Pixels
    public ParticleSystem chargingPfx2; //Ring
    public SpriteRenderer[] pixelDot;
    public Animator muzzleAnimator;
    public ParticleSystem [] barrelExplosion;

    [HideInInspector] public Vector3 frontPosition = new Vector3(0f, 0.45f, 0f);
    [HideInInspector] public Vector3 backPosition = new Vector3(0f, -0.35f, 0f);

    [HideInInspector] public ParticleSystem.MainModule pfx1Main; //For changing pfx size, color
    [HideInInspector] public ParticleSystem.MainModule pfx2Main; //For changing pfx size, color

    //Vector3 Muzzle_FullScale = new Vector3(0f, 0f, 0f);
    //Vector3 Muzzle_HalfScale = new Vector3(0.5f, 0.5f, 0.5f);
    [HideInInspector] public int animState_muzzleFlashBig;
    [HideInInspector] public int animState_muzzleFlashMid;
    [HideInInspector] public int animState_muzzleFlashSmall;

    Transform[] pixelTrans;

    bool maxedCharge = false;

    Vector3 pixelStartScale;
    Vector3 pixelScaleHalf;

    Color transparentPixelColor = new Color(1f, 1f, 1f, 0.5f);
    #endregion

    //MONO
    void Awake()
    {
        pfx1Main = chargingPfx1.main;
        pfx2Main = chargingPfx2.main;
        animState_muzzleFlashBig = Animator.StringToHash("FlashBig");
        animState_muzzleFlashMid = Animator.StringToHash("FlashMid");
        animState_muzzleFlashSmall = Animator.StringToHash("FlashSmall");

        pixelTrans = new Transform[pixelDot.Length];
        for (int i = 0; i < pixelDot.Length; i++)
        {
            pixelTrans[i] = pixelDot[i].transform;
        }

        pixelStartScale = pixelTrans[0].localScale;
        pixelScaleHalf = pixelStartScale * 0.5f;

        StopAll();
    }

    //PUBLICS
    public void StopAll ()
    {
        foreach (var m in pixelDot)
        {
            m.enabled = false;
        }

        chargingPfx1.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        chargingPfx2.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void stg1_BeginCharging()
    {
        chargingPfx1.Play();
        maxedCharge = false;
    }

    public void SetPixelPfx(float percentage)
    {
        //Change size of pixel dot
        //float f = percentage * 0.5f + 0.5f;

        //for (int i = 0; i < pixelDot.Length; i++)
        //{
        //    pixelDot[i].enabled = true;
        //    pixelDot[i].color = new Color(f, f, f);
        //    //pixelTrans[i].localScale = pixelScaleHalf;
        //}

        //foreach (var m in pixelTrans)
        //{
        //    m.localScale = new Vector3(f, f, f);
        //}
    }

    public void stg2_ReachedHalfCharge()
    {
        foreach (var m in pixelDot)
        {
            m.enabled = true;
            m.transform.localScale = pixelScaleHalf;
        }

        chargingPfx2.Play();
        maxedCharge = true;
    }

    public void stg3_ReachedMaxCharge ()
    {
        foreach (var m in pixelDot)
        {
            m.transform.localScale = pixelStartScale;
        }

        chargingPfx2.Play();
        maxedCharge = true;
    }

    public void stg4_ShootBullet(bool isFullCharge)
    {
        //Muzzle flash & barrle explosion
        if (isFullCharge)
        {
            muzzleAnimator.Play(animState_muzzleFlashBig, 0, 0f);
            foreach (var b in barrelExplosion)
            {
                b.Play();
            }
        }
        else
        {
            muzzleAnimator.Play(animState_muzzleFlashSmall, 0, 0f);
        }

        //Hide pixel dots and charging pfx
        chargingPfx1.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        chargingPfx2.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        foreach (var m in pixelDot)
        {
            m.enabled = false;
        }
    }
}

//I don't like the blink, makes me nauceous
//if (maxedCharge)
//{
//    float f = 0.7f +  0.4f * Mathf.Abs(Mathf.Sin(Time.time * 3f));
//    foreach (var m in pixelDot)
//    {
//        m.color = new Color(f, f, f);
//    }
//}