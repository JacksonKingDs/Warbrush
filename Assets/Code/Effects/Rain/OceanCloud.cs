using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCloud : MonoBehaviour 
{
    const float leftBorder = 12f;
    const float upBoorder = 5f;

    const float moveSpeed = 0.005f;
    Transform trans;

    List<TankControllerBase> playersInCloud = new List<TankControllerBase>();

    #region MonoBehaviour    
    void Awake () 
	{
        trans = transform;
        trans.position = new Vector3(trans.position.x, Random.Range(-upBoorder, upBoorder), -1f);
    }
	
	void FixedUpdate () 
	{
        trans.Translate(Vector3.right * moveSpeed);
        OutOfBoundsCheck();

    }
    #endregion

    void OutOfBoundsCheck ()
    {
        if (trans.position.x > leftBorder)
        {
            trans.position = new Vector3(-leftBorder, Random.Range(-upBoorder, upBoorder), -1f);
            RemoveAll();
        }
    }

    #region Methods
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>();
                playersInCloud.Add(enemyPlayer);
                enemyPlayer.HideInCloud(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase enemyPlayer = go.GetComponent<TankControllerBase>();
                
                enemyPlayer.RevealFromCloud();
            }
        }
    }

    void RemoveAll ()
    {
        for (int i = playersInCloud.Count - 1; i >= 0; i--)
        {
            playersInCloud[i].RevealFromCloud();
            playersInCloud.Remove(playersInCloud[i]);
        }
    }
    #endregion
}
