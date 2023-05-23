using UnityEngine;
using System.Collections;

public class CEnemy_Artillery : CampaignEnemyBase
{
    public GameObject pf_bullet;
    public LayerMask rayHitLayer;
    public Transform aimingIcon;
    public Animator aimingIconAnim;
    public float initialWait;

    int animation_hide;
    int animation_aim;

    void Awake()
    {
        base.OnAwake();
        lastKnownPosition = transform.position;

        animation_hide = Animator.StringToHash("AimerHide");
        animation_aim = Animator.StringToHash("LockOn");
    }

    IEnumerator Start()
    {
        base.OnStart();

        MaxHP = 3;
        HP = MaxHP;
        yield return new WaitForSeconds(3f);
        gameStarted = true;
        //StartCoroutine(IntervalUpdate());
    }

    float lockonDuration;
    private void Update()
    {
        if (!gameStarted)
            return;

        //Locked-on update
        if (targetTank != null && targetTank.activeState != VersusActorStates.INACTIVE) 
        {
            lastKnownPosition = targetTrans.position;

            if (Vector2.Distance(aimingIcon.position, targetTrans.position) < 0.6f)
            {
                lockonDuration += Time.deltaTime;
                if (lockonDuration > 3f)
                {
                    Instantiate(pf_bullet, trans.position + new Vector3(0f, 0f, 0.05f), Quaternion.identity).GetComponent<BulletBase>().Shoot(GM.enemyIndex, lastKnownPosition);
                    lockonDuration = 0f;
                }
            }
        }
        else
        {
            lockonDuration = 0f;
            FindRandomPlayer();
        }
                
        aimingIcon.position = Vector2.MoveTowards(aimingIcon.position, lastKnownPosition + (Vector3)Random.insideUnitCircle * 0.2f, 0.02f * Time.timeScale);
    }

    Vector3 lastKnownPosition;
    Transform targetTrans;
    TankControllerBase targetTank;

    float findNewPlayerCD = 0;
    protected void FindRandomPlayer ()
    {
        if (findNewPlayerCD  <= 0)
        {
            findNewPlayerCD = 0.5f;
            targetTank = null;

            if (sceneM.validPlayers.Count > 0)
            {
                //Get a random Valid tank 
                int tankIndex = sceneM.validPlayers[Random.Range(0, sceneM.validPlayers.Count)];

                if (sceneM.tankManagers[tankIndex].activeState != VersusActorStates.INACTIVE)
                {
                    targetTank = sceneM.tankManagers[tankIndex];
                    targetTrans = sceneM.tanksTrans[tankIndex];

                    aimingIconAnim.Play(animation_aim);
                }
            }
        }
        else
        {
            findNewPlayerCD -= Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        DefaultCollisionEnter(col);
    }
}
