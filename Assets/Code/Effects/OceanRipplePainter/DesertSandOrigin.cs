using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DesertSandOrigin : MonoBehaviour
{
    //Const
    const float FULL_LIFE_TIME = 20f;
    const int SCREEN_WIDTH = 15;

    //Refs
    BGTextureManager painter;
    SettingsAndPrefabRefs refs;
    Transform trans;

    //Stats
    float life;
    float speed = 10f;
    bool drawing = false;

    public void Initialize()
    {
        painter = BGTextureManager.instance;
        refs = SettingsAndPrefabRefs.instance;
        trans = transform;
    }

    public void Activation(Vector3 pos)
    {
        life = FULL_LIFE_TIME;
        trans.position = pos;
        drawing = true;
        StartCoroutine(DelayedDeactivation());
        StartCoroutine(IntervalDraw());
    }

    void Update()
    {        
        //Debug.DrawRay(transform.position, transform.up, Color.yellow);
        painter.AddSandstormPoint(trans.position); //If touched color, then set the FG to the new color
    }

    Vector3 pos;
    IEnumerator IntervalDraw ()
    {
        while (drawing)
        {
            //Move left
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            pos = trans.position;
            if (pos.x < -7.3f)
            {
                pos.x += 14.6f;
                trans.position = pos;
            }
                
            //targetDir = Vector3.RotateTowards(trans.up, Vector3.left, rotSpeed, 0.0f);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDir);
            //painter.AddSandstormPoint(pos);
            yield return new WaitForSeconds(0.2f);
        }

        Deactivate();
    }

    IEnumerator DelayedDeactivation ()
    {
        yield return new WaitForSeconds(FULL_LIFE_TIME);
        drawing = false;
    }

    void Deactivate ()
    {
        refs.Push_OceanLine(gameObject);
    }
}