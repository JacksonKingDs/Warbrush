using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

//3 parts to this:
//Set player pref
//Set InputM.inputs
//Set ui display

public class KeyRemapper : MonoBehaviour
{
    #region Variables
    public InputManager inputM;
    public ScMenu_UIManager menuM;
    AudioManager audioM;

    //public Text PressAKeyPrompt;

    public Text ui_p1_left;
    public Text ui_p1_right;
    public Text ui_p1_up;
    public Text ui_p1_down;
    public Text ui_p1_a;
    public Text ui_p1_b;

    public Text ui_p2_left;
    public Text ui_p2_right;
    public Text ui_p2_up;
    public Text ui_p2_down;
    public Text ui_p2_a;
    public Text ui_p2_b;

    public Text ui_p3_left;
    public Text ui_p3_right;
    public Text ui_p3_up;
    public Text ui_p3_down;
    public Text ui_p3_a;
    public Text ui_p3_b;

    public Text ui_p4_left;
    public Text ui_p4_right;
    public Text ui_p4_up;
    public Text ui_p4_down;
    public Text ui_p4_a;
    public Text ui_p4_b;

    static Dictionary<string, Text> uiTextElement;

    const string stringKey_p1_left = "p1_left";
    const string stringKey_p1_right = "p1_right";
    const string stringKey_p1_up = "p1_up";
    const string stringKey_p1_down = "p1_down";
    const string stringKey_p1_a = "p1_a";
    const string stringKey_p1_b = "p1_b";

    const string stringKey_p2_left = "p2_left";
    const string stringKey_p2_right = "p2_right";
    const string stringKey_p2_up = "p2_up";
    const string stringKey_p2_down = "p2_down";
    const string stringKey_p2_a = "p2_a";
    const string stringKey_p2_b = "p2_b";
    
    const string stringKey_p3_left = "p3_left";
    const string stringKey_p3_right = "p3_right";
    const string stringKey_p3_up = "p3_up";
    const string stringKey_p3_down = "p3_down";
    const string stringKey_p3_a = "p3_a";
    const string stringKey_p3_b = "p3_b";
    
    const string stringKey_p4_left = "p4_left";
    const string stringKey_p4_right = "p4_right";
    const string stringKey_p4_up = "p4_up";
    const string stringKey_p4_down = "p4_down";
    const string stringKey_p4_a = "p4_a";
    const string stringKey_p4_b = "p4_b";

    //Remapping key wait
    [HideInInspector] public bool waitingForKey = false;
    Text activeUIText;
    #endregion

    #region Initialization
    private void Awake()
    {
        //Initialize dictionary to associate UI-Text with a string key
        uiTextElement = new Dictionary<string, Text>()
        {
            {stringKey_p1_left, ui_p1_left },
            {stringKey_p1_right, ui_p1_right },
            {stringKey_p1_up, ui_p1_up },
            {stringKey_p1_down, ui_p1_down },
            {stringKey_p1_a, ui_p1_a },
            {stringKey_p1_b, ui_p1_b },

            {stringKey_p2_left, ui_p2_left },
            {stringKey_p2_right, ui_p2_right },
            {stringKey_p2_up, ui_p2_up },
            {stringKey_p2_down, ui_p2_down },
            {stringKey_p2_a, ui_p2_a },
            {stringKey_p2_b, ui_p2_b },

            {stringKey_p3_left, ui_p3_left },
            {stringKey_p3_right, ui_p3_right },
            {stringKey_p3_up, ui_p3_up },
            {stringKey_p3_down, ui_p3_down },
            {stringKey_p3_a, ui_p3_a },
            {stringKey_p3_b, ui_p3_b },

            {stringKey_p4_left, ui_p4_left },
            {stringKey_p4_right, ui_p4_right },
            {stringKey_p4_up, ui_p4_up },
            {stringKey_p4_down, ui_p4_down },
            {stringKey_p4_a, ui_p4_a },
            {stringKey_p4_b, ui_p4_b },
        };

        audioM = AudioManager.instance;
    }
    #endregion

