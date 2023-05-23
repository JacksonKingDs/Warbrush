using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardStar : MonoBehaviour 
{
    #region Fields
    public Image img;
    public Vector3[] playerScoreLocations;

    float maxX = 300f;
    float minX = -300f;
    float maxY = 0f;
    float minY = -200f;

    float scaleMin = 0.7f;
    float scaleMax = 0.8f;

    Vector3 floatDir = new Vector3(0f, 0.02f, 0f);
    #endregion

    #region MonoBehaviour
    IEnumerator DelayedDestroy () 
	{
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        transform.Translate(floatDir);
    }

    public void Initialize (Color col, int winnerIndex) 
	{
        img.color = col;
        RandomizePos(winnerIndex);
        StartCoroutine(DelayedDestroy());
    }

    void RandomizePos (int winner)
    {
        //transform.localPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
        Vector3 center = playerScoreLocations[winner];
        center = new Vector3(
            center.x + Random.Range(-50f, 50f),
            center.y + Random.Range(-50f, 50f),
            0f);

        transform.localPosition = center;

        //float s = Random.Range(scaleMin, scaleMax);
        //transform.localScale = new Vector3(s, s, 1f);
    }
	#endregion
}