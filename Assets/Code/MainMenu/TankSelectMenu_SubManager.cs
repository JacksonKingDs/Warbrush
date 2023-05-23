using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankSelectMenu_SubManager : MonoBehaviour 
{
    public static TankSelectMenu_SubManager instance;

    #region Fields
    public Color color_text_READY;
    public Color color_tankImg_joining;
    public Color color_text_TankName;

    public Color[] tankColors;

    [Space(10)]
    [Header("IMAGE REF")]
    public Sprite tank_rifle;
    public Sprite tank_shotgun;
    public Sprite tank_grenade;
    public Sprite tank_bouncer;
    public Sprite tank_seeker;
    public Sprite noTank_cross;

    [Space(10)]
    [Header("TANK SELECT")]
    public Text[] TankPlayerSlotText;
    public Text[] PressToJoinText;
    public Text[] PressToChangeText;
    public Image[] tankImage;
    public Text[] ReadyTexts;
    public GameObject selectAiInfo;

    [Space(10)]
    [Header("CONFIRM SELECTION")]
    public GameObject confirmSelect;

    ActorMenuState[] playersState = new ActorMenuState[4];
    float canJoin_CDDuration = 0.5f;
    bool confirmDisplayed = false;
    bool allPlayersInactive = true;
    //RectTransform[] tankNamesStartingPositions = new RectTransform[4];


    GM gm;
    ScMenu_UIManager menuManager;
    InputManager inputM;
    AudioManager audioM;
    #endregion

    #region Initialization    
    void Awake () 
	{
        instance = this;

        //for (int i = 0; i < 4; i++)
        //{
        //    tankNamesStartingPositions[i] = TankPlayerSlotText[i].rectTransform;
        //}
    }

    void Start()
    {
        menuManager = ScMenu_UIManager.instance;
        inputM = InputManager.Instance;
        gm = GM.instance;
        audioM = AudioManager.instance;

        ResetAllChar();
    }

    public void ResetAllChar() //Initialize by setting all players to inactive and hiding objects
    {
        if (menuManager.isCampaignMode)
        {
            selectAiInfo.SetActive(false);
        }
        else
        {
            selectAiInfo.SetActive(true);
        }

        for (int i = 0; i < 4; i++)
        {
            SetPlayerToInactive(i);
        }
    }
    #endregion

    #region Input Update
    public void OnUpdate ()
    {
        //PLAYER JOIN INPUT
        for (int i = 0; i < 4; i++)
        {
            //Player Join, by pressing Start or A-button
            if (inputM.playerInputs[i].StartDown || inputM.playerInputs[i].A_BtnDown)
            {
                PlayerPressedStart(i);
                
            }
            //Player Pressed Back, by pressing B-button
            else if (inputM.playerInputs[i].B_BtnDown)
            {
                if (confirmDisplayed) //Quick confirm displayed
                {
                    confirmDisplayed = false;
                    confirmSelect.SetActive(false);
                }
                PressedBack(i);
            }

            //Swap tank models, by pressing Left or Right direction key
            if (inputM.leftBtn_OnceDown[i])
            {
                audioM.Spawn_UI_click_Soft();

                if (playersState[i] == ActorMenuState.INACTIVE)
                {
                    PlayerPressedStart(i);
                }
                else if (playersState[i] == ActorMenuState.AI)
                {
                    TankSelection(i, false);
                }
                else if (playersState[i] == ActorMenuState.JOINED)
                {
                    TankSelection(i, false);
                    confirmSelect.SetActive(false);
                    confirmDisplayed = false;
                }
                else if (playersState[i] == ActorMenuState.READY)
                {
                    PressedBack(i);
                    TankSelection(i, false);
                }
            }
            else if (inputM.rightBtn_OnceDown[i])
            {
                audioM.Spawn_UI_click_Soft();

                if (playersState[i] == ActorMenuState.INACTIVE)
                {
                    PlayerPressedStart(i);
                }
                else if (playersState[i] == ActorMenuState.AI)
                {
                    TankSelection(i, true);
                }
                else if (playersState[i] == ActorMenuState.JOINED)
                {
                    confirmSelect.SetActive(false);
                    confirmDisplayed = false;
                    TankSelection(i, true);
                }
                else if (playersState[i] == ActorMenuState.READY)
                {
                    PressedBack(i);
                    TankSelection(i, true);
                }
            }
        }

        //TOGGLE AI
        if (!menuManager.isCampaignMode)
        {
            if (inputM.AnyLB_Down || Input.GetKeyDown(KeyCode.Alpha1))
            {
                audioM.Spawn_UI_click_verysoft();
                PressedToggleAI(0);
            }
            if (inputM.AnyRB_Down || Input.GetKeyDown(KeyCode.Alpha2))
            {
                audioM.Spawn_UI_click_verysoft();
                PressedToggleAI(1);
            }
            if (inputM.AnyLT_Down || Input.GetKeyDown(KeyCode.Alpha3))
            {
                audioM.Spawn_UI_click_verysoft();
                PressedToggleAI(2);
            }
            if (inputM.AnyRT_Down || Input.GetKeyDown(KeyCode.Alpha4))
            {
                audioM.Spawn_UI_click_verysoft();
                PressedToggleAI(3);
            }
        }
    }
    #endregion

    #region TankSelection
    void TankSelection(int i, bool _chooseNext)
    {
        //Get current selection from GM
        if (playersState[i] == ActorMenuState.JOINED)
        {
            gm.tankModelNames[i] = GetNextModel(i, _chooseNext);
            ReadyTexts[i].text = GetModelName(i);
            UpdateTankImage(i);
        }
        else if (playersState[i] == ActorMenuState.AI)
        {
            gm.tankModelNames[i] = GetNextModel(i, _chooseNext);
            UpdateTankImage(i);
        }
    }

    TankModelNames GetNextModel (int i, bool getNext)
    {
        switch (gm.tankModelNames[i])
        {
            case TankModelNames.RIFLE:                
                return getNext ? TankModelNames.SHOTGUN : TankModelNames.SEEKER;
            case TankModelNames.SHOTGUN:
                return getNext ? TankModelNames.GRENADE : TankModelNames.RIFLE;
            case TankModelNames.GRENADE:
                return getNext ? TankModelNames.BOUNCER : TankModelNames.SHOTGUN;
            case TankModelNames.BOUNCER:
                return getNext ? TankModelNames.SEEKER : TankModelNames.GRENADE;
            default:
            case TankModelNames.SEEKER:
                return getNext ? TankModelNames.RIFLE : TankModelNames.BOUNCER;
        }
    }

    void UpdateTankImage (int i)
    {
        switch (gm.tankModelNames[i])
        {
            case TankModelNames.RIFLE:
                tankImage[i].sprite = tank_rifle;
                break;
            case TankModelNames.SHOTGUN:
                tankImage[i].sprite = tank_shotgun;
                break;
            case TankModelNames.GRENADE:
                tankImage[i].sprite = tank_grenade;
                break;
            case TankModelNames.BOUNCER:
                tankImage[i].sprite = tank_bouncer;
                break;
            case TankModelNames.SEEKER:
            default:
                tankImage[i].sprite = tank_seeker;
                break;
        }
    }
    #endregion
    
    #region Start / Back / AI
    void PlayerPressedStart(int i)
    {
        allPlayersInactive = false;
        switch (playersState[i])
        {
            case ActorMenuState.INACTIVE:
            case ActorMenuState.AI:
                SetPlayerToJoined(i);
                DisplayConfirmUpdate();
                audioM.Spawn_UI_Confirm();
                break;
            case ActorMenuState.JOINED:
                SetPlayerToReady(i);
                DisplayConfirmUpdate();
                audioM.Spawn_UI_Confirm();
                break;
            case ActorMenuState.READY:
                CheckCan_StartGame();
                break;
        }
    }

    //Inactive > AI > inactive
    //Inactive > player > AI > inactive
    //Cannot go from AI to player
    //Inactive > AI will check for Display confirm
    void PressedToggleAI (int i)
    {
        allPlayersInactive = false;
        if (playersState[i] == ActorMenuState.AI)
        {
            SetPlayerToInactive(i);
        }
        else
        {
            SetPlayerToAI(i);
            DisplayConfirmUpdate();
        }
    }

    void PressedBack(int i)
    {
        switch (playersState[i])
        {
            case ActorMenuState.JOINED:
            case ActorMenuState.AI:
                SetPlayerToInactive(i);
                break;
            case ActorMenuState.READY:
                SetPlayerToJoined(i);
                break;
        }

        confirmSelect.SetActive(false);
        confirmDisplayed = false;

        CheckIfAllPlayersInactive_andReturnToMain();
    }

    #endregion

    #region Ready  
    void DisplayConfirmUpdate ()
    {
        //See if there are 2+ READY players and 0 players in JOINED mode.
        int activePlayers = 0;
        int AIPlayers = 0;
        int RealPlayers = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playersState[i] == ActorMenuState.JOINED) //First check if all players are Ready or Inactive. If not, then let nothing 
            {
                confirmSelect.SetActive(false);
                confirmDisplayed = false;
                return;
            }
            else if (playersState[i] == ActorMenuState.READY)
            {
                activePlayers++;
                RealPlayers++;
            }
            else if (playersState[i] == ActorMenuState.AI)
            {
                activePlayers++;
                AIPlayers++;
            }
        }

        //If confirm screen is already displayed, then start level.
        if (activePlayers >= 1)
        {
            if (AIPlayers >= 4) //If have 4 AI players, then go straight into map select
            {
                menuManager.TankSelect_AllPlayersReady();
            }
            else if (RealPlayers >= 1) //If there is at least 1 real player, then reveal Confirm dialog 
            {
                confirmSelect.SetActive(true);
                confirmDisplayed = true;
            }
        }
    }

    void CheckCan_StartGame  ()
    {
        //See if there are 2 locked players and no players still choosing.
        int activePlayers = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playersState[i] == ActorMenuState.JOINED) //First check if all players are Ready or Inactive. If not, then let nothing 
            {
                confirmSelect.SetActive(false);
                confirmDisplayed = false;
            }
            else if (playersState[i] == ActorMenuState.READY || playersState[i] == ActorMenuState.AI)
            {
                activePlayers++;
            }
        }

        //If confirm screen is already displayed, then start level.
        if (activePlayers >= 1 && confirmDisplayed) 
        {
            menuManager.TankSelect_AllPlayersReady();
        }
        else
        {
            audioM.Spawn_UI_Cancel();
        }
    }
    
    void CheckIfAllPlayersInactive_andReturnToMain ()
    {
        //If all players are INACTIVE or AI and someone pressed Back, then go back to main menu
        bool _allPlayersInactive = true;
        for (int _i = 0; _i < 4; _i++)
        {
            if (playersState[_i] == ActorMenuState.JOINED || playersState[_i] == ActorMenuState.READY)
            {
                _allPlayersInactive = false;
            }
        }

        //If no player was ready, then go back to Main Menu.
        if (_allPlayersInactive)
        {
            //Cache allPlayersInactive so the game doesn't go back to menu the first time all players become inactive, but have to press Back again to make it happen
            if (allPlayersInactive) 
            {
                menuManager.ToMain();
            }
            else
            {
                allPlayersInactive = true;
            }
        }
        else
        {
            allPlayersInactive = false;
        }
    }
    #endregion

    #region Player absent/joined/ready 
    void UpdateAllPlayerVisibility()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (playersState[i])
            {
                case ActorMenuState.INACTIVE:
                    SetPlayerToInactive(i);
                    break;
                case ActorMenuState.JOINED:
                    SetPlayerToJoined(i);
                    break;
                case ActorMenuState.READY:
                    SetPlayerToReady(i);
                    break;
                case ActorMenuState.AI:
                    SetPlayerToAI(i);
                    break;
                default:
                    break;
            }
        }
    }

    void SetPlayerToInactive(int i)
    {
        //State
        playersState[i] = ActorMenuState.INACTIVE;

        //Update UI components
        TankPlayerSlotText[i].text = "P" + (i + 1);
        PressToJoinText[i].enabled = true;
        PressToChangeText[i].enabled = false;

        tankImage[i].sprite = noTank_cross;
        tankImage[i].color = color_text_TankName;
        ReadyTexts[i].text = "...";
        ReadyTexts[i].color = color_text_TankName;

        gm.playerType[i] = PlayerTypes.INACTIVE;

        confirmSelect.SetActive(false);
        confirmDisplayed = false;
    }

    void SetPlayerToJoined(int i)
    {
        confirmSelect.SetActive(false);
        confirmDisplayed = false;

        //State
        playersState[i] = ActorMenuState.JOINED;

        //Update UI components
        TankPlayerSlotText[i].text = "P" + (i + 1);
        PressToJoinText[i].enabled = false;
        PressToChangeText[i].enabled = true;
        tankImage[i].color = color_tankImg_joining;

        UpdateTankImage(i);
        ReadyTexts[i].text = GetModelName(i);
        ReadyTexts[i].color = color_text_TankName;

        gm.playerType[i] = PlayerTypes.REAL_PERSON;
    }

    void SetPlayerToReady(int i)
    {
        //State
        playersState[i] = ActorMenuState.READY;

        //Update UI components
        TankPlayerSlotText[i].text = "P" + (i + 1);
        PressToJoinText[i].enabled = false;
        PressToChangeText[i].enabled = false;
        tankImage[i].color = tankColors[i];

        UpdateTankImage(i);
        ReadyTexts[i].text = "READY!";
        ReadyTexts[i].color = color_text_READY;

        gm.playerType[i] = PlayerTypes.REAL_PERSON;
    }

    void SetPlayerToAI(int i)
    {
        //State
        playersState[i] = ActorMenuState.AI;

        //Update UI components
        TankPlayerSlotText[i].text = "A.I.";
        PressToJoinText[i].enabled = false;
        PressToChangeText[i].enabled = false;
        GetNextModel(i, true);
        //TankImage[i].enabled = true;
        tankImage[i].color = tankColors[i];
        UpdateTankImage(i);
        ReadyTexts[i].text = "A.I.";
        ReadyTexts[i].color = color_text_READY;

        gm.playerType[i] = PlayerTypes.AI;
    }
    #endregion

    string GetModelName (int i)
    {
        switch (gm.tankModelNames[i])
        {
            case TankModelNames.RIFLE:
                return "RIFLE";
            case TankModelNames.SHOTGUN:
                return "SPLASH";
            case TankModelNames.GRENADE:
                return "GRENADE";
            case TankModelNames.BOUNCER:
                return "BOUNCER";
            default:
            case TankModelNames.SEEKER:
                return "SEEKER";
        }
    }
}

    public enum ActorMenuState
    {
        INACTIVE, //Haven't pressed start.
        JOINED, //Pressed start.
        READY, //Pressed start and chosen a costum.
        AI
    }