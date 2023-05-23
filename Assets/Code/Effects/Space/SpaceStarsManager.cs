using UnityEngine;
using System.Collections;

public class SpaceStarsManager : MonoBehaviour
{
    public SpriteRenderer moon;
    public Color defaultMoonColor;
    public Color[] winningColor;


    public SpaceStar[] allStars;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //#region Public
    //public Color AddStar (int index)
    //{
    //    UpdateMoonColor();
    //    return winningColor[index];
    //}


    //#endregion
    //void UpdateMoonColor ()
    //{       
    //    int[] scores = { 0, 0, 0, 0 };

    //    //Add up scores
    //    foreach (var s in allStars)
    //    {
    //        if (s.myIndex > -1 && s.myIndex < 4)
    //        {
    //            ++scores[s.myIndex];
    //        }
    //    }

    //    //Find leader
    //    int highestScore = -1;
    //    int winningIndex = -1;

    //    for(int i = 0; i < 4; i++)
    //    {
    //        if (scores[i] > highestScore)
    //        {
    //            winningIndex = i;
    //            highestScore = scores[i];
    //        }
    //        else if (scores[i] == highestScore) //Do not allow draws
    //        {
    //            winningIndex = -1;
    //        }
    //    }

    //    //Set moon color
    //    if (winningIndex != -1)
    //    {
    //        moon.color = winningColor[winningIndex];
    //    }
    //    else
    //    {
    //        moon.color = defaultMoonColor;
    //    }
    //}
}
