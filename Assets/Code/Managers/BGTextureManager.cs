using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGTextureManager : MonoBehaviour
{
    #region Variables
    public static BGTextureManager instance;

    //Public
    public Color sandColor1;
    public Color sandColor2;
    public Color sandColor3;
    public Color wormFullColor;
    public Color SpookyFogColor;
    public Color rippleFullColor;
    public Color spookySplatterColor;

    [Header("Enemy")]
    public Color enemy_body_combat;
    public Color enemy_dark_combat;
    public Color enemy_transp_combat;

    public Color enemy_body_Night;
    public Color enemy_dark_Night;
    public Color enemy_transp_Night;

    public Color enemy_body_ocean;
    public Color enemy_dark_ocean;
    public Color enemy_transp_ocean;

    public Color enemy_body_desert;
    public Color enemy_dark_desert;
    public Color enemy_transp_desert;

    public Color enemy_body_spooky;
    public Color enemy_dark_spooky;
    public Color enemy_transp_spooky;

    public Color enemy_body_arcade;
    public Color enemy_dark_arcade;
    public Color enemy_transp_arcade;

    [Header("Space")]
    public Color spaceStarColor;
    public Color[] hanabiExplode0;
    public Color[] hanabiExplode1;
    public Color[] hanabiExplode2;
    public Color[] hanabiExplode3;

    public Color[] hanabiTrail0;
    public Color[] hanabiTrail1;
    public Color[] hanabiTrail2;
    public Color[] hanabiTrail3;

    public Color constellationColor;

    [Header("SpookyFG")]
    public Color torch_1;
    public Color torch_2;
    public Color torch_3;
    public Color torch_4;

    //public Material mat_with_shader_unlitTexture; 
    //public Material mat_with_shader_unlitTransparent;
    ////Unity sometimes loses reference to a shader directly assigned in inspector when opening project. So I reference the shader that is attached to a material instead.
    //Shader shaderTexture;
    //Shader shaderTransparent;

    [Header("Backgrounds")]
    public Transform BG;
    public Transform FG;
    public Transform FFG_Ripples;
    public Transform FFFG_Sandstorm;

    [Header("Desert")]
    public Color wurmColor2;
    public Color wurmColor1;

    //Hidden public
    [HideInInspector] public Color[] tankColors = new Color[4]; //Normal
    [HideInInspector] public Color[] colorsTrans = new Color[4]; //FG
    [HideInInspector] public Color[] colorsDark = new Color[4]; //Skid
    [HideInInspector] public Color[] colorsSkid = new Color[4]; //Skid
    [HideInInspector] public Color enemy_dark;
    [HideInInspector] public Color enemy_transp;
    [HideInInspector] public Color enemy_body;

    //Static
    public static float BG_Bound_minX;
    public static float BG_Bound_minY;
    public static float BG_Bound_maxX;
    public static float BG_Bound_maxY;
    public static float BG_Bound_sizeX;
    public static float BG_Bound_sizeY;

    static int BG_pixelWidth = 160;
    static int BG_pixelHeight = 108;
    //static int BG_side_offset = 160;
    int totalPixels;
    float totalPixelsFloat;
    float winningPixelsAmount;

    //Class reference
    GM gm;
    UIManager uiManager;
    FightSceneManager sceneM;

    //Cache
    Collider BG_col;
    Bounds BG_bounds;
    Color bgColor;
    Texture2D BG_texture;
    Texture2D FG_texture;
    Texture2D FFG_texture;
    Texture2D FFFG_texture;

    //Pixel data
    //When a tank/bullet of an index passes over the pixel, check if bot layer recorded
    //that pixel (which means it is already colored in BG); check if top layer recorded
    //it (which means a transparent version of the color is colored in FG)
    int[,] FGPixels;
    int[,] BGPixels;
    int[,] FFG_RipplePixels; //Ripple, worm
    int[,] FFFG_FogPixels; //Sandstorm, spooky fog
    int[] playerPixelScores = new int[] { 0, 0, 0, 0 };

    //Splatter
    List<SplatterStrand> splatter = new List<SplatterStrand>();

    //Find winner
    int previousWinnerIndex = -1;

    //Properties
    public Texture2D activeTexture { get; private set; }
    bool isUpdateFFG_Ripple = false;
    bool isUpdateFFFG_Fog = false;
    bool isCombatMode;
    bool isDesertMode;
    bool isSpaceMode;

    //Water ripple color
    Color ripple1;
    Color ripple2;
    Color ripple3;
    Color ripple4;
    Color ripple5;
    Color fog9 = new Color(0f, 0f, 0f, 0.55f);
    Color fog8 = new Color(0f, 0f, 0f, 0.6f);
    Color fog7 = new Color(0f, 0f, 0f, 0.65f);
    Color fog6 = new Color(0f, 0f, 0f, 0.7f);
    Color fog5 = new Color(0f, 0f, 0f, 0.75f);
    Color fog4 = new Color(0f, 0f, 0f, 0.8f);
    Color fog3 = new Color(0f, 0f, 0f, 0.85f);
    Color fog2 = new Color(0f, 0f, 0f, 0.9f);
    Color fog1 = new Color(0f, 0f, 0f, 0.95f);

    Color black9 = new Color(0f, 0f, 0f, 0.1f);
    Color black8 = new Color(0f, 0f, 0f, 0.2f);
    Color black7 = new Color(0f, 0f, 0f, 0.3f);
    Color black6 = new Color(0f, 0f, 0f, 0.4f);
    Color black5 = new Color(0f, 0f, 1f, 0.5f);
    Color black4 = new Color(0f, 1f, 1f, 0.6f);
    Color black3 = new Color(0f, 0f, 0f, 0.7f);
    Color black2 = new Color(0f, 0f, 0f, 0.8f);
    Color black1 = new Color(0f, 0f, 0f, 0.9f);
    #endregion

    #region Initialization
    void Awake()
    {
        //Initialize
        instance = this;
        FGPixels = new int[BG_pixelWidth, BG_pixelHeight];
        BGPixels = new int[BG_pixelWidth, BG_pixelHeight];
        FFG_RipplePixels = new int[BG_pixelWidth, BG_pixelHeight];
        FFFG_FogPixels = new int[BG_pixelWidth, BG_pixelHeight];

        for (int i = 0; i < BG_pixelHeight; i++)
        {
            for (int j = 0; j < BG_pixelWidth; j++)
            {
                BGPixels[j, i] = FGPixels[j, i] = GM.emptyIndex;
                FFG_RipplePixels[j, i] = 0;
            }
        }

        //shaderTexture = mat_with_shader_unlitTexture.shader;
        //shaderTransparent = mat_with_shader_unlitTransparent.shader;

        //Reference
        BG_col = BG.GetComponent<Collider>();
        BG_bounds = BG_col.bounds;

        //Cache
        BG_Bound_minX = BG_bounds.min.x;
        BG_Bound_minY = BG_bounds.min.y;
        BG_Bound_maxX = BG_bounds.max.x;
        BG_Bound_maxY = BG_bounds.max.y;
        BG_Bound_sizeX = BG_bounds.size.x;
        BG_Bound_sizeY = BG_bounds.size.y;
        totalPixels = BG_pixelWidth * BG_pixelHeight;
        totalPixelsFloat = (float)totalPixels;
    }
   
    public void SceneInitialization ()
    {
        gm = GM.instance;
        uiManager = UIManager.instance;
        sceneM = FightSceneManager.instance;

        //Reference Colors
        tankColors = GM.pallet.Tank;
        colorsTrans = GM.pallet.Trans;
        colorsDark = GM.pallet.Dark;
        colorsSkid = GM.pallet.Skid;
        bgColor = GM.pallet.BG;
        Camera.main.backgroundColor = bgColor;

        

        //Generate Texture
        GenerateTexture();

        //Gamemode based differences
        switch (GM.gameMode)
        {
            case GameMode.PVP_Night:
                winningPixelsAmount = totalPixelsFloat * 0.6f;
                break;
            
            case GameMode.PVP_Combat:
                winningPixelsAmount = totalPixelsFloat * 0.4f;
                CombatInitialization();
                break;
            case GameMode.PVP_OceanMist:
                OceanInitialization();
                break;
            case GameMode.PVP_Desert:
                DesertInitialization();
                break;
            case GameMode.Coop_Arcade:
                enemy_body = enemy_body_arcade;
                enemy_transp = enemy_transp_arcade;
                enemy_dark = enemy_dark_arcade;
                break;
            case GameMode.Coop_Torch:
                TorchInitialization();
                break;
            case GameMode.Hanabi:
                HanabiInitialization();
                break;
            case GameMode.Campaign:
                CombatInitialization();
                enemy_body = enemy_body_combat;
                enemy_transp = enemy_transp_combat;
                enemy_dark = enemy_dark_combat;
                //if (GM.campaignMapIndex < 10) //PVP_Combat
                //{
                //    CombatInitialization();
                //    enemy_body = enemy_body_combat;
                //    enemy_transp = enemy_transp_combat;
                //    enemy_dark = enemy_dark_combat;
                //}
                //else if (GM.campaignMapIndex < 20) //Night
                //{
                //    enemy_body = enemy_body_Night;
                //    enemy_transp = enemy_transp_Night;
                //    enemy_dark = enemy_dark_Night;
                //}
                //else if (GM.campaignMapIndex < 30) //PVP_OceanMist
                //{
                //    enemy_body = enemy_body_arcade;
                //    enemy_transp = enemy_transp_arcade;
                //    enemy_dark = enemy_dark_arcade;
                //}
                //else //PVP_Desert
                //{
                //    DesertInitialization();
                //    enemy_body = enemy_body_desert;
                //    enemy_transp = enemy_transp_desert;
                //    enemy_dark = enemy_dark_desert;
                //}
                break;
            default:
                break;
        }
    }

    void CombatInitialization ()
    {
        isCombatMode = true;
    }

    void OceanInitialization()
    {
        isUpdateFFG_Ripple = true;

        rippleFullColor.a = 0.8f;
        ripple5 = rippleFullColor;
        rippleFullColor.a = 0.6f;
        ripple4 = rippleFullColor;
        rippleFullColor.a = 0.4f;
        ripple3 = rippleFullColor;
        rippleFullColor.a = 0.2f;
        ripple2 = rippleFullColor;
        rippleFullColor.a = 0.1f;
        ripple1 = rippleFullColor;

        StartCoroutine(UpdateWaterRipple());
    }
    void DesertInitialization()
    {
        isUpdateFFG_Ripple = true; //Wurm
        isUpdateFFFG_Fog = true;
        isDesertMode = true;

        rippleFullColor.a = 0.8f;
        ripple5 = wormFullColor;
        rippleFullColor.a = 0.6f;
        ripple4 = wormFullColor;
        rippleFullColor.a = 0.4f;
        ripple3 = wormFullColor;
        rippleFullColor.a = 0.2f;
        ripple2 = wormFullColor;
        rippleFullColor.a = 0.1f;
        ripple1 = wormFullColor;
        StartCoroutine(UpdateSandFog());
        StartCoroutine(UpdateWurmRipple());
    }
    void TorchInitialization()
    {
        enemy_body = enemy_body_spooky;
        enemy_transp = enemy_transp_spooky;
        enemy_dark = enemy_dark_spooky;

        isUpdateFFFG_Fog = true;
        StartCoroutine(UpdateSpookyFog());
    }
    void HanabiInitialization()
    {
        isUpdateFFG_Ripple = true;
        isSpaceMode = true;

        //Generate stars
        for (int i = 0; i < 100; i++)
        {
            BG_texture.SetPixel(Random.Range(0, BG_pixelWidth), Random.Range(0, BG_pixelHeight), spaceStarColor);
        }

        StartCoroutine(UpdateHanabiExplosion());
        StartCoroutine(UpdateHanabiTrail());
        StartCoroutine(UpdateHanabiTrailLong());
        StartCoroutine(UpdateConstellation());
    }
    #endregion

    #region Monobehavior Update
    void LateUpdate()
    {
        UpdateUI();
        //Apply texture
        FG_texture.Apply();
        BG_texture.Apply();

        if (isUpdateFFG_Ripple)
        {
            FFG_texture.Apply();
        }
        if (isUpdateFFFG_Fog)
        {
            FFFG_texture.Apply();
        }

    }
    #endregion

    #region Compare Winner
    void UpdateUI ()
    {
        //This method does: 1. update percentage UItext's string 2. Update percentage UIText's color based on who is the winner. 
        int _winnerIndex = -1; //The winner index of this update frame.
        int _highestScore = 0;
        int _comparingScore;

        //Check if the 1st place player has being overtaken. 

        for (int i = 0; i < 4; i++)
        {
            if (GM.gameMode == GameMode.PVP_Combat ||
                GM.gameMode == GameMode.PVP_Night)
            {
                _comparingScore = playerPixelScores[i];

                uiManager.UpdateScore(i, _comparingScore / winningPixelsAmount);//Update percentage UIText's string

                if (i != _winnerIndex && _comparingScore > _highestScore) //Find the winner with most score
                {
                    _highestScore = _comparingScore;
                    _winnerIndex = i;
                }
            }
            else
            {
                _comparingScore = FightSceneManager.kills[i];

                uiManager.UpdateScore(i, _comparingScore);//Update percentage UIText's string

                if (i != _winnerIndex && _comparingScore > _highestScore) //Find the winner with most score
                {
                    _highestScore = _comparingScore;
                    _winnerIndex = i;
                }
            }
        }

        //If winner is different from last frame, then update the color of percentage UIText
        if (_winnerIndex != previousWinnerIndex) 
        {
            previousWinnerIndex = _winnerIndex;
            uiManager.WinnerBeingOverTaken(_winnerIndex);
        }
    }
    #endregion
    
    #region Painting - Public hooks
    public void PaintTankFGPoints(List<Transform> tankPointTransform, int index)
    {
        if (!isDesertMode)
        {
            foreach (Transform t in tankPointTransform)
            {
                IntXY pixelPos = WorldPosToPixelPos_BG(t.position);
                PaintTankFGPixel(pixelPos.x, pixelPos.y, index, true);
            }
        }
        else
        {
            foreach (Transform t in tankPointTransform)
            {
                IntXY pixelPos = WorldPosToPixelPos_BG(t.position);
                PaintDesertTankTransp(pixelPos.x, pixelPos.y, index, true);
            }
        }
    }

    public void PaintBulletPoints(List<IntXY> pixelPos, int index)
    {
        if (!isDesertMode)
        {
            foreach (IntXY p in pixelPos)
            {
                PaintTankFGPixel(p.x, p.y, index, false);
            }
        }
        else
        {
            foreach (IntXY p in pixelPos)
            {
                PaintDesertTankTransp(p.x, p.y, index, true);
            }
        }
    }

    public void PaintFG_ArcadeEnemy_Points(List<Transform> tankPointTransform)
    {
        //Loop through the tankPointsTransform and convent to each pixel positions
        foreach (Transform t in tankPointTransform)
        {
            IntXY pixelPos = WorldPosToPixelPos_BG(t.position);

            PaintFG_ArcadeEnemy_Pixel(pixelPos.x, pixelPos.y);
        }
    }
    #endregion

    #region Painting - Tank and enemy

    public void PaintTankBGPixel(int pixX, int pixY, int newIndex, bool doScreenWarp)
    {        
        if (doScreenWarp)
        {
            WrapBoundOfBoundPixels(ref pixX, ref pixY);
        }
        else
        {
            if (IsOutOfBounds(pixX, pixY))
                return;
        }
        
        //Get existing index
        int curIndex = 0; 
        try
        {
            curIndex = BGPixels[pixX, pixY]; //The playerIndex of the existing pixel (being drawn over)
        }
        catch
        {
            curIndex = GM.emptyIndex;
            UnityEngine.Debug.Log("FGPixels[].length: " + BGPixels.GetLength(0) + ", " + BGPixels.GetLength(1) + ", posX: " + pixX + ", posY: " + pixY + ", index: " + newIndex);
        }

        //If haven't being set by this player. 
        if (curIndex != newIndex) 
        {
            //Debug.Log("cur: " + curIndex + ", newIndex: " + newIndex);
            //Decrease enemy score
            DecreasePlayerScore(curIndex);

            //Increase playerscore
            if (newIndex >= 0 && newIndex < 4)
            {
                BG_texture.SetPixel(pixX, pixY, colorsDark[newIndex]);
                playerPixelScores[newIndex]++;
            }
            else if (newIndex == GM.enemyIndex)
            {
                BG_texture.SetPixel(pixX, pixY, enemy_dark);
            }

            BGPixels[pixX, pixY] = newIndex;
        }
    }

    public void PaintTankFGPixel(int pixX, int pixY, int newIndex, bool doScreenWarp)
    {
        if (doScreenWarp)
        {
            WrapBoundOfBoundPixels(ref pixX, ref pixY);
        }
        else
        {
            if (IsOutOfBounds(pixX, pixY))
                return;
        }

        //Get current index
        int curIndex;
        try
        {
            curIndex = FGPixels[pixX, pixY]; //The playerIndex of the existing pixel (being drawn over)
        }
        catch
        {
            curIndex = GM.emptyIndex;
            UnityEngine.Debug.Log("FGPixels[].length: " + FGPixels.GetLength(0) + ", " + FGPixels.GetLength(1) + ", posX: " + pixX + ", posY: " + pixY + ", index: " + newIndex);
        }

        //If haven't being set by this player. 
        if (curIndex != newIndex)
        {
            //Decrease enemy score
            if (!isCombatMode)
                DecreasePlayerScore(curIndex);

            //Increase playerscore
            if (newIndex >= 0 && newIndex < 4)
            {
                FG_texture.SetPixel(pixX, pixY, colorsTrans[newIndex]);
                if (!isCombatMode)
                    playerPixelScores[newIndex]++;
            }
            else if (newIndex == GM.enemyIndex)
            {
                FG_texture.SetPixel(pixX, pixY, enemy_transp);
            }

            FGPixels[pixX, pixY] = newIndex;
        }
    }

    void PaintDesertTankTransp(int pixX, int pixY, int index, bool doScreenWarp)
    {
        if (doScreenWarp)
        {
            WrapBoundOfBoundPixels(ref pixX, ref pixY);
        }
        else
        {
            if (IsOutOfBounds(pixX, pixY))
                return;
        }

        BG_texture.SetPixel(pixX, pixY, colorsTrans[index]);
        BGPixels[pixX, pixY] = GM.emptyIndex;
    }

    void PaintFG_ArcadeEnemy_Pixel(int pixX, int pixY)
    {
        if (IsOutOfBounds(pixX, pixY))
            return;

        //FG
        FG_texture.SetPixel(pixX, pixY, enemy_transp);
        FGPixels[pixX, pixY] = GM.enemyIndex; //Enemy index

        //BG - completely wipe clean and reset to original BG color.
        BG_texture.SetPixel(pixX, pixY, bgColor);
        BGPixels[pixX, pixY] = GM.emptyIndex; //Empty index
    }

    public void PaintBG_ARCADE_ENEMY(int pixX, int pixY)
    {
        //Clamp to screen
        if (IsOutOfBounds(pixX, pixY))
            return;

        FinalizeBGPixel(pixX, pixY, enemy_dark, GM.enemyIndex);
        FinalizeFGPixel(pixX, pixY, Color.clear, GM.emptyIndex); //FG - wipe clean
    }


    public void PaintSkid(Vector3 worldPos, int newIndex)
    {
        if (isDesertMode)
            return;

        IntXY pixelPos = WorldPosToPixelPos_BG(worldPos.x, worldPos.y);

        int x = pixelPos.x;
        int y = pixelPos.y;

        //Wrap 
        WrapBoundOfBoundPixels(ref x, ref y);
        int curIndex;

        if (isCombatMode)
        {
            curIndex = GetFGPixel(x, y);
            if (curIndex != newIndex)
            {
                DecreasePlayerScore(curIndex);
                IncreasePlayerScore(newIndex);
            }
                
            FinalizeFGPixel(x, y, colorsSkid[newIndex], newIndex);
        }
        else
        {
            curIndex = GetBGPixel(x, y);
            if (curIndex != newIndex)
            {
                DecreasePlayerScore(curIndex);
                IncreasePlayerScore(newIndex);
            }
            FinalizeBGPixel(x, y, colorsSkid[newIndex], newIndex);
        }
    }
    #endregion

    #region Compartmentalize painting logic
    void FinalizeFGPixel (int x, int y, Color color, int index)
    {
        FGPixels[x, y] = index;
        FG_texture.SetPixel(x, y, color);
    }

    void FinalizeBGPixel(int x, int y, Color color, int index)
    {
        BGPixels[x, y] = index;
        BG_texture.SetPixel(x, y, color);
    }

    void IncreasePlayerScore (int i)
    {
        if (i >= 0 && i < 4)
        {
            playerPixelScores[i]++;
        }
    }

    void DecreasePlayerScore(int i)
    {
        if (i >= 0 && i < 4)
        {
            playerPixelScores[i]--;
        }
    }

    int GetFGPixel(int x, int y)
    {
        return FGPixels[x, y];
    }

    int GetBGPixel(int x, int y)
    {
        return BGPixels[x, y];
    }

    bool IsOutOfBounds (int pixX, int pixY)
    {
        if (pixX > BG_pixelWidth - 1 || pixX < 0 ||
            pixY > BG_pixelHeight - 1 || pixY < 0)
        {
            return true;
        }
        return false;
    }

    void WrapBoundOfBoundPixels (ref int x, ref int y)
    {
        if (x >= BG_pixelWidth)
        {
            x -= BG_pixelWidth;
        }
        else if (x < 0)
        {
            x += BG_pixelWidth;
        }

        if (y >= BG_pixelHeight) //Height 108. Arr 0~107
        {
            y -= BG_pixelHeight;
        }
        else if (y < 0)
        {
            y += BG_pixelHeight;
        }
    }
    #endregion

    #region Ocean painting
    Color blendColor1;
    Color blendColor2;

    //Blends 2 tank colors
    public void WaterPaintBGPixel(int posX, int posY, int index)
    {
        //Wrap pixels
        if (posX >= BG_pixelWidth)
        {
            posX = posX - BG_pixelWidth;
        }
        else if (posX < 0)
        {
            posX += BG_pixelWidth;
        }

        if (posY >= BG_pixelHeight) //Height 108. Arr 0~107
        {
            posY -= BG_pixelHeight;
        }
        else if (posY < 0)
        {
            posY += BG_pixelHeight;
        }

        //newPixels.Add(pixelPos);
        int existingIndex = BGPixels[posX, posY];
        if (existingIndex != index)
        {
            if (existingIndex >= 0 && existingIndex < 4)
            {
                FG_texture.SetPixel(posX, posY, (colorsDark[index] + colorsDark[existingIndex])/2f);
                existingIndex = index;
            }
            else
            {
                BGPixels[posX, posY] = index;
                BG_texture.SetPixel(posX, posY, colorsDark[index]);
            }
        }
    }

    public void WaterPaintFGPixel(int posX, int posY, int index)
    {
        //Wrap pixels
        if (posX > BG_pixelWidth - 1 || posX < 0 ||
            posY > BG_pixelHeight - 1 || posY < 0)
        {
            return;
        }

        //newPixels.Add(pixelPos);
        int existingIndex = FGPixels[posX, posY];
        if (existingIndex != index)
        {
            if (existingIndex >= 0 && existingIndex < 4)
            {
                FG_texture.SetPixel(posX, posY, (colorsTrans[index] + colorsTrans[existingIndex]) / 2f);

                existingIndex = index;
            }
            else
            {
                FGPixels[posX, posY] = index;
                FG_texture.SetPixel(posX, posY, colorsTrans[index]);
            }
        }
    }

    public bool GetFGWaterColor (Vector3 worldPos, ref Color color)
    {
        IntXY pixelPos = WorldPosToPixelPos_BG(worldPos.x, worldPos.y);

        int pixX = pixelPos.x;
        int pixY = pixelPos.y;

        if (!IsOutOfBounds(pixX, pixY))
        {
            //int index = FGPixels[p_x, p_y];
            color = FG_texture.GetPixel(pixX, pixX);
            if (color.r > 0.05f && color.g > 0.05f && color.b > 0.05f)
            {
                color.a *= 0.6f;
                return true;
            }
        }
        return false;
    }

    //Paints the temporary and transparent waterripples
    List<IntXY> activeRipplePoints = new List<IntXY>();
    public void AddWaterRipple(Vector3 point, bool paintColor, Color color)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Paint this pixel
        if (paintColor)
        {
            //Only paint, do not update index
            FG_texture.SetPixel(pixX, pixY, color);
        }

        //Add to list of ripples
        if (FFG_RipplePixels[pixX, pixY] < 4)
        {
            activeRipplePoints.Add(pxl);
            FFG_RipplePixels[pixX, pixY] = 6;
        }
    }

    IEnumerator UpdateWaterRipple ()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = activeRipplePoints.Count - 1; i >= 0; i--)
            {
                IntXY point = activeRipplePoints[i];
                int level = --FFG_RipplePixels[point.x, point.y];
                
                //Paint
                if (level <= 0 )
                {
                    FFG_texture.SetPixel(point.x, point.y, Color.clear);
                    activeRipplePoints.RemoveAt(i);
                }
                else if (level >= 5)
                {
                    FFG_texture.SetPixel(point.x, point.y, ripple5);
                }
                else if (level == 4)
                {
                    FFG_texture.SetPixel(point.x, point.y, ripple4);
                }
                else if (level == 3)
                {
                    FFG_texture.SetPixel(point.x, point.y, ripple3);
                }
                else if (level == 2)
                {
                    FFG_texture.SetPixel(point.x, point.y, ripple2);
                }
                else if (level == 1)
                {
                    FFG_texture.SetPixel(point.x, point.y, ripple1);
                }
            }
        }
    }

    //Paints the temporary and transparent waterripples
    public void AddWurmRipple(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples
        if (FFG_RipplePixels[pixX, pixY] < 4)
        {
            activeRipplePoints.Add(pxl);
        }

        FFG_RipplePixels[pixX, pixY] = 21;
    }

    IEnumerator UpdateWurmRipple()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = activeRipplePoints.Count - 1; i >= 0; i--)
            {
                IntXY point = activeRipplePoints[i];
                int level = --FFG_RipplePixels[point.x, point.y];

                //Paint
                if (level <= 0)
                {
                    FFG_texture.SetPixel(point.x, point.y, Color.clear);
                    BG_texture.SetPixel(point.x, point.y, colorsTrans[0]);
                    activeRipplePoints.RemoveAt(i);
                }
                else if (level == 2 || level == 6 || level == 10 || level == 14 || level == 18)
                {
                    FFG_texture.SetPixel(point.x, point.y, wurmColor1);
                }
                else if (level == 4 || level == 8 || level == 12 || level == 16 || level == 20)
                {
                    FFG_texture.SetPixel(point.x, point.y, wurmColor2);
                }
            }
        }
    }

    //Color GetRandomWurmColor ()
    //{
    //    switch(Random.Range(0, 5))
    //    {
    //        case 0:
    //            return wurmColor1;
    //        case 1:
    //            return wurmColor2;
    //        case 2:
    //            return wurmColor3;
    //        case 3:
    //            return wurmColor4;
    //        default:
    //            return wurmColor5;
    //    }
    //}
    #endregion

    #region Spooky Fog FFFG
    //Blends 2 tank colors
    public void Bullet_ClearSpookyFogSml(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        foreach (var p in CircularOffset.Circle3)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            AddSpookyFogPixel(x, y);
        }
    }

    public void Bullet_ClearSpookyFogMid(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        foreach (var p in CircularOffset.Circle5) //circle 5
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            AddSpookyFogPixel(x, y);
        }
    }

    public void Bullet_ClearSpookyFogLarge(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        foreach (var p in CircularOffset.Circle10)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            AddSpookyFogPixel(x, y);
        }
    }

    public void Bullet_ClearSpookyFogTorch(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        Color c;
        if (index == 0)
        {
            c = torch_1;
        }
        else if (index == 1)
        {
            c = torch_2;
        }
        else if (index == 2)
        {
            c = torch_3;
        }
        else
        {
            c = torch_4;
        }

        foreach (var p in CircularOffset.Circle9)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            FFFG_FogPixels[x, y] = -10;
            FFFG_texture.SetPixel(x, y, c);
        }
    }

    public void Spooky_TurnOffTorch(Vector3 point)
    {
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        foreach (var p in CircularOffset.Circle9)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            int value = FFFG_FogPixels[x, y];
            if (value < 0)
            {
                activeFogPoints.Add(new IntXY(x, y));
                FFFG_FogPixels[x, y] = 15;
                FFFG_texture.SetPixel(x, y, Color.clear);
            }

        }
    }

    public void Tank_ClearSpookyFog(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        foreach (var p in CircularOffset.Circle10)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            WrapBoundOfBoundPixels(ref x, ref y);
            AddSpookyFogPixel(x, y);
        }
    }

    void AddSpookyFogPixel(int pixX, int pixY) // only add screen wrap or clamp valided pixels
    {
        int p = FFFG_FogPixels[pixX, pixY];
        if (p >= 0) //0+ are reserved for fog, <0 are reserved for bokeh tortch
        {
            if (p == 0)
                activeFogPoints.Add(new IntXY(pixX, pixY));

            FFFG_FogPixels[pixX, pixY] = 15;
            FFFG_texture.SetPixel(pixX, pixY, Color.clear);
        }
    }

    int _x;
    int _y;
    int _level;
    IEnumerator UpdateSpookyFog()
    {
        while (true)
        {
            //Debug.Log("...UpdateSpookyFog()");
            //Debug.Log("" + activeFogPoints.Count);
            yield return new WaitForSeconds(0.1f);
            for (int i = activeFogPoints.Count - 1; i >= 0; i--)
            {
                IntXY point = activeFogPoints[i];
                _x = point.x;
                _y = point.y;

                _level = --FFFG_FogPixels[_x, _y];
                //FFFG_FogPixels[_x, _y] = _level;

                //Paint
                if (_level < 0) //Already being overridden. Just delete
                {
                    activeFogPoints.RemoveAt(i);
                }
                else if (_level == 0)
                {
                    FFFG_texture.SetPixel(point.x, point.y, SpookyFogColor);
                    activeFogPoints.RemoveAt(i);
                }
                else
                {
                    if (_level == 9)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog9);
                    }
                    else if (_level == 8)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog8);
                    }
                    else if (_level == 7)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog7);
                    }
                    else if (_level == 6)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog6);
                    }
                    else if (_level == 5)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog5);
                    }
                    else if (_level == 4)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog4);
                    }
                    else if (_level == 3)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog3);
                    }
                    else if (_level == 2)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog2);
                    }
                    else if (_level == 1)
                    {
                        FFFG_texture.SetPixel(_x, _y, fog1);
                    }
                }
            }
        }
    }
    #endregion
    
    #region Desert
    //Storm
    //Use the fog infrastructure for the sandstorm
    List<IntXY> activeFogPoints = new List<IntXY>();
    public void AddSandstormPoint(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples
        if (FFFG_FogPixels[pixX, pixY] <= 0)
        {
            activeFogPoints.Add(pxl);
        }
        FFFG_FogPixels[pixX, pixY] = 12;
    }

    IEnumerator UpdateSandFog()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = activeFogPoints.Count - 1; i >= 0; i--)
            {
                IntXY point = activeFogPoints[i];
                _x = point.x;
                _y = point.y;
                int level = --FFFG_FogPixels[_x, _y];

                //Paint
                if (level <= 0)
                {
                    FFFG_texture.SetPixel(_x, _y, Color.clear);
                    activeFogPoints.RemoveAt(i);
                }
                else
                {
                    FFFG_texture.SetPixel(_x, _y, sandColor1);
                }
            }
        }
    }

    Color GetRandomSandcolor()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return sandColor1;
            case 1:
                return sandColor2;
            case 2:
            default:
                return sandColor3;
        }
    }

    //Explosions
    public void DesertGrenadeExplosion(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        foreach (var p in CircularOffset.Circle9)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }

            BG_texture.SetPixel(x, y, bgColor);
            BGPixels[pixX, pixY] = GM.emptyIndex;
        }
    }

    public void DesertStandardExplosion(Vector3 point)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;
        if (IsOutOfBounds(pixX, pixY))
        {
            return;
        }

        //Hollow center
        foreach (var p in CircularOffset.Circle9)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }

            BG_texture.SetPixel(x, y, bgColor);
            BGPixels[pixX, pixY] = GM.emptyIndex;
        }

        //Rim
        foreach (var p in CircularOffset.Ring7)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }

            BG_texture.SetPixel(x, y, colorsTrans[0]);
            BGPixels[pixX, pixY] = GM.emptyIndex;
        }
    }
    #endregion
    
    #region Hanabi explosion
    //We want to color the space dusts with different colors
    List<IntXY>[] activeHanabiExplosion = { new List<IntXY>(), new List<IntXY>(), new List<IntXY>(), new List<IntXY>() };
    public void AddHanabiExplosion(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples if current point is inactive
        int p = FFG_RipplePixels[pixX, pixY];
        if (p <= 0)
        {
            activeHanabiExplosion[index].Add(pxl);
            FFG_RipplePixels[pixX, pixY] = 12;
        }
        else if (p < 12)
        {
            FFG_RipplePixels[pixX, pixY] = 12; //Lingering duration
        }
    }

    public void AddHanabiHeart(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        IntXY[] icon;
        
        switch (Random.Range(0, 4))
        {
            case 0:
                icon = CircularOffset.Heart;
                break;
            case 1:
                icon = CircularOffset.Cupcake;
                break;
            case 2:
                icon = CircularOffset.Clover;
                break;
            default:
                icon = CircularOffset.Trophy;
                break;
        }

        foreach (var p in icon)
        {
            int x = pixX + p.x;
            int y = pixY + p.y;
            if (IsOutOfBounds(x, y))
            {
                continue;
            }
            if (FFG_RipplePixels[x, y] <= 0)
            {
                activeHanabiExplosion[index].Add(new IntXY(x, y));
            }
            FFG_RipplePixels[x, y] = 29; //Lingering duration
        }
    }

    IEnumerator UpdateHanabiExplosion()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int group = 0; group < 4; group++)
            {
                for (int i = activeHanabiExplosion[group].Count - 1; i >= 0; i--)
                {
                    IntXY point = activeHanabiExplosion[group][i];
                    _x = point.x;
                    _y = point.y;
                    int level = --FFG_RipplePixels[_x, _y];

                    //Paint
                    if (level <= 0)
                    {
                        FFG_texture.SetPixel(_x, _y, Color.clear);
                        //BG_texture.SetPixel(_x, _y, GetHanabiExplodeFade(group));
                        activeHanabiExplosion[group].RemoveAt(i);
                    }
                    else
                    {
                        FFG_texture.SetPixel(_x, _y, GetRandomHanabiExplodeColor(group));
                    }
                }
            }
        }
    }

    Color GetRandomHanabiExplodeColor(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return hanabiExplode0[Random.Range(0, hanabiExplode0.Length)];
        }
        else if (playerIndex == 1)
        {
            return hanabiExplode1[Random.Range(0, hanabiExplode1.Length)];
        }
        else if (playerIndex == 2)
        {
            return hanabiExplode2[Random.Range(0, hanabiExplode2.Length)];
        }
        else
        {
            return hanabiExplode3[Random.Range(0, hanabiExplode3.Length)];
        }
    }

    Color GetHanabiExplodeFade(int playerIndex)
    {
        Color c;
        if (playerIndex == 0)
        {
            c = hanabiExplode0[0];
        }
        else if (playerIndex == 1)
        {
            c = hanabiExplode1[0];
        }
        else if (playerIndex == 2)
        {
            c = hanabiExplode2[0];
        }
        else
        {
            c = hanabiExplode3[0];
        }
        return (c + bgColor) / 2f;
    }
    #endregion

    #region Hanabi Trail Short
    //We want to color the space dusts with different colors
    List<IntXY> activeHanabiTrail1 = new List<IntXY>();
    List<IntXY> activeHanabiTrail2 = new List<IntXY>();
    List<IntXY> activeHanabiTrail3 = new List<IntXY>();
    List<IntXY> activeHanabiTrail4 = new List<IntXY>();
    public void AddHanabiTrailShort(Vector3 point, int index)
    {

        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples if current point is inactive
        if (FGPixels[pixX, pixY] <= 0)
        {
            GetHanabiTrailShortPoints(index).Add(pxl);
            //FG_texture.SetPixel(pixX, pixY, GetHanabiTrailColor(index)[0]);
        }
        FGPixels[pixX, pixY] = 4;
    }

    Color[] GetHanabiTrailColor (int index)
    {
        
        if (index == 0)
            return hanabiTrail0;
        else if (index == 1)
            return hanabiTrail1;
        else if (index == 2)
            return hanabiTrail2;
        else
            return hanabiTrail3;
    }

    List<IntXY> GetHanabiTrailShortPoints(int index)
    {
        if (index == 0)
            return activeHanabiTrail1;
        else if (index == 1)
            return activeHanabiTrail2;
        else if (index == 2)
            return activeHanabiTrail3;
        else
            return activeHanabiTrail4;
    }

    IEnumerator UpdateHanabiTrail()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.08f);
            for(int index = 0; index < 4; index++)
            {
                List<IntXY> points = GetHanabiTrailShortPoints(index);
                for (int i = points.Count - 1; i >= 0; i--)
                {
                    IntXY point = points[i];
                    _x = point.x;
                    _y = point.y;
                    int level = --FGPixels[_x, _y];

                    if (level == 0)
                    {
                        FG_texture.SetPixel(_x, _y, Color.clear);
                        points.RemoveAt(i);
                    }
                    else if (level < 3)
                    {
                        FG_texture.SetPixel(point.x, point.y, GetHanabiTrailColor(index)[level]);
                    }
                    else
                    {
                        FG_texture.SetPixel(point.x, point.y, GetHanabiTrailColor(index)[0]);
                    }
                }
            }
        }
    }
    #endregion

    #region Hanabi Trail LONG
    //We want to color the space dusts with different colors
    List<IntXY> activeHanabiTrailLong1 = new List<IntXY>();
    List<IntXY> activeHanabiTrailLong2 = new List<IntXY>();
    List<IntXY> activeHanabiTrailLong3 = new List<IntXY>();
    List<IntXY> activeHanabiTrailLong4 = new List<IntXY>();
    public void AddHanabiTrailLong(Vector3 point, int index)
    {
        //Convert world pos to pixel point
        IntXY pxl = WorldPosToPixelPos_BG(point.x, point.y);
        int pixX = pxl.x;
        int pixY = pxl.y;

        if (IsOutOfBounds(pixX, pixY))
            return;

        //Add to list of ripples if current point is inactive
        if (FGPixels[pixX, pixY] <= 0)
        {
            GetHanabiTrailLongPoints(index).Add(pxl);
            FG_texture.SetPixel(pixX, pixY, GetHanabiTrailColor(index)[0]);
        }
        FGPixels[pixX, pixY] = 20;
    }

    List<IntXY> GetHanabiTrailLongPoints(int index)
    {
        if (index == 0)
        {
            return activeHanabiTrailLong1;
        }
        else if (index == 1)
        {
            return activeHanabiTrailLong2;
        }
        else if (index == 2)
        {
            return activeHanabiTrailLong3;
        }
        else
        {
            return activeHanabiTrailLong4;
        }
    }

    IEnumerator UpdateHanabiTrailLong()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.08f);
            for (int index = 0; index < 4; index++)
            {
                List<IntXY> points = GetHanabiTrailLongPoints(index);
                for (int i = points.Count - 1; i >= 0; i--)
                {
                    IntXY point = points[i];
                    _x = point.x;
                    _y = point.y;
                    int level = --FGPixels[_x, _y];

                    if (level == 0)
                    {
                        FG_texture.SetPixel(_x, _y, Color.clear);
                        points.RemoveAt(i);
                    }
                    else if (level < 7)
                    {
                        FG_texture.SetPixel(point.x, point.y, GetHanabiTrailColor(index)[1]);
                    }
                    else if (level < 14)
                    {
                        FG_texture.SetPixel(point.x, point.y, GetHanabiTrailColor(index)[2]);
                    }
                    else
                    {
                        FG_texture.SetPixel(point.x, point.y, GetHanabiTrailColor(index)[0]);
                    }
                }
            }
        }
    }
    #endregion

    #region Space constellationLine
    const float starLineInterval = 0.3f; //Interval distance
    public void DrawConstelationLine(Vector2 startPos, Vector2 endPos)
    {
        StartCoroutine(AddConstelationLine(startPos, endPos));
    }

    IEnumerator AddConstelationLine(Vector2 startPos, Vector2 endPos)
    {
        //Find intervals and draw dots with equal spacing
        Vector2 dir = endPos - startPos;
        Vector2 intervalDir = dir.normalized * starLineInterval;

        float totalDist = dir.magnitude - starLineInterval - starLineInterval;
        float drawnDist = 0;

        Vector2 newPos = startPos + intervalDir;
        while (drawnDist < totalDist)
        {
            newPos += intervalDir;
            IntXY pixel = WorldPosToPixelPos_BG(newPos.x, newPos.y);
            //Debug.DrawLine(Vector3.zero, newPos, Color.yellow, 10f);
            int _x = pixel.x;
            int _y = pixel.y;

            if (IsOutOfBounds(_x, _y))
            {
                Debug.Log("THIS SHOULD NEVER HAPPEN");
                continue;
            }

            activeConstellation.Add(new IntXY(_x, _y));
            BGPixels[_x, _y] = 50;

            drawnDist += starLineInterval;
            yield return null;
        }
    }

    List<IntXY> activeConstellation = new List<IntXY>();
    IEnumerator UpdateConstellation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = activeConstellation.Count - 1; i >= 0; i--)
            {
                IntXY point = activeConstellation[i];
                _x = point.x;
                _y = point.y;
                int level = --BGPixels[_x, _y];

                if (IsOutOfBounds(_x, _y))
                    yield break;

                //Paint
                if (level <= 0)
                {
                    BG_texture.SetPixel(_x, _y, bgColor);
                    //BG_texture.SetPixel(_x, _y, GetHanabiExplodeFade(group));
                    activeConstellation.RemoveAt(i);
                }
                else if (level < 10)
                {
                    BG_texture.SetPixel(_x, _y, Color.Lerp(bgColor, constellationColor, level * 0.1f));
                }
                else if (level >= 40 && level < 50)
                {
                    BG_texture.SetPixel(_x, _y, Color.Lerp(constellationColor, bgColor, (level - 40) * 0.1f));
                }
            }
        }
    }
    #endregion

    #region Paint Splatter
    public void PaintSplatterFlower(Vector2 worldPos, Vector2 vel, int index)
    {
        if (isDesertMode)
        {
            DesertStandardExplosion(worldPos);
        }
        else
        {

            for (var i = 0; i < Random.Range(7, 10); i++) //Random splatters
            {
                SplatterStrand s = new SplatterStrand(worldPos, vel, index, isCombatMode);
                StartCoroutine(s.PaintStrand());
            }           
        }
    }
    #endregion

    #region Texture generate
    void GenerateTexture()
    {
        //BG
        Material material;
        BG_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.RGB24, false);

        material = BG.GetComponent<Renderer>().material;
        material.mainTexture = BG_texture;
        //material.shader = shaderTexture;
        //material.shader = Shader.Find("Unlit/Texture");

        float w = BG_pixelWidth;
        float h = BG_pixelHeight;

        for (int i = 0; i < BG_pixelWidth; i++)
        {
            for (int j = 0; j < BG_pixelHeight; j++)
            {
                BG_texture.SetPixel(i, j, bgColor);
            }
        }

        BG_texture.filterMode = FilterMode.Point;
        BG_texture.Apply();

        //FG
        Material material2;
        FG_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.ARGB32, false); //ARGB32! for transparency

        material2 = FG.GetComponent<Renderer>().material;
        material2.mainTexture = FG_texture;
        //material2.shader = shaderTransparent;
        //material2.shader = Shader.Find("Unlit/Transparent");

        //Color transBG = bgColor;
        //transBG.a = 0.5f;
        for (int i = 0; i < BG_pixelWidth; i++)
        {
            for (int j = 0; j < BG_pixelHeight; j++)
            {
                FG_texture.SetPixel(i, j, Color.clear);
                //FG_texture.SetPixel(i, j, transBG); delete
            }
        }

        FG_texture.filterMode = FilterMode.Point;
        FG_texture.Apply();

        //FFG - ripples / worm
        if (GM.gameMode == GameMode.PVP_OceanMist || GM.gameMode == GameMode.PVP_Desert || GM.gameMode == GameMode.Hanabi)
        {
            Material material3;
            FFG_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.ARGB32, false); //ARGB32! for transparency

            material3 = FFG_Ripples.GetComponent<Renderer>().material;
            material3.mainTexture = FFG_texture;

            for (int i = 0; i < BG_pixelWidth; i++)
            {
                for (int j = 0; j < BG_pixelHeight; j++)
                {
                    FFG_texture.SetPixel(i, j, Color.clear);
                }
            }

            FFG_texture.filterMode = FilterMode.Point;
            FFG_texture.Apply();
        }
        else
        {
            FFG_Ripples.gameObject.SetActive(false);
        }

        //FFFG - Sandstorm / spooky fog
        if (GM.gameMode == GameMode.PVP_Desert)
        {
            FFFG_Sandstorm.gameObject.SetActive(true);
            Material material4;
            FFFG_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.ARGB32, false); //ARGB32! for transparency

            material4 = FFFG_Sandstorm.GetComponent<Renderer>().material;
            material4.mainTexture = FFFG_texture;

            for (int i = 0; i < BG_pixelWidth; i++)
            {
                for (int j = 0; j < BG_pixelHeight; j++)
                {
                    FFFG_texture.SetPixel(i, j, Color.clear);
                }
            }

            FFFG_texture.filterMode = FilterMode.Point;
            FFFG_texture.Apply();
        }
        else if (GM.gameMode == GameMode.Coop_Torch)
        {
            FFFG_Sandstorm.gameObject.SetActive(true);
            Material material4;
            FFFG_texture = new Texture2D(BG_pixelWidth, BG_pixelHeight, TextureFormat.ARGB32, false); //ARGB32! for transparency

            material4 = FFFG_Sandstorm.GetComponent<Renderer>().material;
            material4.mainTexture = FFFG_texture;

            for (int i = 0; i < BG_pixelWidth; i++)
            {
                for (int j = 0; j < BG_pixelHeight; j++)
                {
                    FFFG_texture.SetPixel(i, j, SpookyFogColor);
                }
            }

            FFFG_texture.filterMode = FilterMode.Point;
            FFFG_texture.Apply();
        }
        else
        {
            FFFG_Sandstorm.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Conversions 
    public static IntXY WorldPosToPixelPos_BG(Vector2 p)
    {
        //UnityEngine.Debug.Log(p + ", " + WorldPosXToPixelX_BG(p.x));
        //UnityEngine.Debug.Break();
        return new IntXY(WorldPosXToPixelX_BG(p.x), WorldPosYToPixelY_BG(p.y));
        //return WorldPosToPixelPos_BG(p.x, p.y);
    }

    public static IntXY WorldPosToPixelPos_BG(float x, float y)
    {
        return new IntXY(WorldPosXToPixelX_BG(x), WorldPosYToPixelY_BG(y));
    }

    public static int WorldPosXToPixelX_BG(float x)
    {
        //UnityEngine.Debug.Log(x + ", BG_Bound_minX: " + BG_Bound_minX + ", BG_Bound_sizeX: " + BG_Bound_sizeX);
        //UnityEngine.Debug.Break();
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
    #endregion
}

/*
     public void PaintSplatter(Vector2 pPoint, Vector2 vel, int index)
    {
        // choose a random color for the paint splatter
        //Color color = GM.instance.tankColors[index];

        for (var i = 0; i < Random.Range(6, 9); i++) //Random 6 to 9 splatters
        {
            splatter.Add(new SplatterStrand(pPoint, vel, Color.red));
        }
    }

    void SplatterUpdate ()
    {
        for (int i = splatter.Count - 1; i >= 0; i--)
        {
            if (!splatter[i].DoUpdate())
            {
                splatter.RemoveAt(i);
            }
        }
    }
     */

