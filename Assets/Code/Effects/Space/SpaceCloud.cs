using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCloud : MonoBehaviour 
{
    const float leftBorder = 12f;

    const float moveSpeed = 0.001f;
    Transform trans;

    #region MonoBehaviour    
    void Awake () 
	{
        trans = transform;
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
            trans.position = new Vector3(-leftBorder, trans.position.y, -1f);
        }
    }
}
