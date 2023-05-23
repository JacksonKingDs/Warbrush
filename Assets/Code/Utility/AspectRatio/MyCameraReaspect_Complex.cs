using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCameraReaspect_Complex : MonoBehaviour
{
    public Camera cam;
    public Vector2 ratio = new Vector2(16, 9);
    public ECameraAnchor anchor = ECameraAnchor.Center;

    Rect originalCamRect;
    Vector2 vectorAnchor;

    private void Start()
    {
        originalCamRect = cam.rect;
        //Debug.Log("originalCamRect: " + originalCamRect);
        switch (anchor)
        {
            case ECameraAnchor.Center:
                vectorAnchor = new Vector2(0.5f, 0.5f);
                break;
            case ECameraAnchor.Top:
                vectorAnchor = new Vector2(0.5f, 1f);
                break;
            case ECameraAnchor.Bottom:
                vectorAnchor = new Vector2(0.5f, 0f);
                break;
            case ECameraAnchor.Left:
                vectorAnchor = new Vector2(0f, 0.5f);
                break;
            case ECameraAnchor.Right:
                vectorAnchor = new Vector2(1f, 0.5f);
                break;
            case ECameraAnchor.TopLeft:
                vectorAnchor = new Vector2(0f, 1f);
                break;
            case ECameraAnchor.TopRight:
                vectorAnchor = new Vector2(1f, 1f);
                break;
            case ECameraAnchor.BottomLeft:
                vectorAnchor = new Vector2(0f, 0f);
                break;
            case ECameraAnchor.BottomRight:
                vectorAnchor = new Vector2(1f, 0f);
                break;
        }

        SetCameraRatio();
    }

    void Update()
    {
        SetCameraRatio();
    }

    public void SetCameraRatio()
    {
        //Step 1: adjust Rect to the correct ratio. Shorten X or Y for whichever is longer. 
        float targetAspect = ratio.x / ratio.y;
        float currentAspect = ((float)Screen.width) / ((float)Screen.height);
        
        Rect ratioCorrectedRect = new Rect();

        if (currentAspect > targetAspect) //If current aspect is too wide in x and too short in y for 16:9, then keep height but narrow down width. 
        {
            ratioCorrectedRect.height = originalCamRect.height;
            ratioCorrectedRect.width = targetAspect / currentAspect;
        }
        else //Current aspect is too narrow in x and too tall in y, then keep width but bring down height. 
        {
            ratioCorrectedRect.width = originalCamRect.width;
            ratioCorrectedRect.height = currentAspect / targetAspect;
            //These are % percentages in relation to the actual pixels, in order to produce a 16:9 ratio, while both numbers must be under 1f. 
        }

        //Step 2: 


        // Resize and position the viewport to fit in it's original position on screen (adhering to a given anchor point)
        Rect screenLetterbox = new Rect();
        screenLetterbox.width = originalCamRect.width;
        screenLetterbox.height = originalCamRect.width * (ratioCorrectedRect.height / originalCamRect.width);
        screenLetterbox.x = originalCamRect.x;
        screenLetterbox.y = Mathf.Lerp(originalCamRect.y, originalCamRect.y + (originalCamRect.height - ratioCorrectedRect.height), vectorAnchor.y); 

        Rect screenPillarbox = new Rect();
        screenPillarbox.width = originalCamRect.height * (ratioCorrectedRect.width / ratioCorrectedRect.height);
        screenPillarbox.height = originalCamRect.height;
        screenPillarbox.x = Mathf.Lerp(originalCamRect.x, originalCamRect.x + (originalCamRect.width - ratioCorrectedRect.width), vectorAnchor.x);
        screenPillarbox.y = originalCamRect.y;

        // Choose the smaller of the 2
        if (screenLetterbox.height >= screenPillarbox.height && screenLetterbox.width >= screenPillarbox.width)
        {
            cam.rect = screenPillarbox;
        }
        else
        {
            cam.rect = screenLetterbox;
            
        }
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 20, 200, 20), "Aspect: " + cam.aspect);
    //}
}

public enum ECameraAnchor
{
    Center,
    Top,
    Bottom,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}