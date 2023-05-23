using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleWeed : MonoBehaviour, IObstacle
{
    public Transform spriteTrans;
    BGTextureManager BG_Painter;

    const float leftBorder = 7.8f;
    const float upBoorder = 4f;

    const float moveSpeed = 0.1f;
    Transform trans;

    List<TankControllerBase> playersInCloud = new List<TankControllerBase>();

    #region MonoBehaviour    
    void Awake () 
	{
        trans = transform;
        trans.position = new Vector3(trans.position.x, Random.Range(-upBoorder, upBoorder), -0.2f);
        spriteTrans.Rotate(new Vector3(0f, 0f, Random.Range(-90f, 90f)));
        StartCoroutine(PeriodicUpdate());
    }
	
    void Start()
    {
        BG_Painter = BGTextureManager.instance;
    }

	IEnumerator PeriodicUpdate () 
	{
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            trans.Translate(Vector3.left * moveSpeed);
            spriteTrans.Rotate(new Vector3(0f, 0f, 30f));

            //BG_Painter.PaintTumbleWeed(trans.position);

            OutOfBoundsCheck();
        }
        
    }
    #endregion

    void OutOfBoundsCheck ()
    {
        if (trans.position.x < -leftBorder)
        {
            trans.position = new Vector3(leftBorder, Random.Range(-upBoorder, upBoorder), -0.2f);
        }
    }

    #region Methods

    public void TakeDmg(int dmg = 1)
    {
    }
    #endregion
}
