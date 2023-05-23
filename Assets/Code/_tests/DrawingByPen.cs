using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingByPen : MonoBehaviour
{
    #region Variables
    public static DrawingByPen instance;

    //Public
    public Color[] hanabi0;
    public Color[] hanabi1;
    public Color[] hanabi2;
    public Color[] hanabi3;

    public Color bgColor;
    public Color bgColor1;
    public Color bgColor2;
    public Color bgColor3;

    [Header("Backgrounds")]
    public Transform BG1;
    public Transform BG2;

    //Static
    public static float BG_Bound_minX;
    public static float BG_Bound_minY;
    public static float BG_Bound_maxX;
    public static float BG_Bound_maxY;
    public static float BG_Bound_sizeX;
    public static float BG_Bound_sizeY;

    static int BG_pixelWidth = 160;
    static int BG_pixelHeight = 108;

    //Class reference
    GM gm;
    UIManager uiManager;
    FightSceneManager sceneM;

    //Cache
    Collider BG_col;
    Bounds BG_bounds;
    Texture2D BG1_texture;
    Texture2D BG2_texture;

    //Pixel data
    int[,] HanabiPixels_BG1;
    int[,] HanabiPixels_BG2;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        //Initialize
        instance = this;
        HanabiPixels_BG1 = new int[BG_pixelWidth, BG_pixelHeight];
        HanabiPixels_BG2 = new int[BG_pixelWidth, BG_pixelHeight];

        for (int i = 0; i < BG_pixelHeight; i++)
        {
            for (int j = 0; j < BG_pixelWidth; j++)
            {
                HanabiPixels_BG1[j, i] = 0;
                HanabiPixels_BG2[j, i] = 0;
            }
        }
        

        //Reference
        BG_col = BG1.GetComponent<Collider>();
        BG_bounds = BG_col.bounds;

        //Cache
        BG_Bound_minX = BG_bounds.min.x;
        BG_Bound_minY = BG_bounds.min.y;
        BG_Bound_maxX = BG_bounds.max.x;
        BG_Bound_maxY = BG_bounds.max.y;
        BG_Bound_sizeX = BG_bounds.size.x;
        BG_Bound_sizeY = BG_bounds.size.y;
    }
   
    public void Start ()
    {
        GenerateTexture();

        StartCoroutine(UpdateHanabi_BG1());
        StartCoroutine(UpdateHanabi_BG2());

        BG1.gameObject.SetActive(true);
        BG2.gameObject.SetActive(false);
        activeBGTexture = BG1_texture;
    }
    Vector3 worldMouse;
    private void Update()
    {
        worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMouse.z = 0;
        //Debug.DrawLine(worldMouse, Camera.main.transform.position, Color.yellow);

        //Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //p.z = 0;
        //transform.position = p;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BG1.gameObject.SetActive(true);
            BG2.gameObject.SetActive(false);
            activeBGTexture = BG1_texture;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            BG1.gameObject.SetActive(false);
            BG2.gameObject.SetActive(true);
            activeBGTexture = BG2_texture;
        }

        //Draw BG
        if (Input.GetKey(KeyCode.Alpha1))
        {
            activeBGColor = bgColor1;
            PaintBGPixel();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            activeBGColor = bgColor2;
            PaintBGPixel();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            activeBGColor = bgColor3;
            PaintBGPixel();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            activeBGColor = bgColor;
            PaintBGPixel();
        }
        

        //Draw hanabi BG1
        if (Input.GetKey(KeyCode.Q))
        {
            AddHanabiPixel_BG1(worldMouse, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            AddHanabiPixel_BG1(worldMouse, 1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            AddHanabiPixel_BG1(worldMouse, 2);
        }
        if (Input.GetKey(KeyCode.R))
        {
            AddHanabiPixel_BG1(worldMouse, 3);
        }

        if (Input.GetKey(KeyCode.T))
        {
            RemoveHanabiExplosion_BG1(worldMouse);
        }

        //Draw hanabi BG 2
        if (Input.GetKey(KeyCode.A))
        {
            AddHanabiPixel_BG2(worldMouse, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            AddHanabiPixel_BG2(worldMouse, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            AddHanabiPixel_BG2(worldMouse, 2);
        }
        if (Input.GetKey(KeyCode.F))
        {
            AddHanabiPixel_BG2(worldMouse, 3);
        }

        if (Input.GetKey(KeyCode.G))
        {
            RemoveHanabiExplosion_BG2(worldMouse);
        }
    }

    void LateUpdate()
    {
        //Apply texture
        BG1_texture.Apply();
        BG2_texture.Apply();
    }
    #endregion

    #region Painting
    Color activeBGColor;
    IntXY pix;
    Texture2D activeBGTexture;
    public void PaintBGPixel()
    {
        pix = WorldPosToPixelPos_BG(worldMouse);
        if (IsOutOfBounds(pix.x, pix.y))
            return;

        activeBGTexture.SetPixel(pix.x, pix.y, activeBGColor);
    }
    #endregion

    #region Hanabi explosion
    //We want to color the space dusts with different colors
    List<IntXY>[] activeHanabiExplosion_BG1 = { new List<IntXY>(), new List<IntXY>(), new List<IntXY>(), new List<IntXY>() };
    List<IntXY>[] activeHanabiExplosion_BG2 = { new List<IntXY>(), new List<IntXY>(), new List<IntXY>(), new List<IntXY>() };

    public void AddHanabiPixel_BG1(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples if current point is inactive
        if (HanabiPixels_BG1[pixX, pixY] <= 0)
        {
            activeHanabiExplosion_BG1[index].Add(pxl);
            HanabiPixels_BG1[pixX, pixY] = 1;
        }
    }

    public void AddHanabiPixel_BG2(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples if current point is inactive
        if (HanabiPixels_BG2[pixX, pixY] <= 0)
        {
            activeHanabiExplosion_BG2[index].Add(pxl);
            HanabiPixels_BG2[pixX, pixY] = 1;
        }
    }

    public void RemoveHanabiExplosion_BG1(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        for (int i = 0; i < 4; i++)
        {
            if (activeHanabiExplosion_BG1[i].Contains(pxl))
            {
                activeHanabiExplosion_BG1[i].Remove(pxl);
                HanabiPixels_BG1[pixX, pixY] = 0;
                BG1_texture.SetPixel(pixX, pixY, bgColor);
            }
        }
    }

    public void RemoveHanabiExplosion_BG2(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        for (int i = 0; i < 4; i++)
        {
            if (activeHanabiExplosion_BG2[i].Contains(pxl))
            {
                activeHanabiExplosion_BG2[i].Remove(pxl);
                HanabiPixels_BG2[pixX, pixY] = 0;
                BG2_texture.SetPixel(pixX, pixY, bgColor);
            }
        }
    }

    IEnumerator UpdateHanabi_BG1()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int group = 0; group < 4; group++)
            {
                for (int i = activeHanabiExplosion_BG1[group].Count - 1; i >= 0; i--)
                {
                    IntXY point = activeHanabiExplosion_BG1[group][i];
                    int _x = point.x;
                    int _y = point.y;
                    BG1_texture.SetPixel(_x, _y, GetRandomHanabiExplodeColor(group));
                }
            }
        }
    }

    IEnumerator UpdateHanabi_BG2()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int group = 0; group < 4; group++)
            {
                for (int i = activeHanabiExplosion_BG2[group].Count - 1; i >= 0; i--)
                {
                    IntXY point = activeHanabiExplosion_BG2[group][i];
                    int _x = point.x;
                    int _y = point.y;
                    BG2_texture.SetPixel(_x, _y, GetRandomHanabiExplodeColor(group));
                }
            }
        }
    }

    Color GetRandomHanabiExplodeColor(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return hanabi0[Random.Range(0, hanabi0.Length)];
        }
        else if (playerIndex == 1)
        {
            return hanabi1[Random.Range(0, hanabi1.Length)];
        }
        else if (playerIndex == 2)
        {
            return hanabi2[Random.Range(0, hanabi2.Length)];
        }
        else
        {
            return hanabi3[Random.Range(0, hanabi3.Length)];
        }
    }
    #endregion
    
    #region Paint Splatter
    public void PaintSplatterFlower(Vector2 worldPos, Vector2 vel, int index)
    {
        for (var i = 0; i < Random.Range(7, 10); i++) //Random splatters
        {
            SplatterStrand s = new SplatterStrand(worldPos, vel, index, true);
            StartCoroutine(s.PaintStrand());
        }
    }
    #endregion

    #region Texture generate
    void GenerateTexture()
    {
        //BG1
        Material material;
        BG1_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.RGB24, false);

        material = BG1.GetComponent<Renderer>().material;
        material.mainTexture = BG1_texture;


        for (int i = 0; i < BG_pixelWidth; i++)
        {
            for (int j = 0; j < BG_pixelHeight; j++)
            {
                BG1_texture.SetPixel(i, j, bgColor);
            }
        }

        BG1_texture.filterMode = FilterMode.Point;
        BG1_texture.Apply();

        //BG2
        Material material2;
        BG2_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.RGB24, false);

        material2 = BG2.GetComponent<Renderer>().material;
        material2.mainTexture = BG2_texture;

        for (int i = 0; i < BG_pixelWidth; i++)
        {
            for (int j = 0; j < BG_pixelHeight; j++)
            {
                BG2_texture.SetPixel(i, j, bgColor);
            }
        }

        BG2_texture.filterMode = FilterMode.Point;
        BG2_texture.Apply();
    }
    #endregion

    #region Util 
    public static IntXY WorldPosToPixelPos_BG(Vector2 p)
    {
        return new IntXY(WorldPosXToPixelX_BG(p.x), WorldPosYToPixelY_BG(p.y));
    }

    public static IntXY WorldPosToPixelPos_BG(float x, float y)
    {
        return new IntXY(WorldPosXToPixelX_BG(x), WorldPosYToPixelY_BG(y));
    }

    public static int WorldPosXToPixelX_BG(float x)
    {
        //Use the world pos.x and .y to find the u,v coordinate on the Texture
        x = (x - BG_Bound_minX) / BG_Bound_sizeX;
        //Next find the pixel position on the texture
        x = x * BG_pixelWidth;
        return (int)x;
    }

    public static int WorldPosYToPixelY_BG(float y)
    {
        y = (y - BG_Bound_minY) / BG_Bound_sizeY;
        y = y * BG_pixelHeight;
        return (int)y;
    }

    bool IsOutOfBounds(int pixX, int pixY)
    {
        if (pixX > BG_pixelWidth - 1 || pixX < 0 ||
            pixY > BG_pixelHeight - 1 || pixY < 0)
        {
            return true;
        }
        return false;
    }
    #endregion
}