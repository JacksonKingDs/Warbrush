using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool instance;
    public GameObject circle;

    const float SPAWN_INTERVAL_MAX = 0.05f;
    const float SPAWN_INTERVAL_MIN = 0.05f;
    float spawnCounter;

    //Pool
    List<GameObject> Pool_circles = new List<GameObject>();

    int amountToPool = 20;
    Vector3 directionBias;

    #region Monobehavior
    void Awake()
    {
        instance = this;

        //Initialize pool
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject g = GameObject.Instantiate(circle, Vector3.zero, Quaternion.identity, transform) as GameObject;
            g.SetActive(false);
            Pool_circles.Add(g);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region Pool
    GameObject GetCircleFromPool()
    {
        for (int i = 0; i < Pool_circles.Count; i++)
        {
            if (!Pool_circles[i].activeSelf)
            {
                return Pool_circles[i];
            }
        }

        GameObject g = GameObject.Instantiate(circle, Vector3.zero, Quaternion.identity, transform) as GameObject;
        Pool_circles.Add(g);
        return g;
    }
    #endregion
}