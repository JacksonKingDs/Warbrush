using UnityEngine;
using System.Collections;

public class TankUtil
{
    public const float topBound = 5f;
    public const float rightBound = 7.5f;
    public const float warpHeight = topBound * 2f;
    public const float warpWidth = rightBound * 2;
    const float modifier = 0.15f;


    public static Vector3 WrapWorldPos  (Vector3 pos)
    {
        //Warping
        if (pos.x < -rightBound) //Warp left to right
        {
            //Debug.Log("l 2 r");
            return new Vector3(pos.x + warpWidth, pos.y, pos.z);
        }
            
        if (pos.x > rightBound) //Warp left to right
        {
            //Debug.Log("r 2 l");
            return new Vector3(pos.x - warpWidth, pos.y, pos.z);
        }
        
        if (pos.y < -topBound) //Warp left to right
        {
            //Debug.Log("b 2 t");
            return new Vector3(pos.x, pos.y + warpHeight, pos.z);
        }
       
        if (pos.y > topBound) //Warp left to right
        {
            //Debug.Log("t 2 b");
            return new Vector3(pos.x, pos.y - warpHeight, pos.z);
        }
        return pos;
    }

    //The pixels and tank position has a weird interaction, the modifier offset just makes sure it doesn't get messed up
    public static Vector3 WrapWorldPosTankPos(Vector3 pos)
    {
        //Warping
        if (pos.x < -rightBound) //Warp left to right
        {
            //Debug.Log("l 2 r");
            return new Vector3(pos.x + warpWidth - modifier, pos.y, pos.z);
        }

        if (pos.x > rightBound) //Warp left to right
        {
            //Debug.Log("r 2 l");
            return new Vector3(pos.x - warpWidth + modifier, pos.y, pos.z);
        }

        if (pos.y < -topBound) //Warp left to right
        {
            //Debug.Log("b 2 t");
            return new Vector3(pos.x, pos.y + warpHeight, pos.z);
        }

        if (pos.y > topBound) //Warp left to right
        {
            //Debug.Log("t 2 b");
            return new Vector3(pos.x, pos.y - warpHeight, pos.z);
        }


        return pos;
    }

    public static bool IsWorldPosOutOfBounds (Vector3 pos)
    {
        if (pos.x > BGTextureManager.BG_Bound_maxX || pos.x < BGTextureManager.BG_Bound_minX ||
            pos.y > BGTextureManager.BG_Bound_maxY || pos.y < BGTextureManager.BG_Bound_minY)
        {
            return true;
        }
        return false;
    }
}