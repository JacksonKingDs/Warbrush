using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class UIFadeUtil
{
    #region Fading of Canvas
    public static IEnumerator Canvas_FadeToOpaque (CanvasGroup cvsGrp, float fadeSpd)
    {
        while (cvsGrp.alpha < 1)
        {
            cvsGrp.alpha += Time.deltaTime * fadeSpd;
            yield return null;
        }

        cvsGrp.alpha = 1f;

        cvsGrp.blocksRaycasts = true;
        cvsGrp.interactable = true;
    }

    public static IEnumerator Canvas_FadeToTransparent (CanvasGroup cvsGrp, float fadeSpd)
    {
        cvsGrp.blocksRaycasts = false;
        cvsGrp.interactable = false;

        while (cvsGrp.alpha > 0)
        {
            cvsGrp.alpha -= Time.deltaTime * fadeSpd;
            yield return null;
        }

        cvsGrp.alpha = 0f;
    }

    public static void Canvas_InstantTransparent (CanvasGroup cvsGrp)
    {
        cvsGrp.blocksRaycasts = false;
        cvsGrp.interactable = false;
        cvsGrp.alpha = 0f;
    }

    public static void Canvas_InstantOpaque(CanvasGroup cvsGrp)
    {
        cvsGrp.blocksRaycasts = true;
        cvsGrp.interactable = true;
        cvsGrp.alpha = 1f;
    }
    #endregion

    #region Fading of Image (a UI component)
    public static IEnumerator Image_Fade(bool fadeToOpaque, Image image, float fadeSpd)
    {
        Color c = image.color;
        if (fadeToOpaque)
        {
            while (c.a < 1f)
            {
                c.a += fadeSpd * Time.deltaTime;
                image.color = c;
                yield return null;
            }
            c.a = 1f;
            image.color = c;
        }
        //Fade to clear
        else
        {
            while (c.a > 0f)
            {
                c.a -= fadeSpd * Time.deltaTime;
                image.color = c;
                yield return null;
            }
            c.a = 0f;
            image.color = c;
        }
    }

    public static void Image_Instant(bool fadeToOpaque, Image image)
    {
        Color c = image.color;
        if (fadeToOpaque)
        {
            c.a = 1f;
            image.color = c;
        }
        //Fade to clear
        else
        {
            c.a = 0f;
            image.color = c;
        }
    }
    #endregion
}
