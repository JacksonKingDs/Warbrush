using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text waveText;
    public Text[] percentageText;

    [Header("FG")]
    public Image[] p1Lives;
    public Image[] p2Lives;
    public Image[] p3Lives;
    public Image[] p4Lives;
    public Image[][] AllLives;

    [Header("BG")]
    public Image[] p1LivesBG;
    public Image[] p2LivesBG;
    public Image[] p3LivesBG;
    public Image[] p4LivesBG;
    public Image[][] AllLivesBG;

    public Color uiColor_combat;
    public Color uiColor_night;
    public Color uiColor_ocean;
    public Color uiColor_arcade;
    public Color uiColor_spooky;
    public Color uiColor_desert;
    public Color uiColor_space;
    public Color uiColor_campaign;

    public Color waveColor_arcade;
    public Color waveColor_spooky;
    public Color waveColor_space;

    //Class reference
    GM gm;
    FightSceneManager sceneM;
    SettingsAndPrefabRefs refs;

    //Color reference
    Color uiColor;
    Color[] tankColors = new Color[4];

    #region MonoBehaviour
    void Awake()
    {
        instance = this;
        AllLives = new Image[][] { p1Lives, p2Lives, p3Lives, p4Lives};
        AllLivesBG = new Image[][] { p1LivesBG, p2LivesBG, p3LivesBG, p4LivesBG };
    }

    void Start()
    {
        gm = GM.instance;
        refs = SettingsAndPrefabRefs.instance;
        sceneM = FightSceneManager.instance;

        tankColors = GM.pallet.Tank;

        switch (GM.gameMode)
        {
            case GameMode.PVP_Combat:
                uiColor = uiColor_combat;
                waveText.enabled = false;
                break;
            case GameMode.PVP_Night:
                uiColor = uiColor_night;
                waveText.enabled = false;
                break;
            case GameMode.PVP_OceanMist:
                uiColor = uiColor_ocean;
                waveText.enabled = false;
                break;
            case GameMode.PVP_Desert :
                uiColor = uiColor_desert;
                waveText.enabled = false;
                break;
            case GameMode.Coop_Arcade:
                uiColor = uiColor_arcade;
                StartCoroutine(DelayedRevealOfWaveText());
                waveText.color = waveColor_arcade;
                break;
            case GameMode.Coop_Torch:
                uiColor = uiColor_spooky;
                StartCoroutine(DelayedRevealOfWaveText());
                waveText.color = waveColor_spooky;
                break;
            case GameMode.Hanabi:
                uiColor = uiColor_space;
                waveText.enabled = false;
                //waveText.color = waveColor_space;
                break;
            case GameMode.Campaign:
                uiColor = uiColor_campaign;
                waveText.enabled = false;
                StartCoroutine(BriefDisplayOfCampaignLevel());
                break;
            default:
                break;
        }
        
        for (int i = 0; i < 4; i++)
        {
            //Hide Inactive UI
            if (gm.playerType[i] == PlayerTypes.INACTIVE)
            {
                foreach (Image img in AllLives[i])
                {
                    img.enabled = false;
                }
                foreach (Image img in AllLivesBG[i])
                {
                    img.enabled = false;
                }
                percentageText[i].enabled = false;
            }
            else
            {
                //Swap tank UI textures
                Sprite t = refs.GetTankModel_Sprite(gm.tankModelNames[i]);
                foreach (Image img in AllLives[i]) //Lives FG
                {
                    img.sprite = t;
                    img.color = uiColor;
                }

                foreach (Image img in AllLivesBG[i]) //Lives FG
                {
                    img.sprite = t;
                }
                percentageText[i].color = uiColor;
            }
        }
    }
    IEnumerator DelayedRevealOfWaveText()
    {
        yield return new WaitForSeconds(3f);
        waveText.enabled = true;
    }


    IEnumerator BriefDisplayOfCampaignLevel ()
    {
        yield return new WaitForSeconds(3f);
        waveText.enabled = true;
        waveText.color = Color.white;
        switch(GM.campaignMapIndex)
        {
            case 0: waveText.text = "1 - Easing In"; break;
            case 1: waveText.text = "2 - Potholes and Turrets"; break;
            case 2: waveText.text = "3 - Artillery"; break;
            case 3: waveText.text = "4 - Edge Warp Tactic"; break;
            case 4: waveText.text = "5 - Runway"; break;
            case 5: waveText.text = "6 - Take Out The Guards"; break;
            case 6: waveText.text = "7 - Spinners First"; break;
            case 7: waveText.text = "8 - Make Room"; break;
            case 8: waveText.text = "9 - On The move"; break;
            case 9: waveText.text = "10 - Shoot More"; break;

            case 10: waveText.text = "11: Vertical Warp"; break;
            case 11: waveText.text = "12: Just Run First, Don't Fight"; break;
            case 12: waveText.text = "13: Breather"; break;
            case 13: waveText.text = "14: Warp Away"; break;
            case 14: waveText.text = "15: Behind A Dead Body"; break;
            case 15: waveText.text = "16: Wait And Strike"; break;
            case 16: waveText.text = "17: Target Priority"; break;
            case 17: waveText.text = "18: Pressure Box"; break;
            case 18: waveText.text = "19: Run To Corner"; break;
            case 19: waveText.text = "20: Stay Centered"; break;

            case 20: waveText.text = "21: Warp Juggler"; break;
            case 21: waveText.text = "22: Just Run And Hide"; break;
            case 22: waveText.text = "23: Get Out"; break;
            case 23: waveText.text = "24: Stay Behind Sandbags"; break;
            case 24: waveText.text = "25: Ready Player One"; break;
            case 25: waveText.text = "26: Small Bullet"; break;
            case 26: waveText.text = "27: Sunflower"; break;
            case 27: waveText.text = "28: North Star"; break;
            case 28: waveText.text = "29: Snipe First"; break;
            case 29: waveText.text = "30: One Side At A Time"; break;

            case 30: waveText.text = "31: Knife God"; break;
            case 31: waveText.text = "32: Break Out And Run"; break;
            case 32: waveText.text = "33: Crossfire"; break;
            case 33: waveText.text = "34: Hell"; break;
            case 34: waveText.text = "35: Take Out A Cannon"; break;
            case 35: waveText.text = "36: Don't Attack First"; break;
            case 36: waveText.text = "37: Right Place Right Time"; break;
            case 37: waveText.text = "38: Skynet"; break;
            case 38: waveText.text = "39: Straight Up"; break;
            default: waveText.text = "40: Eye Of The Storm"; break;
        }
        yield return new WaitForSeconds(2f);
        waveText.enabled = false;
    }
    #endregion

    #region Public - Player UI
    public void UpdateScore (int index, float percentage)
    {
        int perc = (int)(percentage * 100f);
        if (perc >= 100)
        {
            FightSceneManager.instance.WonByPaint(index);
        }
        percentageText[index].text = perc + "%";
    }

    public void UpdateScore(int index, int kills)
    {
        percentageText[index].text = kills.ToString();
    }

    public void WinnerBeingOverTaken (int index)
    {
        if (GM.gameMode != GameMode.Hanabi)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != index)
                    percentageText[i].color = uiColor;
                else
                    percentageText[i].color = tankColors[i];
            }
        }
    }

    public void PlayerLostLife (int index, int life)
    {
        try
        {
            AllLives[index][life].enabled = false;
        }
        catch
        { }
        //catch (Exception e)
        //{
        //    //This occurs when a player is hit by 2 bullets in the same frame
        //    Debug.Log("Unable to disable player life. " + index + ", life: " + life + ". Exception: " + e);
        //}
    }
    #endregion
}

/*
  void ResetLives (int index) //This is not "HP", this is lives
    {
        //Reset life
        for (int j = 0; j < AllLives[index].Length; j++)
        {
            AllLives[index][j].enabled = true;
        }
    }
     */
