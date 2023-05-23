using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OceanLineTrail
{
    public IntXY pixelPos;
    public Color color;

    public OceanLineTrail(IntXY pixelPos, Color color)
    {
        this.pixelPos = pixelPos;
        this.color = color;
    }
}

public class OceanLineOrigin : MonoBehaviour
{
    //public List<Transform> paintPoints;
    float speed = 0.5f;
    BGTextureManager painter;
    SettingsAndPrefabRefs refs;
    Transform trans;

    const float fullLifeTime = 0.6f;
    float lifeTime;

    bool touchedColor = false;
    Color color;

    public void Initialize()
    {
        painter = BGTextureManager.instance;
        refs = SettingsAndPrefabRefs.instance;
        trans = transform;
    }

    public void Activation(Vector3 pos, Quaternion rot, float forcePercentage)
    {
        trans.position = pos;
        trans.rotation = rot;

        lifeTime = forcePercentage * fullLifeTime;
        StartCoroutine(DelayedDeactivation());
        touchedColor = false;
        StartCoroutine(TouchedColorCheck());
    }

    void Update()
    {
        transform.Translate(trans.up * Time.deltaTime * speed, Space.World);
        //Debug.DrawRay(transform.position, transform.up, Color.yellow);
        painter.AddWaterRipple(trans.position, touchedColor, color); //If touched color, then set the FG to the new color
    }

    IEnumerator TouchedColorCheck ()
    {
        while(!touchedColor)
        {
            yield return null; //Skip a few frames to save calculations
            yield return null;
            yield return null;
            touchedColor = painter.GetFGWaterColor(trans.position, ref color);
        }
    }

    IEnumerator DelayedDeactivation ()
    {
        yield return new WaitForSeconds(lifeTime);
        Deactivate();
    }

    void Deactivate ()
    {
        
        refs.Push_OceanLine(gameObject);
    }
}