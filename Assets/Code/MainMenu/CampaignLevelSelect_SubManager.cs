using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CampaignLevelSelect_SubManager : MonoBehaviour
{
    public static CampaignLevelSelect_SubManager instance;

    public GameObject campaign_1;
    public GameObject campaign_2;
    public GameObject campaign_3;
    public GameObject campaign_4;
    public GameObject campaign_5;
    public GameObject campaign_6;
    public GameObject campaign_7;
    public GameObject campaign_8;
    public GameObject campaign_9;
    public GameObject campaign_10;
    public GameObject campaign_11;
    public GameObject campaign_12;
    public GameObject campaign_13;
    public GameObject campaign_14;
    public GameObject campaign_15;
    public GameObject campaign_16;
    public GameObject campaign_17;
    public GameObject campaign_18;
    public GameObject campaign_19;
    public GameObject campaign_20;
    public GameObject campaign_21;
    public GameObject campaign_22;
    public GameObject campaign_23;
    public GameObject campaign_24;
    public GameObject campaign_25;
    public GameObject campaign_26;
    public GameObject campaign_27;
    public GameObject campaign_28;
    public GameObject campaign_29;
    public GameObject campaign_30;
    public GameObject campaign_31;
    public GameObject campaign_32;
    public GameObject campaign_33;
    public GameObject campaign_34;
    public GameObject campaign_35;
    public GameObject campaign_36;
    public GameObject campaign_37;
    public GameObject campaign_38;
    public GameObject campaign_39;
    public GameObject campaign_40;

    public RectTransform selectionRing;
    public Sprite locked;

    List<GameObject> AllCampaignButtons;

    ScMenu_UIManager sceneM;
    GameObject lastSelect_mainMenu;
    EventSystem eventSystem;
    InputManager inputM;
    AudioManager audioM;

    private void Awake()
    {
        instance = this;

        AllCampaignButtons = new List<GameObject>()
        {
            campaign_1, campaign_2, campaign_3, campaign_4,campaign_5,campaign_6,campaign_7,campaign_8,campaign_9,campaign_10,
            campaign_11, campaign_12, campaign_13, campaign_14,campaign_15,campaign_16,campaign_17,campaign_18,campaign_19,campaign_20,
            campaign_21, campaign_22, campaign_23, campaign_24,campaign_25,campaign_26,campaign_27,campaign_28,campaign_29,campaign_30,
            campaign_31, campaign_32, campaign_33, campaign_34,campaign_35,campaign_36,campaign_37,campaign_38,campaign_39,campaign_40
        };

        lastSelect_mainMenu = campaign_1;
    }

    void Start()
    {
        eventSystem = EventSystem.current;
        sceneM = ScMenu_UIManager.instance;
        inputM = InputManager.Instance;
        audioM = AudioManager.instance;
        UpdateLockImages();
    }

    void UpdateLockImages ()
    {
        for (int i = AllCampaignButtons.Count - 1; i > GM.unlockedCampaignIndex; i--)
        {
            AllCampaignButtons[i].GetComponent<Image>().sprite = locked;
            AllCampaignButtons[i].GetComponent<Button>().interactable = false;
            AllCampaignButtons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetButtonSelection ()
    {
        eventSystem.SetSelectedGameObject(campaign_1);
        campaign_1.GetComponent<Selectable>().OnPointerEnter(null);
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 20, 2000, 20), "current button: " + eventSystem.currentSelectedGameObject);
    //}

    public void OnUpdate()
    {
        //====================== 1 - 10
        GameObject go = eventSystem.currentSelectedGameObject;

        if (go == null || !go.GetComponent<Button>() || go.GetComponent<Button>().interactable == false) //If selected no button. 
        {
            if (lastSelect_mainMenu == null || lastSelect_mainMenu.GetComponent<Button>().interactable == false)
            {
                HighlightButton(campaign_1);
                
            }
            else
            {
                HighlightButton(lastSelect_mainMenu);
            }
        }
        // 1
        else if (eventSystem.currentSelectedGameObject == campaign_1)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_31);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_11);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_10);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_2);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(0);
        }
        // 2
        else if (eventSystem.currentSelectedGameObject == campaign_2)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_32);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_12);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_1);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_3);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(1);
        }
        // 3
        else if (eventSystem.currentSelectedGameObject == campaign_3)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_33);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_13);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_2);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_4);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(2);
        }
        // 4
        else if (eventSystem.currentSelectedGameObject == campaign_4)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_34);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_14);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_3);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_5);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(3);
        }
        // 5
        else if (eventSystem.currentSelectedGameObject == campaign_5)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_35);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_15);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_4);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_6);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(4);
        }
        // 6
        else if (eventSystem.currentSelectedGameObject == campaign_6)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_36);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_16);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_5);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_7);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(5);
        }
        // 7
        else if (eventSystem.currentSelectedGameObject == campaign_7)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_37);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_17);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_6);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_8);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(6);
        }
        // 8
        else if (eventSystem.currentSelectedGameObject == campaign_8)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_38);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_18);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_7);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_9);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(7);
        }
        // 9
        else if (eventSystem.currentSelectedGameObject == campaign_9)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_39);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_19);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_8);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_10);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(8);
        }
        // 10
        else if (eventSystem.currentSelectedGameObject == campaign_10)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_40);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_20);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_9);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_1);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(9);
        }
        //====================== 11 - 20 =========================================
         // 11
        else if (eventSystem.currentSelectedGameObject == campaign_11)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_1);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_21);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_20);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_12);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(10);
        }
        // 12
        else if (eventSystem.currentSelectedGameObject == campaign_12)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_2);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_22);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_11);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_13);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(11);
        }
        // 13
        else if (eventSystem.currentSelectedGameObject == campaign_13)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_3);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_23);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_12);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_14);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(12);
        }
        // 14
        else if (eventSystem.currentSelectedGameObject == campaign_14)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_4);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_24);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_13);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_15);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(13);
        }
        // 15
        else if (eventSystem.currentSelectedGameObject == campaign_15)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_5);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_25);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_14);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_16);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(14);
        }
        // 16
        else if (eventSystem.currentSelectedGameObject == campaign_16)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_6);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_26);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_15);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_17);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(15);
        }
        // 17
        else if (eventSystem.currentSelectedGameObject == campaign_17)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_7);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_27);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_16);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_18);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(16);
        }
        // 18
        else if (eventSystem.currentSelectedGameObject == campaign_18)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_8);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_28);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_17);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_19);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(17);
        }
        // 19
        else if (eventSystem.currentSelectedGameObject == campaign_19)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_9);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_29);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_18);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_20);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(18);
        }
        // 20
        else if (eventSystem.currentSelectedGameObject == campaign_20)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_10);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_30);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_19);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_11);

            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(19);
        }


        //====================== 21 - 30
        else if (eventSystem.currentSelectedGameObject == campaign_21)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_11);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_31);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_30);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_22);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(20);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_22)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_12);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_32);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_21);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_23);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(21);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_23)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_13);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_33);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_22);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_24);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(22);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_24)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_14);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_34);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_23);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_25);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(23);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_25)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_15);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_35);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_24);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_26);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(24);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_26)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_16);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_36);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_25);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_27);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(25);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_27)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_17);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_37);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_26);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_28);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(26);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_28)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_18);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_38);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_27);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_29);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(27);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_29)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_19);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_39);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_28);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_30);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(28);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_30)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_20);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_40);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_29);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_21);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(29);
        }
        //====================== 31 - 40
        else if (eventSystem.currentSelectedGameObject == campaign_31)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_21);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_1);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_40);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_32);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(30);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_32)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_22);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_2);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_31);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_33);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(31);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_33)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_23);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_3);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_32);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_34);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(32);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_34)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_24);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_4);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_33);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_35);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(33);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_35)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_25);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_5);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_34);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_36);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(34);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_36)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_26);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_6);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_35);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_37);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(35);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_37)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_27);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_7);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_36);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_38);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(36);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_38)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_28);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_8);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_37);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_39);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(37);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_39)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_29);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_9);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_38);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_40);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(38);
        }
        else if (eventSystem.currentSelectedGameObject == campaign_40)
        {
            if (inputM.AnyUp_Down)      HighlightButton(campaign_30);
            if (inputM.AnyDown_Down)    HighlightButton(campaign_10);
            if (inputM.AnyLeft_Down)    HighlightButton(campaign_39);
            if (inputM.AnyRight_Down)   HighlightButton(campaign_31);
            if (inputM.AnyA_Down || inputM.AnyStart_Down) StartCampaignIndex(39);
        }
    }

    void HighlightButton(GameObject button)
    {
        if (button.GetComponent<Button>().interactable)
        {
            selectionRing.position = button.GetComponent<RectTransform>().position;
            eventSystem.SetSelectedGameObject(button);
            //campaign_1.GetComponent<Selectable>().OnPointerEnter(null);
            lastSelect_mainMenu = button;
            audioM.Spawn_UI_Confirm();
        }
    }


    public void StartCampaignIndex (int index)
    {
        Debug.Log("StartCampaignIndex " + index);
        GM.campaignMapIndex = index;
        lastSelect_mainMenu = GetActiveLevelButton();
        sceneM.SelectedCampaignLevel(index);
    }

    GameObject GetActiveLevelButton ()
    {
        switch (GM.campaignMapIndex)
        {
            case 0: return campaign_1;
            case 1: return campaign_2;
            case 2: return campaign_3;
            case 3: return campaign_4;
            case 4: return campaign_5;
            case 5: return campaign_6;
            case 6: return campaign_7;
            case 7: return campaign_8;
            case 8: return campaign_9;
            case 9: return campaign_10;

            case 10: return campaign_11;
            case 11: return campaign_12;
            case 12: return campaign_13;
            case 13: return campaign_14;
            case 14: return campaign_15;
            case 15: return campaign_16;
            case 16: return campaign_17;
            case 17: return campaign_18;
            case 18: return campaign_19;
            case 19: return campaign_20;

            case 20: return campaign_21;
            case 21: return campaign_22;
            case 22: return campaign_23;
            case 23: return campaign_24;
            case 24: return campaign_25;
            case 25: return campaign_26;
            case 26: return campaign_27;
            case 27: return campaign_28;
            case 28: return campaign_29;
            case 29: return campaign_20;

            case 30: return campaign_31;
            case 31: return campaign_32;
            case 32: return campaign_33;
            case 33: return campaign_34;
            case 34: return campaign_35;
            case 35: return campaign_36;
            case 36: return campaign_37;
            case 37: return campaign_38;
            case 38: return campaign_39;
            default:
            case 39: return campaign_40;
        }
    }
}
