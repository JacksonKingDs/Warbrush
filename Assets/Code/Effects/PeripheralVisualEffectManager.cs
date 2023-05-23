using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeripheralVisualEffectManager : MonoBehaviour
{
    public static PeripheralVisualEffectManager instance;

    public List<GameObject> clouds;

    SettingsAndPrefabRefs refs;

    float BG_Bound_minX;
    float BG_Bound_minY;
    float BG_Bound_maxX;
    float BG_Bound_maxY;

    float rainMaxInterval;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        refs = SettingsAndPrefabRefs.instance;

        BG_Bound_minX = BGTextureManager.BG_Bound_minX;
        BG_Bound_minY = BGTextureManager.BG_Bound_minY;
        BG_Bound_maxX = BGTextureManager.BG_Bound_maxX;
        BG_Bound_maxY = BGTextureManager.BG_Bound_maxY;

        if (GM.gameMode == GameMode.PVP_OceanMist)
        {
            rainMaxInterval = Random.Range(0.06f, 0.1f);
            StartCoroutine(DoRain());
            //Debug.Log("Rain interval " + rainMaxInterval);
            intervalCounter = rainMaxInterval;

            foreach (var cloud in clouds)
            {
                cloud.SetActive(true);
            }
        }
        else
        {
            this.enabled = false;
        }
    }

    float intervalCounter;
    IEnumerator DoRain ()
    {
        while (true)
        {
            if (intervalCounter > 0)
            {
                intervalCounter -= Time.deltaTime;
            }
            else
            {
                intervalCounter = rainMaxInterval;
                SpawnRain();
            }
            yield return null;
        }
    }

    void SpawnRain()
    {
        refs.Pop_RainStroke(GetRandomPosition());
        refs.Pop_RainSplatter(GetRandomPosition());
    }

    Vector3 GetRandomPosition ()
    {
        return new Vector3(Random.Range(BG_Bound_minX, BG_Bound_maxX), Random.Range(BG_Bound_minY, BG_Bound_maxY), 0.2f);
    }
}