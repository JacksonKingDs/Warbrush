using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScLogo_LogoFadingManager : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public string nextSceneName = "Scene_Menu";
    public float fadeSpeed = 1f;

    [Header("FADING")]
    public float initialWait = 0.5f;
    public float stayVisibleDuration = 1.5f;
    

    void Awake ()
    {
        logoCanvasGroup.alpha = 0f;
    }


    IEnumerator Start()
    {
        //Fade in logo
        yield return new WaitForSeconds(initialWait);
        yield return StartCoroutine(UIFadeUtil.Canvas_FadeToOpaque(logoCanvasGroup, fadeSpeed));

        //Fade out logo
        yield return new WaitForSeconds(stayVisibleDuration);
        yield return StartCoroutine(UIFadeUtil.Canvas_FadeToTransparent(logoCanvasGroup, fadeSpeed));

        //Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}