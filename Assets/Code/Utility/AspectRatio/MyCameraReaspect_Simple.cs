using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCameraReaspect_Simple : MonoBehaviour
{
    public Camera cam;
    public Vector2 ratio = new Vector2(16, 9);

    Rect originalCamRect;

    private void Start()
    {
        originalCamRect = cam.rect;
        //Debug.Log("originalCamRect: " + originalCamRect);
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
            ratioCorrectedRect.height = 1;
            ratioCorrectedRect.width = targetAspect / currentAspect;
        }
        else //Current aspect is too narrow in x and too tall in y, then keep width but bring down height. 
        {
            ratioCorrectedRect.width = 1;
            ratioCorrectedRect.height = currentAspect / targetAspect;
            //These are % percentages in relation to the actual pixels, in order to produce a 16:9 ratio, while both numbers must be under 1f. 
        }

        //Step 2: 


        // Resize and position the viewport to fit in it's original position on screen (adhering to a given anchor point)
        Rect screenLetterbox = new Rect();
        screenLetterbox.width = 1f;
        screenLetterbox.height = ratioCorrectedRect.height;
        screenLetterbox.x = 0f;
        screenLetterbox.y = (1f - screenLetterbox.height) / 2f;

        Rect screenPillarbox = new Rect();
        screenPillarbox.width = ratioCorrectedRect.width;
        screenPillarbox.height = 1f;
        screenPillarbox.x = (1f - screenPillarbox.width) /2f;
        screenPillarbox.y = 0f;

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
