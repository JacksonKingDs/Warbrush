using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceDustOrigin : MonoBehaviour
{
    //Const
    const float FULL_LIFE_TIME = 0.5f;

    //Refs
    BGTextureManager painter;
    SettingsAndPrefabRefs refs;
    Transform trans;
    Rigidbody2D rb;
    //GravityPointsManager gravityM;
    int index;

    //Stats
    float lifeTime;
    float speed = 10f;
    //bool anitGravity = false;

    int waitBeforeDraw = 6;

    public void Initialize()
    {
        painter = BGTextureManager.instance;
        refs = SettingsAndPrefabRefs.instance;
        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        //gravityM = GravityPointsManager.instance;
    }

    public void Activation(Vector3 pos, Quaternion rot, int index, bool small)
    {
        this.index = index;
        trans.position = pos;
        trans.rotation = rot;
        rb.velocity = speed * trans.up;
        lifeTime = FULL_LIFE_TIME;
        if (small)
            lifeTime = FULL_LIFE_TIME * 0.5f;

        waitBeforeDraw = 6;

        StartCoroutine(IntervalDraw());
    }

    
    int updateDirCounter = 0; 
    IEnumerator IntervalDraw ()
    {
        while (waitBeforeDraw > 0)
        {
            waitBeforeDraw--;
            yield return null;
        }

        while (lifeTime > 0f)
        {
            //Update rotation
            //updateDirCounter--;
            //if (updateDirCounter <= 0)
            //{
            //    updateDirCounter = 2;
            //    UpdateDir();
            //}
            if (TankUtil.IsWorldPosOutOfBounds(trans.position))
                yield break;
            //pos = TankUtil.WrapWorldPosTankPos(trans.position);
            painter.AddHanabiExplosion(trans.position, index);

            //UpdateDir();

            ////Move up
            //transform.Translate(trans.up * Time.deltaTime * speed, Space.World);
            //pos = TankUtil.WrapWorldPosTankPos(trans.position);
            //trans.position = pos;

            //targetDir = Vector3.RotateTowards(trans.up, Vector3.left, rotSpeed, 0.0f);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDir);

            lifeTime -= Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }

    void UpdateDir ()
    {
        //Debug.Log(gravityM);
        //    Debug.Log(trans);
        //trans.rotation = Quaternion.LookRotation(Vector3.forward, gravityM.GetWeightedRotation(trans.position, anitGravity));
    }

    #region Deactivation
    void Deactivate()
    {
        refs.Push_OceanLine(gameObject);
    }
    #endregion
}