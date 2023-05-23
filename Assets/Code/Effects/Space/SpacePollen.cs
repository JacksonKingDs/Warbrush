using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpacePollen : MonoBehaviour
{
    //Refs
    Vector3 moveDir = new Vector3(0.02f, 0.002f, 0f);
    BGTextureManager painter;
    Transform trans;

    //Stats
    float speed = 10f;
    bool activated = false;
    bool anitGravity = false;

    void Awake()
    {
        painter = BGTextureManager.instance;
        trans = transform;
        StartCoroutine(IntervalDraw());

        moveDir = moveDir * Random.Range(0.2f, 1f);
    }

    Vector3 pos;
    int updateDirCounter = 0; 
    IEnumerator IntervalDraw ()
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));

        while (true)
        {
            trans.position = TankUtil.WrapWorldPosTankPos(trans.position);
            trans.Translate(moveDir);

            //painter.AddSpacePollen(pos);
            
            yield return new WaitForSeconds(0.2f);
        }
    }
}