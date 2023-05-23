using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIPanningMenuBG : MonoBehaviour
{    
    public float xSpeed = -10;

    float minX = -1920;
    float xOffset = 1920;
    Image image;

	void Start ()
    {
        image = GetComponent<Image>();
	}
	
	void Update ()
    {
        Vector3 pos = image.rectTransform.position;
        pos.x += xSpeed * Time.deltaTime;

        if (pos.x < minX)
        {
            pos.x += xOffset * 2;
        }

        image.rectTransform.position = pos;
    }
}
