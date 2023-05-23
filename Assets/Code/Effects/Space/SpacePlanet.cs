using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePlanet : MonoBehaviour 
{
    public SpriteRenderer rend;
    public Color shiningColor;
    public Color startingColor;
    float shiningDuration;
    
    Color targetColor; //Lerping

    #region Methods
    private void Awake()
    {
        targetColor = startingColor;
    }

    private void Update()
    {
        if (shiningDuration > 0)
        {
            //Tick timer
            shiningDuration -= Time.deltaTime;
            if (shiningDuration < 0f)
            {
                shiningDuration = 0f;
                targetColor = startingColor;
            }
        }

        rend.color = Color.Lerp(rend.color, targetColor, Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerDeadTank)
            {
                Rigidbody2D r = go.GetComponent<Rigidbody2D>();
                r.velocity = Vector3.zero;
                r.angularVelocity = 0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerPlayer)
            {
                go.GetComponent<TankControllerBase>().DisableSlide();
            }
        }
        targetColor = shiningColor;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerPlayer)
            {
                go.GetComponent<TankControllerBase>().EnableSlide();
            }
        }
    }
    #endregion
}
