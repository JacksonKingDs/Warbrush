using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour 
{
    #region Fields
    public Transform[] targets;

    float rotSpeed = 1f;
    #endregion

    void Start () 
	{
        curDir = transform.up;
	}
    Vector2 targetDir;
    Vector2 curDir;

    bool turnLeft;
    Vector3 cross;

    void Update () 
	{
        UpdateDirToClosestEnemy();

        Debug.DrawRay(transform.position, transform.up, Color.white);
        Debug.DrawRay(transform.position, targetDir, Color.green);

        cross = Vector3.Cross(transform.up, targetDir);
        turnLeft = cross.z < 0;
    }

    void UpdateDirToClosestEnemy()
    {
        float shortestDist = float.MaxValue;

        foreach (var v in targets)
        {

            Vector2 dir = v.position - transform.position;
            float dist = dir.magnitude;
            if (dist < shortestDist)
            {
                targetDir = dir;
                shortestDist = dist;
                //shortestIndex = i;
            }
        }
    }
}