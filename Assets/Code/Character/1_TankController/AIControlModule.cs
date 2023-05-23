using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIModes
{
    //NOENEMY,
    TARGETED,
    RANDOM
}

public class AIControlModule
{
    GM gm;
    FightSceneManager sceneM;
    TankControllerBase tank;
    Transform trans;
    ControlScheme control;
    EnemyManager enemyM;
    CampaignLevelInfo levelInfo;
    int index;

    float countToRandomizeMovementX = 0f;
    float countToRandomizeMovementY = 0f; //Counts down to the next movement randomization.

    //Aiming
    Vector2 targetDir;
    float selectEnemyInterval = 5f;
    float shootCDCounter;

    //Turning
    public AIModes aiMode;
    bool turnLeft;
    bool isBouncer;
    bool isPvP;
    bool isCampaign = false;

    //Ctor
    public AIControlModule(TankControllerBase tank)
    {
        //Reference data about self
        this.tank = tank;
        index = tank.index;
        trans = tank.transform;
        control = tank.control;

        //Ref class
        gm = GM.instance;
        sceneM = FightSceneManager.instance;
        enemyM = EnemyManager.instance;

        //Initialize
        aiMode = AIModes.TARGETED;
        control.A_Btn = true;
        if (GM.gameMode == GameMode.Coop_Arcade || GM.gameMode == GameMode.Coop_Torch)
            isPvP = false;
        else if (GM.gameMode == GameMode.Campaign)
        {
            isPvP = false;
            isCampaign = true;
            levelInfo = CampaignLevelInfo.active;
        }
        else
            isPvP = true;

        //countToRandomizeMovementY = Random.Range(1f, 4f);
        //countToRandomizeMovementX = Random.Range(1f, 4f);
        shootCDCounter = Random.Range(2f, 4f);
        isBouncer = tank.modelName == TankModelNames.BOUNCER;
        tank.StartCoroutine(BehaviorToggle());
    }

    Vector3 cross;
    bool enemyAimed = false;
    //FUNCTIONS
    public void DoUpdate()
    {
        if (aiMode == AIModes.RANDOM)
        {
            AIMode_Random();
        }
        else // (aiMode == AIModes.CLOSEST)
        {
            AIMode_Targeted();
        }
        //Debug.DrawLine(trans.position, targetDir, Color.red, 0.3f);
    }

    #region AI Modes: intentional and random
    void AIMode_Random ()
    {
        //Shooting
        if (shootCDCounter < 0f)
        {
            ShootBullet();
            shootCDCounter = isBouncer ? 4f : Random.Range(1f, 2f);
        }
        else
        {
            shootCDCounter -= Time.deltaTime;
        }

        //Forward back: Move Y
        if (countToRandomizeMovementY <= 0)
        {
            countToRandomizeMovementY = Random.Range(0.2f, 2f);
            RandomizeMoveY();
        }
        else
        {
            countToRandomizeMovementY -= Time.deltaTime;
        }

        //Forward back: Move X
        if (countToRandomizeMovementX <= 0)
        {
            countToRandomizeMovementX = Random.Range(0.5f, 2f);
            RandomizeMoveX();
        }
        else
        {
            countToRandomizeMovementX -= Time.deltaTime;
        }
    }

    float dot;
    bool onRight;
    bool onLeft;
    bool inFront;
    void AIMode_Targeted()
    {
        cross = Vector3.Cross(trans.up, targetDir);
        inFront = Vector3.Dot(targetDir, trans.up) > 0;
        onRight = (cross.z > 0.4f);
        onLeft = (cross.z < -0.4f);        

        //ROTATION
        if (inFront) //In front
        {
            if (onRight)
            {
                RotRight();
                enemyAimed = false;
            }
            else if (cross.z < -0.2f)
            {
                RotLeft();
                enemyAimed = false;
            }
            else
            {
                control.MoveX = 0;
                enemyAimed = true;
            }
        }
        else
        {
            if (onRight)
            {
                RotRight();
            }
            else //does not allow for situations where the tank doesn't rotate.
            {
                RotLeft();
            }
        }

        //VERTICAL MOVEMENT Forward and back
        if (countToRandomizeMovementY <= 0)
        {
            countToRandomizeMovementY = Random.Range(0.2f, 2f);
            RandomizeMoveY();
        }
        else
        {
            countToRandomizeMovementY -= Time.deltaTime;
        }

        //Shooting
        if (shootCDCounter < 0f)
        {
            if (enemyAimed)
            {
                ShootBullet();
                shootCDCounter = isBouncer ? 3f : Random.Range(1f, 2f);
            }
        }
        else
        {
            shootCDCounter -= Time.deltaTime;
        }
    }

    void RotRight()
    {
        control.MoveX = -1;
    }

    void RotLeft()
    {
        control.MoveX = 1;
    }

    void RandomizeMoveY()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                control.MoveY = 0;
                break;
            case 1:
                control.MoveY = -1;
                break;
            default: 
                control.MoveY = 1;
                break;
        }
    }

    void RandomizeMoveX()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                RotLeft();
                break;
            case 1:
                RotRight();
                break;
            default:
                control.MoveX = 0;
                break;
        }
    }
    #endregion

    #region Locate target
    //Check if there is a valid enemy and then update direction towards it.
    bool HasEnemyTarget()
    {
        if (isPvP)
        {
            List<int> enemies = new List<int>(sceneM.validPlayers);
            enemies.Remove(index);
            int enemyCount = enemies.Count;

            if (enemyCount > 0) //If there is 1 or more enemy, then acquire target.
            {
                int enemyIndex = enemies[0];
                if (enemyCount > 1)
                {
                    enemyIndex = enemies[Random.Range(0, enemies.Count)]; //Random target
                }

                targetDir = sceneM.tanksTrans[enemyIndex].position - trans.position;

                return true;
            }
            return false;
        }
        else if (isCampaign)
        {
            if (CampaignEnemyBase.count > 0)
            {
                int i = Random.Range(0,  levelInfo.enemies.Count);
                targetDir = levelInfo.enemies[i].transform.position - trans.position;
                return true;
            }
            return false;
        }
        else 
        {
            if (enemyM.activeEnemies.Count > 0)
            {
                int i = Random.Range(0, enemyM.activeEnemies.Count);
                targetDir = enemyM.activeEnemies[i].transform.position - trans.position;
                //Debug.DrawLine(enemyM.Enemies[i].transform.position, trans.position, Color.red);
                //Debug.Log("hi");
                return true;
            }
            return false;
        }
    }
    #endregion

    IEnumerator BehaviorToggle () //Runs in the background
    {
        while (true)
        {
            //Targeted mode
            float targetingDuration = Random.Range(6f, 8f);
            aiMode = AIModes.TARGETED;
            while (HasEnemyTarget() && targetingDuration > 0f)
            {
                targetingDuration -= Time.deltaTime;
                yield return null;
            }
            //Random mode
            aiMode = AIModes.RANDOM;
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            
        }
    }

    //SHOOTING
    void ShootBullet()
    {
        tank.StartCoroutine(ShootThenCharge());
    }

    IEnumerator ShootThenCharge()
    {
        //Debug.Log("ShootThenCharge");
        control.A_Btn = false;
        control.A_BtnUp = true;
        yield return new WaitForSeconds(0.2f);
        control.A_BtnUp = false;
        control.A_Btn = true;
    }
}