    #region Read Keys from Save file
    void Start()
    {
        UpdateInputManagerKeyMapping();
        UpdateAllUiTextDisplay();

        //Debug only
        //if (GM.gameMode != GameMode.Campaign)
        //{
        //    UpdateAllUiTextDisplay();
        //}
    }

    public void PrintAllKeys()
    {
        Debug.Log(stringKey_p1_left     + InputManager.p1_leftKey);
        Debug.Log(stringKey_p1_right    + InputManager.p1_rightKey);
        Debug.Log(stringKey_p1_up       + InputManager.p1_upKey);
        Debug.Log(stringKey_p1_down     + InputManager.p1_downKey);
        Debug.Log(stringKey_p1_a        + InputManager.p1_aKey);
        Debug.Log(stringKey_p1_b        + InputManager.p1_bKey);

        Debug.Log(stringKey_p2_left + InputManager.p2_leftKey);
        Debug.Log(stringKey_p2_right + InputManager.p2_rightKey);
        Debug.Log(stringKey_p2_up + InputManager.p2_upKey);
        Debug.Log(stringKey_p2_down + InputManager.p2_downKey);
        Debug.Log(stringKey_p2_a + InputManager.p2_aKey);
        Debug.Log(stringKey_p2_b + InputManager.p2_bKey);

        Debug.Log(stringKey_p3_left + InputManager.p3_leftKey);
        Debug.Log(stringKey_p3_right + InputManager.p3_rightKey);
        Debug.Log(stringKey_p3_up + InputManager.p3_upKey);
        Debug.Log(stringKey_p3_down + InputManager.p3_downKey);
        Debug.Log(stringKey_p3_a + InputManager.p3_aKey);
        Debug.Log(stringKey_p3_b + InputManager.p3_bKey);

        Debug.Log(stringKey_p4_left + InputManager.p4_leftKey);
        Debug.Log(stringKey_p4_right + InputManager.p4_rightKey);
        Debug.Log(stringKey_p4_up + InputManager.p4_upKey);
        Debug.Log(stringKey_p4_down + InputManager.p4_downKey);
        Debug.Log(stringKey_p4_a + InputManager.p4_aKey);
        Debug.Log(stringKey_p4_b + InputManager.p4_bKey);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PrintAllKeys();
    }
    #endregion

