using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class FightSceneManager : MonoBehaviour
{    
    public static FightSceneManager instance;
    public static FightSceneStates sceneState;

    public CanvasGroupFader blackFader;
    public AudioSource winMusic;
    public AudioSource failMusic;

    [Header("Tanks PF")]
    public GameObject Pf_Tank_RIFLE;
    public GameObject Pf_Tank_SHOTGUN;
    public GameObject Pf_Tank_GRENADE;
    public GameObject Pf_Tank_BOUNCER;
    public GameObject Pf_Tank_LANCER;

    [Header("Scene objects")]    
    public Transform[] respawnPoints_Brawl;
    public Transform[] respawnPoints_Coop;
    public Transform[] respawnPoints_Beach;

    public Masterpieced winscreen;

    public List<CampaignLevelInfo> levelInfos;
    [HideInInspector] public CampaignLevelInfo activeLevelInfo;

    [Header("LEVEL SETTING")]
    public float screenShakeLength = 0.06f;
    public float camShakeMagnitude = 0.2f;
    public bool allowDummies = false;


    //[Header("Player objects")]
    [HideInInspector] public TankControllerBase[] tankManagers;
    [HideInInspector] public Transform[] tanksTrans;
    [HideInInspector] public List<int> validPlayers; //Tanks that are AI or REAL PLAYERS and also not INACTIVE
    [HideInInspector] public Transform[] respawnPoints;

    public static int[] attacks;
    public static int[] landed;
    public static int[] kills;

    //Class references
    GM gm;
    InputManager inputManager;
    UIManager uiManager;
    AudioManager audioManager;
    BGTextureManager BGManager;
    EnemyManager enemyM;
    bool inSceneTransition = false;

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 80, 200, 20), "gameEnding: " + gameEnded);
    //    //GUI.Label(new Rect(20, 100, 200, 20), "readl enemyCount: " + enemyM.Enemies.Count);
    //    //GUI.Label(new Rect(20, 120, 200, 20), "GM.gameMode: " + GM.gameMode);
    //}

    #region MonoBehavior
    void Awake ()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        tankManagers = new TankControllerBase[4];
        tanksTrans = new Transform[4];
        attacks = new int[] { 0, 0, 0, 0 };
        landed = new int[] { 0, 0, 0, 0 };
        kills = new int[] { 0, 0, 0, 0 };

        //Reference classes
        gm = GM.instance;
        inputManager = InputManager.Instance;
        uiManager = UIManager.instance;
        audioManager = AudioManager.instance;
        BGManager = BGTextureManager.instance;
        enemyM = EnemyManager.instance;

        //Scene initialization
        BGManager.SceneInitialization();

        switch (GM.gameMode)
        {
            case GameMode.PVP_Combat:
            case GameMode.PVP_Night:
            case GameMode.PVP_Desert:
            case GameMode.PVP_OceanMist:
                respawnPoints = respawnPoints_Brawl;
                break;
            case GameMode.Coop_Arcade:
            case GameMode.Coop_Torch:
                respawnPoints = respawnPoints_Coop;
                break;
            case GameMode.Campaign:
                activeLevelInfo = levelInfos[GM.campaignMapIndex];
                break;
            //case GameMode.Coop_Beach:
            //    respawnPoints = respawnPoints_Beach;
            //    break;
            default:
                break;
        }

        //Spawn tanks
        if (GM.gameMode == GameMode.Campaign)
        {
            respawnPoints = activeLevelInfo.respawnPoints;
        }

        for (int i = 0; i < 4; i++)
        {
            TankModelNames m = gm.tankModelNames[i];
            tankManagers[i] = Instantiate(GetTankPfFromModel(m), respawnPoints[i].position, respawnPoints[i].rotation).GetComponent<TankControllerBase>();
            tanksTrans[i] = tankManagers[i].transform;
        }

        //Activate tanks
        for (int i = 0; i < 4; i++)
        {
            tankManagers[i].SceneInitialization(i);
        }

        validPlayers = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (gm.playerType[i] != PlayerTypes.INACTIVE)
            {
                validPlayers.Add(i);
            }
        }

        //Start game
        sceneState = FightSceneStates.DISABLED;
        yield return new WaitForSeconds(2f);
        sceneState = FightSceneStates.PLAY;
    }

    void Update()
    {
    }
    #endregion

    #region End round
    public void LevelCompleted (bool campainWon = true)
    {
        Debug.Log("gameEnding? " + inSceneTransition);
        sceneState = FightSceneStates.DISABLED;
        
        //blackFader.FadeIn(() => { SceneManager.LoadScene(0); });

        switch (GM.gameMode)
        {
            case GameMode.PVP_Combat:
                GM.combatMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.PVP_Night:
                GM.nightMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.Coop_Torch:
                GM.spookyMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.PVP_Desert:
                GM.desertMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.Campaign:
                StartCoroutine(FadeToCampaignLevel(campainWon));
                break;
                //case GameMode.PVP_OceanMist:
                //    break;
                //case GameMode.Coop_Arcade:
                //    break;
           default:
                StartCoroutine(FadeToMenu());
                break;
        }
    }

    public void PauseMenuGoToMainMenu()
    {
        sceneState = FightSceneStates.DISABLED;

        //blackFader.FadeIn(() => { SceneManager.LoadScene(0); });

        switch (GM.gameMode)
        {
            case GameMode.PVP_Combat:
                GM.combatMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.PVP_Night:
                GM.nightMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.Coop_Torch:
                GM.spookyMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.PVP_Desert:
                GM.desertMapIndex++;
                StartCoroutine(FadeToMenu());
                break;
            case GameMode.Campaign:
                StartCoroutine(FadeToMenu());
                break;
            //case GameMode.PVP_OceanMist:
            //    break;
            //case GameMode.Coop_Arcade:
            //    break;
            default:
                StartCoroutine(FadeToMenu());
                break;
        }
    }

    IEnumerator FadeToMenu ()
    {
        if (!inSceneTransition)
        {
            inSceneTransition = true;

            blackFader.FadeIn();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator FadeToCampaignLevel(bool levelWon)
    {
        if (!inSceneTransition)
        {
            Debug.Log("level won" + levelWon);
            if (levelWon)
            {
                ++GM.campaignMapIndex;
            }

            if (GM.campaignMapIndex > 39)
            {
                //Debug.Log("Reset campaign index: " + GM.campaignMapIndex);
                GM.campaignMapIndex = 0;
                StartCoroutine(FadeToMenu());
            }
            else
            {
                inSceneTransition = true;
                blackFader.FadeIn();
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        
    }

    public void WonByPaint (int index)
    {
        if (!inSceneTransition)
        {
            WinnerIsFoundForRound(index);
        }
    }


    public void CheckWinner(int playerIndex, int killerIndex, bool wonByPaint = false)
    {
        //print("loserPlayerIndex: " + loserPlayerIndex);

        if (!inSceneTransition) //If there is no winner yet...
        {
            if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
            {
                //Check if all players are INACTIVE and lives <= 0
                foreach (var tank in tankManagers)
                {
                    if (tank.activeState != VersusActorStates
                        .INACTIVE || tank.lives >= 1)
                    {
                        //Debug.Log("tank.lives" + tank.lives + "tank.activeState" + tank.activeState + " " + "");
                        return;
                    }
                }

                //gameEnding = true;
                WinnerIsFoundForRound(playerIndex);
            }
            else if (GM.gameMode == GameMode.Campaign)
            {
                //Check if all players are dead
                foreach (var tank in tankManagers)
                {
                    if (tank.activeState != VersusActorStates
                        .INACTIVE || tank.lives >= 1)
                    {
                        //Debug.Log("tank.lives" + tank.lives + "tank.activeState" + tank.activeState + " " + "");
                        return;
                    }
                }
                //gameEnding = true;

                winMusic.Stop();
                failMusic.Play();
                sceneState = FightSceneStates.GAME_END_STANDBY;
                winscreen.gameObject.SetActive(true);
                winscreen.CampaignLost(playerIndex);
            }
            else// if (GM.gameMode == GameMode.PVP_Combat)
            {
                if (!wonByPaint)
                {
                    int playersHasLife = 0;
                    int alivePlayerIndex = killerIndex;

                    //Check if all players are INACTIVE and lives <= 0
                    for (int i = 0; i < 4; i++)
                    {
                        if (tankManagers[i].activeState != VersusActorStates
                           .INACTIVE || tankManagers[i].lives >= 1)
                        {
                            playersHasLife++;
                            alivePlayerIndex = i;
                        }
                    }

                    if (playersHasLife <= 1)
                    {
                        if (alivePlayerIndex < 0 || alivePlayerIndex > 3)
                        {
                            Debug.Log("Won by combat: " + playerIndex);
                            WinnerIsFoundForRound(playerIndex);
                        }
                        else
                        {
                            Debug.Log("Won by combat. Killer is not a player. Alive: : " + alivePlayerIndex + ". Dead: " + playerIndex);
                            WinnerIsFoundForRound(alivePlayerIndex);
                        }
                    }
                }
                else
                {
                    Debug.Log("Won by paint" + playerIndex);
                    //print("Won by paint. Player " + playerIndex);
                    WinnerIsFoundForRound(playerIndex);
                }
            }
        }
    }

    bool playingWinnerScreen = false;
    void WinnerIsFoundForRound(int winnerIndex) //Record and give the winner score point
    {
        if (!playingWinnerScreen)
        {
            playingWinnerScreen = true;
            //visualFeedback.ShowFinalScoreboard();
            winMusic.Play();
            sceneState = FightSceneStates.GAME_END_STANDBY;
            winscreen.gameObject.SetActive(true);
            winscreen.StartExpand(winnerIndex);
        }
    }
    #endregion

    #region Actor killed
    public void AllEnemiesDead(int winnerIndex)
    {
        if (!playingWinnerScreen)
        {
            playingWinnerScreen = true;
            winMusic.Play();
            sceneState = FightSceneStates.GAME_END_STANDBY;
            GM.CampaignWon();

            winscreen.CampaignWon(winnerIndex);
        }
    }

    public static void AddKillScore(int index)
    {
        kills[index]++;
    }
    #endregion

    #region Util
    public void TankIndexActivate(int index, bool activate)
    {
        if (activate && !validPlayers.Contains(index))
        {
            validPlayers.Add(index);
        }
        else if (!activate && validPlayers.Contains(index))
        {
            validPlayers.Remove(index);
        }
    }

    GameObject GetTankPfFromModel(TankModelNames model)
    {
        switch (model)
        {
            case TankModelNames.RIFLE:
                return Pf_Tank_RIFLE;
            case TankModelNames.SHOTGUN:
                return Pf_Tank_SHOTGUN;
            case TankModelNames.GRENADE:
                return Pf_Tank_GRENADE;
            case TankModelNames.BOUNCER:
                return Pf_Tank_BOUNCER;
            case TankModelNames.SEEKER:
            default:
                return Pf_Tank_LANCER;
        }
    }
    #endregion
}

public enum FightSceneStates
{
    DISABLED,
    PLAY,
    PAUSED,
    GAME_END_STANDBY
}