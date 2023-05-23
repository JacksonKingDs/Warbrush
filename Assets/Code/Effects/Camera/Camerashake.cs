using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tank mid is the position 
public class Camerashake : MonoBehaviour 
{
    public static Camerashake instance;

    //Camera shake
    float shakeCounter;
    float magnitude;

    //Cache reference
    FightSceneManager sceneM;
    Transform trans;
    Vector3 originalPos;

    float hitPauseTimer = 0f;
    bool bigPause = false;

    void Awake()
    {
        instance = this;
        trans = transform;
        originalPos = trans.position;
    }

    void Start ()
    {
        //sceneM = FightSceneManager.instance;
    }

    void Update()
    {
        if (shakeCounter > 0)
        {
            trans.localPosition = trans.localPosition + Random.insideUnitSphere * magnitude;
            shakeCounter -= Time.deltaTime;
        }
        else
        {
            trans.localPosition = originalPos;
        }

        if (hitPauseTimer > 0f)
        {
            hitPauseTimer -= Time.unscaledDeltaTime;

            if (hitPauseTimer <= 0f)
            {
                Time.timeScale = 1f;
            }
            else if (bigPause)
            {
                Time.timeScale = Mathf.Lerp(0f, 1f, 0.5f - hitPauseTimer);
            }
        }
    }

    bool inHitPause = false;
    public void HitPause(bool bigPause)
    {
        this.bigPause = bigPause;
        Time.timeScale = 0f;
        if (bigPause)
        {
            hitPauseTimer = 0.5f;
        }
        else
        {
            hitPauseTimer = 0.1f;
        }
    }

    IEnumerator DoPause(bool bigPause)
    {
        inHitPause = true;

        if (bigPause)
        {
            //Debug.Log(index + " bigPause" + Time.time);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(0.05f);
            Time.timeScale = 1;

            //for (float i = 0; i < 0.29; i += 0.01f)
            //{
            //    Time.timeScale = Mathf.Lerp(0f, 1f, i);
            //    yield return null;
            //}
            //Time.timeScale = 1;
            //Debug.Log("Time.timeScale " + Time.timeScale);
        }
        else
        {
            //Debug.Log(index + "smallPause" + Time.time);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(0.02f);
            Time.timeScale = 1;
        }

        inHitPause = false;
    }


    public void DoSmallShake ()
    {
        shakeCounter = 0.2f;
        magnitude = 0.02f;
    }

    public void DoBigShake ()
    {
        shakeCounter = 0.3f;
        magnitude = 0.03f;
    }

    Vector3 _total;
    Vector3 tankCenter;
    void UpdateTankCenterPoint()
    {
        _total = Vector3.zero;

        foreach (var i in sceneM.validPlayers)
        {
            _total = _total + sceneM.tanksTrans[i].position;
        }

        tankCenter = _total / sceneM.validPlayers.Count;
    }
}