    #region Update All
    void UpdateInputManagerKeyMapping()
    {
        InputManager.p1_leftKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_left, (int)KeyCode.A);
        InputManager.p1_rightKey    = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_right, (int)KeyCode.D);
        InputManager.p1_upKey       = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_up, (int)KeyCode.W);
        InputManager.p1_downKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_down, (int)KeyCode.S);
        InputManager.p1_aKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_a, (int)KeyCode.J);
        InputManager.p1_bKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p1_b, (int)KeyCode.K);

        InputManager.p2_leftKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_left, (int)KeyCode.LeftArrow);
        InputManager.p2_rightKey    = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_right, (int)KeyCode.RightArrow);
        InputManager.p2_upKey       = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_up, (int)KeyCode.UpArrow);
        InputManager.p2_downKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_down, (int)KeyCode.DownArrow);
        InputManager.p2_aKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_a, (int)KeyCode.RightShift);
        InputManager.p2_bKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p2_b, (int)KeyCode.RightControl);

        InputManager.p3_leftKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_left, (int)KeyCode.F);
        InputManager.p3_rightKey    = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_right, (int)KeyCode.H);
        InputManager.p3_upKey       = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_up, (int)KeyCode.T);
        InputManager.p3_downKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_down, (int)KeyCode.G);
        InputManager.p3_aKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_a, (int)KeyCode.V);
        InputManager.p3_bKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p3_b, (int)KeyCode.B);

        InputManager.p4_leftKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_left, (int)KeyCode.Keypad4);
        InputManager.p4_rightKey    = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_right, (int)KeyCode.Keypad6);
        InputManager.p4_upKey       = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_up, (int)KeyCode.Keypad8);
        InputManager.p4_downKey     = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_down, (int)KeyCode.Keypad5);
        InputManager.p4_aKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_a, (int)KeyCode.KeypadPlus);
        InputManager.p4_bKey        = (KeyCode)PlayerPrefs.GetInt(stringKey_p4_b, (int)KeyCode.KeypadEnter);
    }

    void ResetPlayerPref ()
    {
        //Set keys
        PlayerPrefs.SetInt(stringKey_p1_left, (int)KeyCode.A);
        PlayerPrefs.SetInt(stringKey_p1_right, (int)KeyCode.D);
        PlayerPrefs.SetInt(stringKey_p1_up, (int)KeyCode.W);
        PlayerPrefs.SetInt(stringKey_p1_down, (int)KeyCode.S);
        PlayerPrefs.SetInt(stringKey_p1_a, (int)KeyCode.J);
        PlayerPrefs.SetInt(stringKey_p1_b, (int)KeyCode.K);

        PlayerPrefs.SetInt(stringKey_p2_left, (int)KeyCode.LeftArrow);
        PlayerPrefs.SetInt(stringKey_p2_right, (int)KeyCode.RightArrow);
        PlayerPrefs.SetInt(stringKey_p2_up, (int)KeyCode.UpArrow);
        PlayerPrefs.SetInt(stringKey_p2_down, (int)KeyCode.DownArrow);
        PlayerPrefs.SetInt(stringKey_p2_a, (int)KeyCode.RightShift);
        PlayerPrefs.SetInt(stringKey_p2_b, (int)KeyCode.RightControl);

        PlayerPrefs.SetInt(stringKey_p3_left, (int)KeyCode.F);
        PlayerPrefs.SetInt(stringKey_p3_right, (int)KeyCode.H);
        PlayerPrefs.SetInt(stringKey_p3_up, (int)KeyCode.T);
        PlayerPrefs.SetInt(stringKey_p3_down, (int)KeyCode.G);
        PlayerPrefs.SetInt(stringKey_p3_a, (int)KeyCode.V);
        PlayerPrefs.SetInt(stringKey_p3_b, (int)KeyCode.B);

        PlayerPrefs.SetInt(stringKey_p4_left, (int)KeyCode.Keypad4);
        PlayerPrefs.SetInt(stringKey_p4_right, (int)KeyCode.Keypad6);
        PlayerPrefs.SetInt(stringKey_p4_up, (int)KeyCode.Keypad8);
        PlayerPrefs.SetInt(stringKey_p4_down, (int)KeyCode.Keypad5);
        PlayerPrefs.SetInt(stringKey_p4_a, (int)KeyCode.KeypadPlus);
        PlayerPrefs.SetInt(stringKey_p4_b, (int)KeyCode.KeypadEnter);
    }

    void UpdateAllUiTextDisplay()
    {
        SetUI(ui_p1_left, InputManager.p1_leftKey.ToString());
        SetUI(ui_p1_right, InputManager.p1_rightKey.ToString());
        SetUI(ui_p1_up, InputManager.p1_upKey.ToString());
        SetUI(ui_p1_down, InputManager.p1_downKey.ToString());
        SetUI(ui_p1_a, InputManager.p1_aKey.ToString());
        SetUI(ui_p1_b, InputManager.p1_bKey.ToString());

        SetUI(ui_p2_left, InputManager.p2_leftKey.ToString());
        SetUI(ui_p2_right, InputManager.p2_rightKey.ToString());
        SetUI(ui_p2_up, InputManager.p2_upKey.ToString());
        SetUI(ui_p2_down, InputManager.p2_downKey.ToString());
        SetUI(ui_p2_a, InputManager.p2_aKey.ToString());
        SetUI(ui_p2_b, InputManager.p2_bKey.ToString());

        SetUI(ui_p3_left, InputManager.p3_leftKey.ToString());
        SetUI(ui_p3_right, InputManager.p3_rightKey.ToString());
        SetUI(ui_p3_up, InputManager.p3_upKey.ToString());
        SetUI(ui_p3_down, InputManager.p3_downKey.ToString());
        SetUI(ui_p3_a, InputManager.p3_aKey.ToString());
        SetUI(ui_p3_b, InputManager.p3_bKey.ToString());

        SetUI(ui_p4_left, InputManager.p4_leftKey.ToString());
        SetUI(ui_p4_right, InputManager.p4_rightKey.ToString());
        SetUI(ui_p4_up, InputManager.p4_upKey.ToString());
        SetUI(ui_p4_down, InputManager.p4_downKey.ToString());
        SetUI(ui_p4_a, InputManager.p4_aKey.ToString());
        SetUI(ui_p4_b, InputManager.p4_bKey.ToString());
    }
    #endregion


    #region Public - Reset keys
    public void ResetAllMapping()
    {
        ResetPlayerPref();
        UpdateInputManagerKeyMapping();
        UpdateAllUiTextDisplay();
    }
    #endregion

    #region Public hook: ask to rebind a key
    string buttonOriginalString;
    int buttonOriginalFontsize;
    public void RemapKey(Text uiText)
    {
        if (!waitingForKey)
        {
            waitingForKey = true;

            buttonOriginalString = uiText.text;
            buttonOriginalFontsize = uiText.fontSize;
            uiText.text = "???";
            uiText.color = Color.red;
            uiText.fontSize = 150;
            audioM.Spawn_UI_click_Soft();

            //PressAKeyPrompt.enabled = true;            
            StartCoroutine(ListenForKeyInput(uiText));
        }
    }

    IEnumerator ListenForKeyInput (Text uiText)
    {
        while (waitingForKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || inputM.AnyStart_Down)
            {
                waitingForKey = false;
                uiText.text = buttonOriginalString;
                uiText.fontSize = buttonOriginalFontsize;
                uiText.color = Color.white;
                audioM.Spawn_UI_click_verysoft();
                yield break;
            }

            if (Input.anyKeyDown)
            {
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if ((int)keycode < 323)
                    {
                        if (Input.GetKeyDown(keycode))
                        {
                            //Display text in UI-Text element
                            SetUI(uiText, keycode.ToString());

                            StartCoroutine(menuM.PreventScreenChangeForOneFrame());

                            //Save remapped key to playerPref
                            string stringKey = Get_StringKey_Of_UiTextElement(uiText);
                            PlayerPrefs.SetInt(stringKey, (int)keycode);

                            //Update it in InputManager
                            UpdateInputManagerKeyMapping();
                            Debug.Log("Key remapped. Key " + stringKey + " button: " + keycode);

                            //Wrap up
                            waitingForKey = false;

                            audioM.Spawn_UI_click_verysoft();
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    #endregion

    #region Display Key
    void SetUI(Text uiText, string s)
    {
        //Set text
        s = GetTrimmedText(s);
        s = s.ToUpper();
        uiText.text = s;
        uiText.color = Color.white;

        //Set scale
        if (s.Length == 1)
        {
            uiText.fontSize = 300;
        }
        else if (s.Length < 6)
        {
            uiText.fontSize = 130;
        }
        else 
        {
            uiText.fontSize = 100;
        }
    }

    string GetTrimmedText(string s)
    {
        s = s.Replace("Joystick", "Joy");
        s = s.Replace("Keypad", "Pad\n");
        s = s.Replace("Control", "Ctrl");
        if (!s.Contains("Arrow"))
        {
            s = s.Replace("Left", "Left\n");
            s = s.Replace("Right", "Right\n");
        }
        else
        {
            s = s.Replace("Arrow", "");
        }
        s = s.Replace("Alpha", "");
        return s;
    }
    #endregion

    #region Util
    Text GetUITextFromString(string s)
    {
        return uiTextElement[s];
    }

    string Get_StringKey_Of_UiTextElement(Text text)
    {
        foreach(var item in uiTextElement)
        {
            //Debug.Log(item);
            if (text == item.Value)
            {
                return item.Key;
            }
        }
        Debug.Log("failed to find entry");
        return uiTextElement.FirstOrDefault(x => x.Value == text).Key;
    }
    #endregion
}
