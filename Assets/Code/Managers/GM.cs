using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    PVP_Combat,
    PVP_Night,
    PVP_OceanMist,
    PVP_Desert,
    Coop_Arcade,
    Coop_Torch, 
    Hanabi,
    Campaign
}

[System.Serializable]
public class ColorPallet
{
    public Color BG;

    public Color[] Tank;
    public Color[] Trans;
    public Color[] Dark;
    public Color[] Skid;
}

//This first version test allows you to switch active canvases
public class GM : MonoBehaviour 
{
    #region Fields
    public static GM instance;
    public static ColorPallet pallet;

    public const int enemyIndex = -2;
    public const int emptyIndex = -1;

    [Header("Colors")]
    public ColorPallet pallet_0_oil;
    public ColorPallet pallet_1_brown;
    public ColorPallet pallet_2_night;
    public ColorPallet pallet_3_ocean;
    public ColorPallet pallet_4_arcade;
    public ColorPallet pallet_5;
    public ColorPallet pallet_6;
    public ColorPallet pallet_7;
    public ColorPallet pallet_8;
    public ColorPallet pallet_9;
    public ColorPallet pallet_10;
    public ColorPallet pallet_11;
    public ColorPallet pallet_12;
    public ColorPallet pallet_13;
    public ColorPallet pallet_14;
    public ColorPallet pallet_15;
    public ColorPallet pallet_16;

    //Persistent data
    [HideInInspector] public PlayerTypes[] playerType; //Player, AI, or none]
    
    [HideInInspector] public TankModelNames[] tankModelNames;
    public static GameMode gameMode = GameMode.PVP_OceanMist;
    public static int campaignMapIndex = 3;
    public static int combatMapIndex = 0;
    public static int nightMapIndex = 0;
    public static int spookyMapIndex = 0;
    public static int desertMapIndex = 0;
    public static int lastLoadedLevel = -1;
    public static int unlockedCampaignIndex;

    //Save data
    string saveLocation;
    MySaveClass saveFile = new MySaveClass(); //Create the container class

    //Layers
    [HideInInspector] public static int layerPlayer;
    [HideInInspector] public static int layerObstacle;
    [HideInInspector] public static int layerProp;
    [HideInInspector] public static int layerEnemy;
    [HideInInspector] public static int layerBullet;
    [HideInInspector] public static int layerDeadTank;
    #endregion

    #region MonoBehaviour  
    void Awake()
    {
        //Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        Screen.SetResolution(1920, 1080, true);
        //Screen.fullScreen = true;

        layerPlayer     = LayerMask.NameToLayer("Players");
        layerObstacle   = LayerMask.NameToLayer("Obstacles");
        layerProp       = LayerMask.NameToLayer("Props");
        layerEnemy      = LayerMask.NameToLayer("Enemies");
        layerBullet     = LayerMask.NameToLayer("Bullets");
        layerDeadTank   = LayerMask.NameToLayer("DeadTank");

        //Singleton
        //SingletonCheck();
        ResetPersistentData();
    }

