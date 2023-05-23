using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScMenu_UIManager : MonoBehaviour
{
    public static ScMenu_UIManager instance;

    #region Fields
    MenuStates currentState = MenuStates.MAIN;

    [Space(10)]
    [Header("REFERENCE CANVAS GROUPS")]
    public CanvasGroup cvsMenu;
    public CanvasGroup cvsCampaignSelect;
    public CanvasGroup cvsTANKSELECT;
    public CanvasGroup cvsMapSelect;
    public CanvasGroup cvsAbout;

    [Space(10)]
    [Header("UI BUTTONs")]
    [SerializeField] GameObject menu_Button_Campaign;
    [SerializeField] GameObject menu_Button_Play;
    [SerializeField] GameObject menu_Button_About;
    [SerializeField] GameObject menu_Button_Quit;

    [SerializeField] GameObject TankMenu_Button_Back;
    [SerializeField] GameObject About_Button_Back;
    [SerializeField] GameObject MapSel_Button_Back;

    [SerializeField] GameObject Map_Button_Night;
    [SerializeField] GameObject Map_Button_Combat;
    [SerializeField] GameObject Map_Button_Ocean;
    [SerializeField] GameObject Map_Button_Desert;
    [SerializeField] GameObject Map_Button_Torch;
    [SerializeField] GameObject Map_Button_Arcade;
    [SerializeField] GameObject Map_Button_Space;
    [SerializeField] GameObject Map_Button_Random;

    [Space(10)]
    [Header("FADING SPEED")]
    public float initialWait = 0.5f;    
    public float fadeSpeed_menuTransition = 30f;
    public float fadeSpeed_menuInitial = 2f;

    [Space(10)]
    [Header("NEXT LEVEL")]
    int buildIndex_brawl = 1;

    [Space(10)]
    [Header("OTHERS")]
    public CaveStoryFaderProper fader;
    [HideInInspector] public bool isCampaignMode;

    //Reference class
    InputManager inputM;
    EventSystem eventSystem;
    AudioManager audioM;
    GM gm;
    TankSelectMenu_SubManager tankSelect_Manager;
    CampaignLevelSelect_SubManager campainSelect_Manager;

    //Menu state
    GameObject lastSelect_mainMenu;
    GameObject lastSelect_mapSelect;
    bool inTransition = false;
    #endregion

    #region Monobehavior Start
    void Awake()
    {
        //Init
        cvsMenu.alpha = 0f;
        instance = this;

        //Hide all canvases
        UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(MenuStates.MAIN));
        UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(MenuStates.CAMPAIGN_LEVELS));
        UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(MenuStates.TANKSELECT));
        UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(MenuStates.MAPSELECT));
        UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(MenuStates.ABOUT));
    }

    void Start ()
    {
        inputM = InputManager.Instance;
        eventSystem = EventSystem.current;
        tankSelect_Manager = TankSelectMenu_SubManager.instance;
        eventSystem.SetSelectedGameObject(menu_Button_Play);
        audioM = AudioManager.instance;
        gm = GM.instance;
        campainSelect_Manager = CampaignLevelSelect_SubManager.instance;

        //Fade in menu canvas group
        //yield return new WaitForSeconds(initialWait);
        //StartCoroutine(UIFadeUtil.Canvas_FadeToOpaque(cvsMenu, fadeSpeed_menuInitial));
        UIFadeUtil.Canvas_InstantOpaque(cvsMenu);

        //menu_Button_Play.GetComponent<Selectable>().OnPointerEnter(null);
        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(menu_Button_Play);
        menu_Button_Play.GetComponent<Selectable>().OnPointerEnter(null);
    }
    #endregion

    #region Public method hook
    // MENU BUTTONS
    public void ToMain()
    {
        StateChange(MenuStates.MAIN); 
    }

    public void ToCampaignOrMapSelect()
    {
        if (!isCampaignMode)
        {
            StateChange(MenuStates.MAPSELECT);
        }
        else
        {
            StateChange(MenuStates.CAMPAIGN_LEVELS);
        }
    }

    public void MenuButtonClick_Maps()
    {
        lastSelect_mainMenu = menu_Button_Play;
        isCampaignMode = false;

        StateChange(MenuStates.MAPSELECT);
    }

    public void MenuButtonClick_Campaign()
    {
        lastSelect_mainMenu = menu_Button_Campaign;
        isCampaignMode = true;
        StateChange(MenuStates.CAMPAIGN_LEVELS);
    }

    public void MenuButtonClick_ToAbout()
    {
        lastSelect_mainMenu = menu_Button_About;
        StateChange(MenuStates.ABOUT);
    }

    public void MenuButtonClick_ToQuitGame()
    {
        Application.Quit();
    }

    //MAP BUTTONS
    public void ToCombat()
    {
        audioM.Spawn_UI_Confirm();
        gm.UpdatePallete(GameMode.PVP_Combat);
        GM.gameMode = GameMode.PVP_Combat;

        StateChange(MenuStates.TANKSELECT);
    }

    public void ToOcean()
    {
        audioM.Spawn_UI_Confirm();
        gm.UpdatePallete(GameMode.PVP_OceanMist);
        GM.gameMode = GameMode.PVP_OceanMist;

        StateChange(MenuStates.TANKSELECT);
    }

    public void ToDesert ()
    {
        audioM.Spawn_UI_Confirm();
        gm.UpdatePallete(GameMode.PVP_Desert);
        GM.gameMode = GameMode.PVP_Desert;
        StateChange(MenuStates.TANKSELECT);
    }

    public void ToSpace()
    {
        audioM.Spawn_UI_Confirm();
        gm.UpdatePallete(GameMode.Hanabi);
        GM.gameMode = GameMode.Hanabi;
        StateChange(MenuStates.TANKSELECT);
    }

    public void ToNight_GameMode()
    {
        audioM.Spawn_UI_Confirm();
        GM.gameMode = GameMode.PVP_Night;
        gm.UpdatePallete(GameMode.PVP_Night);
        StateChange(MenuStates.TANKSELECT);
    }

    public void ToCoop()
    {
        audioM.Spawn_UI_Confirm();
        GM.gameMode = GameMode.Coop_Arcade;
        gm.UpdatePallete(GameMode.Coop_Arcade);
        StateChange(MenuStates.TANKSELECT);
    }

    public void ToSpooky()
    {
        audioM.Spawn_UI_Confirm();
        GM.gameMode = GameMode.Coop_Torch;
        gm.UpdatePallete(GameMode.Coop_Torch);
        StateChange(MenuStates.TANKSELECT);
    }

    public void ToRandom()
    {
        audioM.Spawn_UI_Confirm();
        switch (Random.Range(0, 7))
        {
            case 0:
                ToCombat();
                break;
            case 1:
                ToNight_GameMode();
                break;
            case 2:
                ToDesert();
                break;
            case 3:
                ToOcean();
                break;
            case 4:
                ToSpace();
                break;
            case 5:
                ToSpooky();
                break;
            case 6:
            default:
                ToCoop();
                break;
        }
    }


    //NON-UI-BUTTON CALLS
    public void TankSelect_AllPlayersReady() //To map select
    {
        StartCoroutine(GoToLevel(buildIndex_brawl));
    }

    public void SelectedCampaignLevel (int index)
    {
        audioM.Spawn_UI_Confirm();
        GM.gameMode = GameMode.Campaign;
        gm.UpdatePallete(GameMode.Campaign);
        StateChange(MenuStates.TANKSELECT);
    }
    #endregion

    #region Scene and Canvas transitioning
    IEnumerator GoToLevel(int levelIndex)
    {
        if (!inTransition)
        {
            inTransition = true;
            fader.CloseTiles();
            yield return new WaitForSeconds(0.8f);
            SceneManager.LoadScene(levelIndex);
        }
    }
    #endregion

    #region Navigation Update
    void Update()
    {
        if (currentState == MenuStates.MAIN)
        {
            Update_Menu();
        }
        else if (currentState == MenuStates.TANKSELECT)
        {
            Update_TankSelect();
        }
        else if (currentState == MenuStates.MAPSELECT)
        {
            Update_MapSelect();
        }
        else if (currentState == MenuStates.CAMPAIGN_LEVELS)
        {
            Update_Campaign();
            campainSelect_Manager.OnUpdate();
        }
        else if (currentState == MenuStates.ABOUT)
        {
            Update_Info();
        }
    }

    void Update_Menu()
    {
        if (eventSystem.currentSelectedGameObject == null) //If selected no button. 
        {
            if (lastSelect_mainMenu == null)
            {
                eventSystem.SetSelectedGameObject(menu_Button_Play);
                menu_Button_Play.GetComponent<Selectable>().OnPointerEnter(null);
            }
            else
            {
                eventSystem.SetSelectedGameObject(lastSelect_mainMenu);
                lastSelect_mainMenu.GetComponent<Selectable>().OnPointerEnter(null);
            }
        }
        //AT CHAR-SELECT BUTTON
        else if (eventSystem.currentSelectedGameObject == menu_Button_Play)
        {
            //Press DOWN
            if (inputM.AnyRight_Down)
            {
                SelectButton(menu_Button_Campaign);
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                MenuButtonClick_Maps();
            }
        }
        //AT CHAR-SELECT BUTTON
        else if (eventSystem.currentSelectedGameObject == menu_Button_Campaign)
        {
            //Press DOWN
            if (inputM.AnyLeft_Down)
            {
                SelectButton(menu_Button_Play);
            }
            else if (inputM.AnyRight_Down)
            {
                SelectButton(menu_Button_About);
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                MenuButtonClick_Campaign();
            }
        }
        //ABOUT
        else if (eventSystem.currentSelectedGameObject == menu_Button_About)
        {
            //Press UP
            if (inputM.AnyLeft_Down)
            {
                SelectButton(menu_Button_Campaign);
            }
            //Press DOWN
            if (inputM.AnyRight_Down)
            {
                SelectButton(menu_Button_Quit);
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                //audioM.Spawn_UI_click_Soft();
                StateChange(MenuStates.ABOUT);
            }
        }
        //AT QUIT BUTTON
        else if (eventSystem.currentSelectedGameObject == menu_Button_Quit)
        {
            //Press UP
            if (inputM.AnyLeft_Down)
            {
                SelectButton(menu_Button_About);
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                Application.Quit();
            }
        }
    }

    void SelectButton (GameObject button)
    {
        eventSystem.SetSelectedGameObject(button);
        lastSelect_mainMenu = button;
        audioM.Spawn_UI_Confirm();
    }

    void Update_TankSelect()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //audioM.Spawn_UI_Cancel();
            StateChange(MenuStates.MAIN);
            return;
        }

        tankSelect_Manager.OnUpdate();
    }

    void Update_Campaign ()
    {
        if (canScreenChange && (inputM.AnyStart_Down || Input.GetKeyDown(KeyCode.Escape) || inputM.AnyB_Down))
        {
            StateChange(MenuStates.MAIN);
            //audioM.Spawn_UI_Confirm();
        }
    }

    void Update_MapSelect()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || inputM.AnyB_Down)
        {
            //audioM.Spawn_UI_Cancel();
            StateChange(MenuStates.MAIN);
            return;
        }

        if (eventSystem.currentSelectedGameObject == null) //If selected no button. 
        {
            if (lastSelect_mapSelect == null)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Combat);
                Map_Button_Night.GetComponent<Selectable>().OnPointerEnter(null);
            }
            else
            {
                eventSystem.SetSelectedGameObject(lastSelect_mapSelect);
                lastSelect_mapSelect.GetComponent<Selectable>().OnPointerEnter(null);
            }
        }
        //===============COMBAT===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Combat)
        {
            //Press DOWN
            if (inputM.AnyDown_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Ocean);
                lastSelect_mapSelect = Map_Button_Combat;
                audioM.Spawn_UI_Confirm();
            }
            //Press RIGHT
            if (inputM.AnyRight_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Desert);
                lastSelect_mapSelect = Map_Button_Combat;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToCombat();
            }
        }
        //===============DESERT===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Desert)
        {
            //Press DOWN
            if (inputM.AnyDown_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Night);
                lastSelect_mapSelect = Map_Button_Desert;
                audioM.Spawn_UI_Confirm();
            }
            //Press LEFT
            if (inputM.AnyLeft_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Combat);
                lastSelect_mapSelect = Map_Button_Desert;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToDesert();
            }
        }
        //===============OCEAN===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Ocean)
        {
            //Press UP
            if (inputM.AnyUp_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Combat);
                lastSelect_mapSelect = Map_Button_Ocean;
                audioM.Spawn_UI_Confirm();
            }
            //Press RIGHT
            if (inputM.AnyRight_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Night);
                lastSelect_mapSelect = Map_Button_Ocean;
                audioM.Spawn_UI_Confirm();
            }
            //Press DOWN
            if (inputM.AnyDown_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Space);
                lastSelect_mapSelect = Map_Button_Ocean;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToOcean();
            }
        }
        //===============NIGHT===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Night) //1
        {
            //Press UP
            if (inputM.AnyUp_Down) //2
            {
                eventSystem.SetSelectedGameObject(Map_Button_Desert); //3
                lastSelect_mapSelect = Map_Button_Night; //4
                audioM.Spawn_UI_Confirm();
            }
            //Press LEFT
            if (inputM.AnyLeft_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Ocean);
                lastSelect_mapSelect = Map_Button_Night;
                audioM.Spawn_UI_Confirm();
            }
            //Press DOWN
            if (inputM.AnyDown_Down) //2
            {
                eventSystem.SetSelectedGameObject(Map_Button_Torch); //3
                lastSelect_mapSelect = Map_Button_Night; //4
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToNight_GameMode();                
            }
        }
        //===============SPACE===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Space)
        {
            //Press UP
            if (inputM.AnyUp_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Ocean);
                lastSelect_mapSelect = Map_Button_Space;
                audioM.Spawn_UI_Confirm();
            }
            //Press RIGHT
            if (inputM.AnyRight_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Torch);
                lastSelect_mapSelect = Map_Button_Space;
                audioM.Spawn_UI_Confirm();
            }
            //Press Down
            if (inputM.AnyDown_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Arcade);
                lastSelect_mapSelect = Map_Button_Space;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToSpace();
            }
        }
        //===============TORCH===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Torch)
        {
            //Press UP
            if (inputM.AnyUp_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Night);
                lastSelect_mapSelect = Map_Button_Torch;
                audioM.Spawn_UI_Confirm();
            }
            //Press LEFT
            if (inputM.AnyLeft_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Space);
                lastSelect_mapSelect = Map_Button_Torch;
                audioM.Spawn_UI_Confirm();
            }
            //Press DOWN
            if (inputM.AnyDown_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Random);
                lastSelect_mapSelect = Map_Button_Torch;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToSpooky();
            }
        }

        //===============ARCADE===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Arcade)
        {
            //Press UP
            if (inputM.AnyUp_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Space);
                lastSelect_mapSelect = Map_Button_Arcade;
                audioM.Spawn_UI_Confirm();
            }
            //Press RIGHT
            if (inputM.AnyRight_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Random);
                lastSelect_mapSelect = Map_Button_Arcade;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToCoop();
            }
        }

        //===============RANDOM===================
        else if (eventSystem.currentSelectedGameObject == Map_Button_Random)
        {
            //Press UP
            if (inputM.AnyUp_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Torch);
                lastSelect_mapSelect = Map_Button_Random;
                audioM.Spawn_UI_Confirm();
            }
            //Press LEFT
            if (inputM.AnyLeft_Down)
            {
                eventSystem.SetSelectedGameObject(Map_Button_Arcade);
                lastSelect_mapSelect = Map_Button_Random;
                audioM.Spawn_UI_Confirm();
            }
            //Confirm
            else if (inputM.AnyA_Down || inputM.AnyStart_Down)
            {
                ToRandom();
            }
        }
    }

    public IEnumerator PreventScreenChangeForOneFrame ()
    {
        //Prevents player pressing B to remap key and then also causing the screen to change (returning to main menu) in the same frame. 
        canScreenChange = false;
        yield return null;
        canScreenChange = true;
    }
    bool canScreenChange = true;

    void Update_Info()
    {
        //eventSystem.SetSelectedGameObject(About_Button_Back);

        if (canScreenChange && (inputM.AnyStart_Down || Input.GetKeyDown(KeyCode.Escape) || inputM.AnyB_Down))
        {
            StateChange(MenuStates.MAIN);
            //audioM.Spawn_UI_Confirm();
        }
    }
    #endregion

    #region Transition
    void StateChange (MenuStates newState)
    {
        if (currentState != newState)
        {
            audioM.Spawn_UI_Confirm();
            ClearUISelection();

            UIFadeUtil.Canvas_InstantTransparent(MenuStateToCanvas(currentState));
            UIFadeUtil.Canvas_InstantOpaque(MenuStateToCanvas(newState));

            switch (newState)
            {
                case MenuStates.MAIN:
                    eventSystem.SetSelectedGameObject(lastSelect_mainMenu);
                    lastSelect_mainMenu.GetComponent<Selectable>().OnPointerEnter(null);
                    break;
                case MenuStates.CAMPAIGN_LEVELS:
                    campainSelect_Manager.ResetButtonSelection();
                    break;
                case MenuStates.TANKSELECT:
                    if (currentState != MenuStates.MAPSELECT)
                        tankSelect_Manager.ResetAllChar();
                    break;
                case MenuStates.MAPSELECT:
                    eventSystem.SetSelectedGameObject(Map_Button_Combat);
                    Map_Button_Combat.GetComponent<Selectable>().OnPointerEnter(null);
                    break;
                case MenuStates.ABOUT:
                    //eventSystem.SetSelectedGameObject(About_Button_Back);
                    //lastSelect = About_Button_Back;
                    break;
                default:
                    Debug.Log("ERROR: No such state or canvas group exist.");
                    break;
            }
            currentState = newState;
        }
    }
    #endregion

    #region Enum to canvas Look up
    CanvasGroup MenuStateToCanvas(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.MAIN:
                return cvsMenu;
            case MenuStates.TANKSELECT:
                return cvsTANKSELECT;
            case MenuStates.MAPSELECT:
                return cvsMapSelect;
            case MenuStates.ABOUT:
                return cvsAbout;
            case MenuStates.CAMPAIGN_LEVELS:
                return cvsCampaignSelect;
            default:
                Debug.Log("ERROR: No such state or canvas group exist.");
                return null;
        }
    }
    #endregion


    void ClearUISelection()
    {
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(null);
    }
}

public enum MenuStates
{
    MAIN,
    TANKSELECT,
    CAMPAIGN_LEVELS,
    MAPSELECT,
    ABOUT
}

//public enum MenuStates
//{
//    MAIN,
//    TANKSELECT,
//    MAPSELECT,
//    ABOUT,
//    TRANSITIONING
//}