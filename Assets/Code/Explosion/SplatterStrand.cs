using UnityEngine;
using System.Collections;

public class SplatterStrand
{
    //Variables
    public Vector2 worldPos;
    public int index;
    public Vector2 dir;

    public int size;
    public int decay;

    public float velocity;
    public float decelerationMod = 0.7f;

    public int iterationMax;
    public int iterationCount;

    public float drawDelay;

    //Const: these modify the splatter strand overtime
    const int Size_min = 8; //Starting size
    const int Size_max = 10;
    const int Decay_min = 2;
    const int Decay_max = 5;

    const float Vel_min = 0.6f;
    const float Vel_max = 0.8f;

    const int Iterations_min = 4;
    const int Iterations_max = 14;

    const float DrawDelay_min = 0.02f;
    const float DrawDelay_max = 0.03f;

    //Class reference
    BGTextureManager painter;

    bool isEnemy;
    bool isCombat;

    //Ctor
    public SplatterStrand(Vector2 worldPos, Vector2 initialDir, int index, bool isCombat)
    {
        painter = BGTextureManager.instance;
        iterationCount = 0;

        this.worldPos = worldPos;
        dir = (initialDir.normalized + Random.insideUnitCircle.normalized).normalized;
        this.index = index;
        isEnemy = index == GM.enemyIndex;

        this.isCombat = isCombat;

        size = Random.Range(Size_min, Size_max + 1); //This is the pixel size
        decay = Random.Range(Decay_min, Decay_max + 1);

        velocity = Random.Range(Vel_min, Vel_max);

        iterationMax = Random.Range(Iterations_min, Iterations_max + 1);
        drawDelay = Random.Range(DrawDelay_min, DrawDelay_max);
    }

    public IEnumerator PaintStrand()
    {        
        if (GM.gameMode == GameMode.PVP_Night || GM.gameMode == GameMode.Coop_Torch) 
        {
            size += 2;
            iterationCount = -1;
        }

        //If itererated enough times or if the splatter gets too small, then stop painting further
        while (iterationCount < iterationMax && size > 0)
        {
            iterationCount++;

            //Paint circle
            IntXY pixelPos = BGTextureManager.WorldPosToPixelPos_BG(worldPos);

            //Color
            if (isEnemy)
            {
                foreach (IntXY offset in CircularOffset.Circles[size])
                {
                    painter.PaintBG_ARCADE_ENEMY(pixelPos.x + offset.x, pixelPos.y + offset.y);
                }
            }
            else
            {
                foreach (IntXY offset in CircularOffset.Circles[size])
                {
                    painter.PaintTankBGPixel(pixelPos.x + offset.x, pixelPos.y + offset.y, index, false);
                }                
            }

            //Update position
            if (size > 5)
                velocity *= decelerationMod; //Let the spread slow down as the paint gets smaller

            worldPos = worldPos + dir * velocity;

            //Update size
            size -= decay;
                
            yield return new WaitForSeconds(drawDelay);
        }
    }
}