    void SingletonCheck ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
                Debug.Log("Destroyed duplicate GM");
            }
        }
    }

    void Start () 
	{
        LevelLoaded(); //Have to make this call because OnLevelWasLoaded() is not called when the game first starts.
        saveLocation = Application.persistentDataPath + "/gamesave.save";
    }

    private void Update()
    {
        //While cursor hidden, check if mouse moves
        //While cursor revealed 

        //See if mouse moved
        Vector3 curPos = Input.mousePosition;
        if (previousMousePos != curPos)
        {
            mouseMoved = true;
            previousMousePos = curPos;
        }
        else
        {
            mouseMoved = false;
        }

        //If mouse was hidden and mouse moved, then reveal mouse.
        if (cursorHidden && mouseMoved)
        {
            Cursor.visible = true;
            cursorTimer = 1f;
            cursorHidden = false;
        }
        //If mouse was showned and mouse didn't move, then countdown to hide.
        else if (!cursorHidden && !mouseMoved)
        {
            cursorTimer -= Time.deltaTime;
            if (cursorTimer < 0f)
            {
                Cursor.visible = false;
                cursorHidden = true;
            }
        }        

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdatePallete(gameMode);
        }
    }

    bool mouseMoved = false;
    bool cursorHidden = false;
    Vector3 previousMousePos;
    float cursorTimer = 1f;
    #endregion

    #region LevelLoaded
    void OnLevelWasLoaded(int level)  //Called by MonoBehaviour when a new level is loaded.
    {
        LevelLoaded();
    }

    void LevelLoaded()
    {
        SingletonCheck();

        //Update save file
        unlockedCampaignIndex = PlayerPrefs.GetInt("CampaignProgress", 0);
        //Debug.Log("campaignProgress " + unlockedCampaignIndex);

        int loadedLevel = SceneManager.GetActiveScene().buildIndex;
        
        //MENU LEVEL
        if (loadedLevel != lastLoadedLevel)
        {
            Debug.Log("===== Loaded level: " + loadedLevel + ". Last loaded: " + lastLoadedLevel + ". combatMapIndex: " + combatMapIndex + ". NightMapIndex: " + nightMapIndex);

            if (loadedLevel == 0) //Logo
            {
                pallet = pallet_1_brown;
                ResetPersistentData();
            }
            else if (loadedLevel == 1) //Fight scene
            {
                UpdatePallete(gameMode);
                //Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            }
            else if (loadedLevel == 2) //
            {

            }

            lastLoadedLevel = loadedLevel; //Only allow these to be called once to prevent mapIndex increasd too much.
        }
    }
    #endregion

    public static void CampaignWon ()
    {
        if (unlockedCampaignIndex <= campaignMapIndex )
        {
            PlayerPrefs.SetInt("CampaignProgress", campaignMapIndex + 1);
        }
        
    }

    #region Persistent Data 
    void ResetPersistentData ()
    {
        playerType = new PlayerTypes[4] 
        {
            PlayerTypes.AI,
            PlayerTypes.AI,
            PlayerTypes.INACTIVE,
            PlayerTypes.INACTIVE};
        
        tankModelNames = new TankModelNames[4] 
        {
            TankModelNames.RIFLE,
            TankModelNames.SHOTGUN,
            TankModelNames.GRENADE,
            TankModelNames.SEEKER };
    }

    public void UpdatePallete (GameMode newGameMode)
    {
        switch (newGameMode)
        {
            case GameMode.PVP_Combat:
                pallet = pallet_10;
                break;
            case GameMode.PVP_Night:
                pallet = pallet_2_night;
                break;
            case GameMode.PVP_OceanMist:
                pallet = pallet_3_ocean;
                break;
            case GameMode.PVP_Desert:
                pallet = pallet_6;
                break;
            case GameMode.Coop_Arcade:
                pallet = pallet_8;
                break;
            case GameMode.Coop_Torch:
                pallet = pallet_12;
                break;
            case GameMode.Hanabi:
                pallet = pallet_7;
                break;
            default:
            case GameMode.Campaign:
                pallet = pallet_10;
                //if (campaignMapIndex < 10) //PVP_Combat
                //{
                //    pallet = pallet_10;
                //}
                //else if (campaignMapIndex < 20) //Night
                //{
                //    pallet = pallet_2_night;
                //}
                //else if (campaignMapIndex < 30) //Coop_Arcade
                //{
                //    pallet = pallet_8;
                //}
                //else //PVP_Desert
                //{
                //    pallet = pallet_6;
                //}
                break;
        }
    }
    #endregion

    #region Save Data - Highscore
    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter(); //Creates formatter
        FileStream file = File.Create(saveLocation); //Creates a file
        //FileStream file2 = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
        bf.Serialize(file, saveFile); //Write the class to the file
        file.Close(); //Close the file
    }

    void AttemptToLoad()
    {
        if (File.Exists(saveLocation))
        {
            BinaryFormatter bf = new BinaryFormatter(); //Creates formatter
            FileStream file = File.Open(saveLocation, FileMode.Open);
            saveFile = (MySaveClass)bf.Deserialize(file); //Have to cast it or it's a generic file
            file.Close();

            Debug.Log("Loaded. myInt = " + saveFile.highScore);
        }
        else
        {
            Debug.Log("Cannot find save file located at " + saveLocation + ". Creating new SaveFile.");
        }
    }
    #endregion
}

public enum PlayerTypes
{
    INACTIVE,
    REAL_PERSON,
    AI
}

[System.Serializable] //Tells unity this class can be turned into bytes and saved.
public class MySaveClass
{
    public int highScore = 1;
}