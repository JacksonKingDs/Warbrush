using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuUI : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject btn_resume;
    public GameObject btn_restart;
    public GameObject btn_ToMenu;

    public AudioListener listener;
    

    [SerializeField] string sc_MainMenuName;
    [SerializeField] float timeBeforeInitialFadeOut = 2f; //Need to wait for Generic black fader to fade it.

    //Canvas groups
    [SerializeField] CanvasGroup pauseMenu;

    [SerializeField] Scene MenuScene;
    [SerializeField] CanvasGroupFader blackFader;

    InputManager inputM;
    AudioManager audioM;

    bool inSceneTransition = true;

    #region MonoBehavior
    void Awake()
    {
        //Hide pause menu.
        UIFadeUtil.Canvas_InstantTransparent(pauseMenu);
    }

    IEnumerator Start()
    {
        inputM = InputManager.Instance;
        audioM = AudioManager.instance;

        //Wait time before allowing for pausing. 
        yield return new WaitForSeconds(timeBeforeInitialFadeOut);
        inSceneTransition = false;
    }

    float toggleCDTimer = 0f;

    void Update()
    {
        if (toggleCDTimer > 0f)
        {
            toggleCDTimer -= Time.unscaledDeltaTime;

            return;
        }

        if (IsPaused && !inSceneTransition)
        {
            if (EventSystem.current.currentSelectedGameObject == null) //If selected no button. 
            {
                EventSystem.current.SetSelectedGameObject(btn_resume);
            }
            //AT CHAR-SELECT BUTTON
            else if (EventSystem.current.currentSelectedGameObject == btn_resume)
            {
                //Press DOWN >> Restart
                if (inputM.AnyDown_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_restart);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Press Up >> Quit to menu
                else if (inputM.AnyUp_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_ToMenu);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Confirm
                else if (inputM.AnyA_Down)
                {
                    TogglePause();
                    audioM.Spawn_UI_Confirm(true);
                }
            }
            //AT CHAR-SELECT BUTTON
            else if (EventSystem.current.currentSelectedGameObject == btn_restart)
            {
                //Press DOWN >> Quit to menu
                if (inputM.AnyDown_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_ToMenu);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Press Up >> Resume
                else if (inputM.AnyUp_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_resume);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Confirm
                else if (inputM.AnyA_Down || inputM.AnyStart_Down)
                {
                    RestartLevel();
                    audioM.Spawn_UI_Confirm(true);
                }
            }
            //QUIT
            else if (EventSystem.current.currentSelectedGameObject == btn_ToMenu)
            {
                //Press Down >> RESUME
                if (inputM.AnyDown_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_resume);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Press Up >> Restart
                else if (inputM.AnyUp_Down)
                {
                    EventSystem.current.SetSelectedGameObject(btn_restart);
                    audioM.Spawn_UI_click_Soft(true);
                }
                //Confirm
                else if (inputM.AnyA_Down || inputM.AnyStart_Down)
                {
                    ToStartMenu();
                    audioM.Spawn_UI_Confirm(true);
                }
            }
        }

        if (!inSceneTransition && FightSceneManager.sceneState == FightSceneStates.PLAY && (inputM.AnyStart_Down || inputM.AnyXboxX_Down || Input.GetKeyDown(KeyCode.Escape)))
        {
            TogglePause();
        }
    }
    
    #endregion

    #region Public
    public void RestartLevel ()
    {
        if (!inSceneTransition)
        {
            Debug.Log("Restart");
            inSceneTransition = true;
            StartCoroutine(DoRestart());
        }
    }

    IEnumerator  DoRestart()
    {
        Time.timeScale = 1f;
        blackFader.FadeIn();
        AudioListener.pause = false;
        IsPaused = false;
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // FightSceneManager.instance.RestartLevel();
    }

    public void ToStartMenu()
    {
        if (!inSceneTransition)
        {
            Debug.Log("To menu");
            inSceneTransition = true;
            Time.timeScale = 1f;
            blackFader.FadeIn();
            AudioListener.pause = false;
            IsPaused = false;

            StartCoroutine(DoToMenu());
        }
    }

    IEnumerator DoToMenu()
    {
        yield return new WaitForSeconds(1.5f);
        EventSystem.current.SetSelectedGameObject(null);

        FightSceneManager.instance.PauseMenuGoToMainMenu();
    }

    void TogglePause()
    {
        //Debug.Log("In toggle pause: " + IsPaused);

        if (toggleCDTimer > 0)
            return;
        else
            toggleCDTimer = 0.1f;

        IsPaused = !IsPaused;
                
        //Pause
        if (IsPaused)
        {
            AudioListener.pause = true;
            UIFadeUtil.Canvas_InstantOpaque(pauseMenu);
            Time.timeScale = 0f;
            if (EventSystem.current.currentSelectedGameObject == null) //If selected no button. 
            {
                EventSystem.current.SetSelectedGameObject(btn_resume);
            }
        }
        //Unpause
        else
        {
            AudioListener.pause = false;
            Time.timeScale = 1f;
            UIFadeUtil.Canvas_InstantTransparent(pauseMenu);
            EventSystem.current.SetSelectedGameObject(null);
            //if (EventSystem.current.currentSelectedGameObject == null) //If selected no button. 
            //{
            //    EventSystem.current.SetSelectedGameObject(null);
            //}
        }
    }
    #endregion
}