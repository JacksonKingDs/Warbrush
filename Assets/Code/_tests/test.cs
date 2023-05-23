using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test: MonoBehaviour 
{
    #region Fields
    
    #endregion

    #region MonoBehaviour   
    private void Update()
    {
        Vector2 pos = transform.position;

        if (Input.GetMouseButton(0))
        {
            var v3 = Input.mousePosition;
            v3.z = 10f;
            v3 = Camera.main.ScreenToWorldPoint(v3);

            Vector2 Indir = Vector3.zero - v3;
            Vector2 outDir = Vector2.Reflect(Indir, Vector2.up);

            Debug.DrawRay(v3, Indir, Color.red, 1f);
            Debug.DrawRay(Vector3.zero, Vector3.up, Color.blue, 1f);
            Debug.DrawRay(Vector3.zero, outDir, Color.yellow, 1f);
        }
    }

    void ReflectOffV3Zero ()
    {
        var v3 = Input.mousePosition;
        v3.z = 10f;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        Vector2 Indir = Vector3.zero - v3;
        Vector2 outDir = Vector2.Reflect(Indir, Vector2.up);

        Debug.DrawRay(v3, Indir, Color.red, 1f);
        Debug.DrawRay(Vector3.zero, Vector3.up, Color.blue, 1f);
        Debug.DrawRay(Vector3.zero, outDir, Color.yellow, 1f);
    }

    void ReflectV2 ()
    {
        //Get mouse pos
        var v3 = Input.mousePosition;
        v3.z = 10f;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        Vector2 Indir = Vector3.zero - v3;
        Vector2 outDir = Vector2.Reflect(Indir, Vector2.up);

        Debug.DrawRay(v3, Indir, Color.red, 1f);
        Debug.DrawRay(Vector3.zero, Vector3.up, Color.blue, 1f);
        Debug.DrawRay(Vector3.zero, outDir, Color.yellow, 1f);
    }



    #endregion
}
