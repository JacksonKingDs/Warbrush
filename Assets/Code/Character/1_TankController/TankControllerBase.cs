using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankControllerBase : MonoBehaviour
{
    #region Fields
    [Header("Tank GO components in scene")]
    
    public SpriteRenderer spriteRend;
    public Animator anim;
    public List<Transform> paintPoints;
    public List<Transform> paintOncePoints;
    public Transform[] skidPoints; //For drawing skid marks on the ground
    public List<Transform> trailPointsBack; //OceanTrail
    public List<Transform> trailPointsFront; //OceanTrail
    public SpriteRenderer border;
    [HideInInspector] public TankTaunter tauntM;

    public ShootChargingEffect chargingPfx;
    //All of these needs to be prefabs
    //Muzzle comes with Points
    //FrontCharging Pfx , backChargingPfx
    //public ParticleSystem chargingPfx; //For playing and stopping pfx

    [HideInInspector] public Vector3 pos;
    [HideInInspector] public Transform trans;
    [HideInInspector] public Rigidbody2D rb; //Gameobject components
    [HideInInspector] public PolygonCollider2D playerCol;
    
    [Header("Tank Data")]
    [HideInInspector] public int index; //Tank index (0~3)
    public TankModelNames modelName;

    [HideInInspector] public Transform respawnPoint;
    [HideInInspector] public PlayerTypes playerType;
    [HideInInspector] public ControlScheme control;
    [HideInInspector] public VersusActorStates activeState = VersusActorStates.INACTIVE;
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public Vector3 knockbackDir;
    [HideInInspector] public float knockbackTimer;
    [HideInInspector] public Color tankColor;
    [HideInInspector] public Vector3 storedVel;

    //Space stage
    [HideInInspector] public SpaceStar recentStar;

    public AudioSource audio_move;
    public AudioSource audio_rot;

    //STATE AND BEHAVIOR MODULES
    protected TankStateBase tankState_Normal;
    protected TankStateBase tankState_Knockback;
    protected TankStateBase tankState_Ability;
    protected TankStateBase tankState_InitialStandby;
    protected TankStateBase tankState_RespawnStandby;
    protected TankStateBase tankState_InactiveHidden;

    //Tank state
    [HideInInspector] public int lives;
    protected TankStateBase activeStateClass;
    protected int HP = MAX_HP;
    protected bool invulnerable = false;
    int stutterCounter = 0;
    [HideInInspector] public float moveSpeed;
    

    bool AIControlled = false;
    AIControlModule aiController;

    //Class references
    protected GM gm;
    protected InputManager inputM;
    protected SettingsAndPrefabRefs refs;
    protected UIManager uiManager;
    protected BGTextureManager BG_Painter;
    protected FightSceneManager sceneManager;
    protected Camerashake camShake;
    protected AudioManager audioM;

    //General settings
    public const int MAX_HP = 3;
    public const int MAX_LIVES = 3;
    public const float RECOIL_DUR = 0.2f;
    public const float KNOCKBACK_DUR = 0.5f;
    public const float RECOIL_FORCE = 1f;
    public const float KNOCKBACK_FORCE = 2f;
    public const int stutterSkidAmount = 12;
    bool canPaint = true;

    //Optimization cache
    bool isOceanMode = false;
    bool isCombatMode = false;
    bool isSpookyMode = false;
    bool isSpaceMode = false;
    GameMode gameMode;

    #endregion
    [HideInInspector] public int animState_blink;
    [HideInInspector] public int animState_default;

    bool inClouds = false;
    OceanCloud curCloud;
    float cloudFadeSpeed = 5f;

    #region Game Start Initialization
    public void SceneInitialization(int index)
    {
        this.index = index;

        //Ref classes
        gm = GM.instance;
        inputM = InputManager.Instance;
        refs = SettingsAndPrefabRefs.instance;
        uiManager = UIManager.instance;
        BG_Painter = BGTextureManager.instance;
        sceneManager = FightSceneManager.instance;
        camShake = Camerashake.instance;
        audioM = AudioManager.instance;

        //Ref components
        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<PolygonCollider2D>();
        audio_move.enabled = false;

        //Initialize by referencing game settings
        tankColor = GM.pallet.Tank[index];
        moveSpeed = SettingsAndPrefabRefs.TankStats[modelName].MoveSpeed;
        //Color b = GM.pallet.BG;
        //b.a = 1f;

        spriteRend.color = tankColor;
        respawnPoint = sceneManager.respawnPoints[index];

        //Disable tank if this player slot is not being played
        playerType = gm.playerType[index];
        if (playerType == PlayerTypes.INACTIVE)
        {
            //This will kill the script and the gameObject
            gameObject.SetActive(false);
            lives = 0;
            activeState = VersusActorStates.INACTIVE;
            //Debug.Log("[P" + index + "] is inactive and deactivated");
            return;
        }
        //If AI mode, then use an AI script to control inputs
        else
        {
            //Other initializations
            animState_default = Animator.StringToHash("Default");
            animState_blink = Animator.StringToHash("Blink");

            lives = MAX_LIVES;
            gameMode = GM.gameMode;
            isOceanMode = GM.gameMode == GameMode.PVP_OceanMist;
            isCombatMode = GM.gameMode == GameMode.PVP_Combat;
            isSpookyMode = GM.gameMode == GameMode.Coop_Torch;
            isSpaceMode = GM.gameMode == GameMode.Hanabi;
            canSlide = isSpaceMode;

            //Taunt manager
            tauntM = Instantiate(refs.pf_tankTaunter, Vector3.zero, Quaternion.identity).GetComponent<TankTaunter>();
            tauntM.Initialize(trans);

            if (!isOceanMode && !isSpaceMode)
            {
                BG_Painter.PaintTankFGPoints(paintOncePoints, index);
                BG_Painter.PaintTankFGPoints(paintPoints, index);
            }

            if (isSpaceMode)
            {
                storedVel = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)) * trans.up * RECOIL_FORCE * 0.2f;
            }
            else if (isSpookyMode)
            {
                border.color = new Color(0f, 0f, 0f, 1f);
            }
            else
            {
                border.color = new Color(1f, 1f, 1f, 0.1f);
            }

            if (playerType == PlayerTypes.AI)
            {
                control = new ControlScheme();
                AIControlled = true;
                aiController = new AIControlModule(this);
            }
            else if (playerType == PlayerTypes.REAL_PERSON)
            {
                control = inputM.playerInputs[index];
            }

            //Initialize states
            tankState_InitialStandby = new TankStateInitialStandby(this);
            tankState_Normal = new TankState_Generic_MoveAndShoot(this);
            tankState_Knockback = new TankStateKnockback(this);
            tankState_RespawnStandby = new TankStateStandby(this);
            tankState_InactiveHidden = new TankStateInactiveHidden(this);

            activeState = VersusActorStates.INITIAL_STANDBY;
            activeStateClass = tankState_InitialStandby;

            StartCoroutine(InitialSpawn());
        }
    }
    #endregion

    #region Update
    void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            camShake.HitPause(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            camShake.HitPause(false);
        }

        if (FightSceneManager.sceneState != FightSceneStates.PAUSED)
        {
            //Various Updates
            //AI controller
            if (AIControlled)
                aiController.DoUpdate();

            //Active class
            activeStateClass.OnUpdate();

            //Slide switching
            if (antiStoppageTimer > 0f)
            {
                antiStoppageTimer -= Time.deltaTime;
            }

            //Paint stutter or normal
            if (canPaint)
            {
                //Paint BG and FG when not in these modes
                if (!isOceanMode && !isSpaceMode) 
                {
                    //Paint stutter
                    if (HP == 1) 
                    {
                        if (stutterCounter <= 0)
                        {
                            foreach (var p in skidPoints)
                            {
                                BG_Painter.PaintSkid(p.position, index);
                            }
                            stutterCounter = stutterSkidAmount;
                        }
                        else
                        {
                            stutterCounter--;
                        }
                    }
                    //Paint skid marks
                    else if (HP == 2) 
                    {
                        foreach (var p in skidPoints)
                        {
                            BG_Painter.PaintSkid(p.position, index);
                        }
                    }
                    //FUll hp = normal
                    else
                    {
                        BG_Painter.PaintTankFGPoints(paintPoints, index);
                    }
                }

                //Spooky mode fog
                if (isSpookyMode)
                { 
                    BG_Painter.Tank_ClearSpookyFog(trans.position);
                }
                //Space mode glitter
                //else if (isSpaceMode)
                //{
                //    //rb.velocity.sqrMagnitude 
                //    if (rb.velocity.sqrMagnitude  > 0.6f)
                //    {
                //        refs.SpawnSpaceDustRandomized(trans.position, index);
                //    }
                //}
            }
        }
        if (!invulnerable)
        {
            if (isOceanMode)
            {
                Color tankCol = spriteRend.color;
                Color borderCol = border.color;
                float diff = Time.deltaTime * cloudFadeSpeed;

                if (inClouds)
                {
                    if (tankCol.a > 0f)
                    {
                        tankCol.a -= diff;
                        spriteRend.color = tankCol;
                    }

                    if (borderCol.a > 0f)
                    {
                        borderCol.a -= diff;
                        border.color = tankCol;
                    }
                }
                else if (!inClouds && tankCol.a < 1f)
                {
                    if (tankCol.a < 1f)
                    {
                        tankCol.a += diff;
                        spriteRend.color = tankCol;
                    }

                    if (borderCol.a < 0.1f)
                    {
                        borderCol.a += diff;
                        border.color = tankCol;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (FightSceneManager.sceneState != FightSceneStates.PAUSED)
        {
            activeStateClass.OnFixedUpdate();
        }
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 20, 200, 20), "activeStateClass: " + activeStateClass);
    //    GUI.Label(new Rect(20, 40, 200, 20), "Control: " + control + ", index: " + index);
    //    GUI.Label(new Rect(20, 60, 200, 20), "MoveX: " + inputM.playerInputs[index].MoveX);
    //}
    #endregion

    #region State Transitions
    void GoToState(VersusActorStates newState)
    {
        if (activeState != newState)
        {
            activeState = newState;
            activeStateClass.OnStateExit();
        }

        switch (newState)
        {
            case VersusActorStates.RESPAWN_STANDBY:
                activeStateClass = tankState_RespawnStandby;
                //sceneManager.TankIndexActivate(index, true);
                break;
            case VersusActorStates.INITIAL_STANDBY:
                activeStateClass = tankState_InitialStandby;
                //sceneManager.TankIndexActivate(index, true);
                break;
            case VersusActorStates.NORMAL:
                activeStateClass = tankState_Normal;
                break;
            case VersusActorStates.ABILITY:
                activeStateClass = tankState_Ability;
                break;
            case VersusActorStates.KNOCKBACK:
                activeStateClass = tankState_Knockback;
                break;
            case VersusActorStates.INACTIVE:
            default:
                sceneManager.TankIndexActivate(index, false);
                activeStateClass = tankState_InactiveHidden;
                break;
        }

        activeStateClass.OnStateEntry();
    }
    #endregion

    #region Public
    public void DoRecoil (bool backwardRecoil = true)
    {
        knockbackForce = RECOIL_FORCE;
        knockbackDir = backwardRecoil ? -trans.up : trans.up;
        knockbackTimer = RECOIL_DUR;
        GoToState(VersusActorStates.KNOCKBACK);
    }

    public void DoAbility ()
    {
        GoToState(VersusActorStates.ABILITY);
    }

    public void GoToNormalState ()
    {
        GoToState(VersusActorStates.NORMAL);
    }

    public void SpawnOceanTrail (int strength)
    {

    }

    public void HideInCloud(OceanCloud curCloud)
    {
        this.curCloud = curCloud;
        inClouds = true;
    }

    public void RevealFromCloud()
    {
        curCloud = null;
        inClouds = false;
    }

    [HideInInspector] public bool canSlide;
    public void DisableSlide ()
    {
        if (antiStoppageTimer <= 0f)
        {
            antiStoppageTimer = 0.2f;
            canSlide = false;
        }
    }
    public void EnableSlide ()
    {
        canSlide = true;
        antiStoppageTimer = 0.3f;
        storedVel = rb.velocity.normalized * moveSpeed;
        //storedVel = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)) * trans.up * RECOIL_FORCE * 0.5f;
    }
    float antiStoppageTimer = 0f;
    #endregion

    #region Public: Collision hit > hp check > respawn  
    bool dead = false;
    public virtual void GetsHitByAttack(Vector3 bulletPos, int enemyIndex) //Called by the bullet that hits this
    {
        if (invulnerable)
            return;

        //CALCULATE KNOCK BACK DIR
        knockbackDir = (trans.position - bulletPos).normalized;
        
        //HEALTH CHECKS
        //Lose hp
        HP -= 1;

        //At 0 hp ...
        if (HP <= 0)
        {
            if (!dead)
            {
                dead = true; //This prevents spawning multiple tank wrecks in the same frame

                //Spawn dead tank prefab
                GameObject deadtank = GameObject.Instantiate(refs.GetDeadTank(modelName), trans.position, trans.rotation) as GameObject;
                deadtank.GetComponent<DeadTank>().KnockBack(knockbackDir);

                if (enemyIndex >= 0 && enemyIndex < 4)
                {
                    FightSceneManager.AddKillScore(enemyIndex);
                }

                camShake.HitPause(true);
                

                //GameObject.Instantiate(refs.Pfx_HitSparkB_Long, trans.position, Quaternion.identity);
                //audioM.Spawn_explode3();
                lives--;
                uiManager.PlayerLostLife(index, lives);
                //At 0 lives, player is permanently dead.
                if (lives <= 0)
                {
                    //audioM.PlayAndGetRandomAudio(RandomAudioTypes.DEATH);
                    //gm.playerType[index] = PlayerTypes.INACTIVE;

                    GoToState(VersusActorStates.INACTIVE);
                    gameObject.SetActive(false);
                    sceneManager.CheckWinner(index, enemyIndex);
                    //Debug.Log("Player " + index + " defeated");
                }
                else
                {
                    StartCoroutine(FaintThenRespawn());
                }
            }
        }
        //If more than 0 hp...
        else 
        {
            //GameObject.Instantiate(refs.Pfx_HitSparkB_Shorter, trans.position, Quaternion.identity);
            knockbackTimer = KNOCKBACK_DUR;
            knockbackForce = KNOCKBACK_FORCE;

            GoToState(VersusActorStates.KNOCKBACK);

            camShake.HitPause(false);
            //Then display onhit feedbacks
            StartCoroutine(GetHitBlink());
        }
    }


    public virtual void GetsHitByAttackNoDmg(Vector3 bulletPos) //Called by the bullet that hits this
    {
        if (invulnerable)
            return;

        //CALCULATE KNOCK BACK DIR
        knockbackDir = (trans.position - bulletPos).normalized;

        knockbackTimer = KNOCKBACK_DUR;
        knockbackForce = KNOCKBACK_FORCE;

        GoToState(VersusActorStates.KNOCKBACK);

        //Then display onhit feedbacks
        StartCoroutine(GetHitBlink());
    }


    IEnumerator FaintThenRespawn()
    {

        //Disable and hide tank
        invulnerable = true;
        GoToState(VersusActorStates.INACTIVE);
        spriteRend.color = Color.clear;
        trans.position =  respawnPoint.position;
        trans.rotation = respawnPoint.rotation;
        playerCol.enabled = false;
        canPaint = false;
        recentStar = null;
        border.color = Color.clear;

        yield return new WaitForSeconds(2f);
        Debug.Log("spawning start");        StartCoroutine(Respawn());
    }

    IEnumerator InitialSpawn()
    {
        canPaint = true;
        playerCol.enabled = true;
        dead = false;
        GoToState(VersusActorStates.INITIAL_STANDBY);
        //Respawn                
        HP = MAX_HP;
        if (isSpaceMode)
        {
            border.color = Color.white;
            storedVel = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)) * trans.up * RECOIL_FORCE * 0.2f;
        }

        //Do blinks
        bool isWhite = false;
        for (int i = 0; i < 15; i++)
        {
            if (isWhite)
            {
                spriteRend.color = tankColor;
            }
            else
            {
                spriteRend.color = Color.white;
                //spriteRend.color = Color.clear;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }
        ResetTankColor();

        yield return new WaitForSeconds(1.29f);
        invulnerable = false;
        sceneManager.TankIndexActivate(index, true);
        GoToState(VersusActorStates.NORMAL);
    }

    IEnumerator Respawn()
    {
        canPaint = true;
        playerCol.enabled = true;
        dead = false;
        GoToState(VersusActorStates.RESPAWN_STANDBY);
        //Respawn                
        HP = MAX_HP;
        if (isSpaceMode)
        {
            border.color = Color.white;
            storedVel = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)) * trans.up * RECOIL_FORCE * 0.2f;
        }

        //Do blinks
        bool isWhite = false;
        for (int i = 0; i < 15; i++)
        {
            if (isWhite)
            {
                spriteRend.color = tankColor;
            }
            else
            {
                spriteRend.color = Color.white;
                //spriteRend.color = Color.clear;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }

        tauntM.Taunt();

        ResetTankColor();
        invulnerable = false;
        sceneManager.TankIndexActivate(index, true);
        GoToState(VersusActorStates.NORMAL);
    }
    
    IEnumerator GetHitBlink()
    {
        invulnerable = true;

        //Do black white blinks
        spriteRend.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.red;
        yield return null;
        spriteRend.color = Color.white;
        yield return new WaitForSeconds(0.05f);

        //Do transparent blinks
        bool isWhite = false;
        for (int i = 0; i < 8; i++)
        {
            if (isWhite)
            {
                spriteRend.color = tankColor;
            }
            else
            {
                spriteRend.color = Color.white;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }

        invulnerable = false;
        ResetTankColor();
    }

    void ResetTankColor ()
    {
        spriteRend.color = tankColor;
    }
    #endregion    
}