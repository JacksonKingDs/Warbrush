using UnityEngine;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

//Arcade enemy manager
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    //Prefabs
    [Header("Arcade")]
    public GameObject pf_Arrow_Follower;

    public GameObject pf_Diamond_Straight;
    public GameObject pf_Hat_ZigZag;
    public GameObject pf_Moon;
    public GameObject pf_Plane_Random;
    public GameObject pf_X_Shooter;
    public GameObject pf_centipedeHead;
    public GameObject pf_centipedeBody;
    public GameObject pf_centipedeTail;

    [Header("Spooky")]
    public GameObject pf_Zombie;
    public GameObject pf_Bat;
    public GameObject pf_Ghost;
    public GameObject pf_Archer;

    //Spawn points
    public List<Transform> LeftSpawnPoints;
    public List<Transform> RightSpawnPoints;
    public List<Transform> TopSpawnPoints;
    public List<Transform> BotSpawnPoints;

    public List<Transform> squadSpawnPoint1;
    public List<Transform> squadSpawnPoint2;

    //For enemy targeting
    [HideInInspector] public List<GameObject> activeEnemies;

    //Pool
    List<GameObject> pool_Arrow_Follower;
    List<GameObject> pool_diamondStraight;
    List<GameObject> pool_Hat_ZigZag;
    List<GameObject> pool_Moon;
    List<GameObject> pool_Plane_Random;
    List<GameObject> pool_X_Shooter;
    List<GameObject> pool_centipede_Head;
    List<GameObject> pool_centipede_Body;
    List<GameObject> pool_centipede_Tail;

    List<GameObject> pool_zombie;
    List<GameObject> pool_bat;
    List<GameObject> pool_ghost;
    List<GameObject> pool_archer;

    //Spawn points
    List<Transform> AllSpawnPoints;
    Vector3 RandomSpawnPosition;
    Quaternion RandomSpawnRotation;

    //Setting
    Vector3 offscreen = new Vector3(-10f, -10f, 0f);
    float spawnDelay = 0.3f;

    //Stat
    [HideInInspector] public int wave;

    UIManager uiM;
    FightSceneManager fightM;

    #region Initialize
    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        if (GM.gameMode != GameMode.Coop_Arcade && GM.gameMode != GameMode.Coop_Torch)
        {
            //Debug.Log("disabled EnemyManager " + GM.gameMode);
            this.enabled = false;
            yield break;
        }

        yield return new WaitForSeconds(2.8f);

        //Ref
        uiM = UIManager.instance;
        fightM = FightSceneManager.instance;

        //Initialize
        AllSpawnPoints = LeftSpawnPoints.Concat(RightSpawnPoints).Concat(TopSpawnPoints).Concat(BotSpawnPoints).ToList();
        activeEnemies = new List<GameObject>();
        wave = 0;
        

        //Spawn
        if (GM.gameMode == GameMode.Coop_Arcade)
        {
            pool_Arrow_Follower = new List<GameObject>();
            pool_diamondStraight = new List<GameObject>();
            pool_Hat_ZigZag = new List<GameObject>();
            pool_Moon = new List<GameObject>();
            pool_Plane_Random = new List<GameObject>();
            pool_X_Shooter = new List<GameObject>();
            pool_centipede_Head = new List<GameObject>();
            pool_centipede_Body = new List<GameObject>();
            pool_centipede_Tail = new List<GameObject>();

            StartCoroutine(PeriodicSpawn());
        }
        else if (GM.gameMode == GameMode.Coop_Torch)
        {
            pool_zombie = new List<GameObject>();
            pool_bat = new List<GameObject>();
            pool_ghost = new List<GameObject>();
            pool_archer = new List<GameObject>();

            StartCoroutine(PeriodicSpawnSpooky());
        }
    }

    IEnumerator Test_spawnPlanes()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.PlaneRandom).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator Test_spawnX()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.XShooter).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator Test_spawnMoon()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.Moon).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator Test_spawnSmlArrow()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.ArrowFollower).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator Test_spawnStraight()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.DiamondStraight).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator Test_spawnZig()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(EnemyType.HatZigZag).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    EnemyType otherType = GetRandomEnemyType_Spooky();
        //    UpdateRandomSpawnPoint();
        //    PopFromPool(EnemyType.spooky3_ghost).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    StartCoroutine(Spawn_5_centipede());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    StartCoroutine(Test_spawnMoon());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    StartCoroutine(Spawn_5_centipede());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    StartCoroutine(Test_spawnMoon());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    StartCoroutine(Test_spawnPlanes());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    StartCoroutine(Test_spawnX());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    StartCoroutine(Test_spawnSmlArrow());
        //    wave++; uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    StartCoroutine(Test_spawnStraight());
        //    wave++;
        //    uiM.waveText.text = wave.ToString();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    StartCoroutine(Test_spawnZig());
        //    wave++;
        //    uiM.waveText.text = wave.ToString();
        //}
    }

    int waveType = 1;
    IEnumerator PeriodicSpawn()
    {
        //The first wave must be squad
        StartCoroutine(SpawnSquad());
        wave++;
        uiM.waveText.text = "Wave " + wave.ToString();
        yield return new WaitForSeconds(8f);

        while (true)
        {
            if (activeEnemies.Count < 100)
            {
                switch (waveType)
                {
                    case 0:
                        Spawn_1Direction();
                        break;
                    case 1:
                        StartCoroutine(Spawn_2BroadwayDirection());
                        break;
                    case 2:
                        StartCoroutine(Spawn_3_Shooter());  //BUG
                        break;
                    case 3:
                        StartCoroutine(Spawn_4_3Types());
                        break;
                    case 4:
                    default:
                        StartCoroutine(Spawn_5_centipede());
                        waveType = -1;
                        break;
                }
                waveType++;
                wave++;
                uiM.waveText.text = "Wave " + wave.ToString();
            }

            yield return new WaitForSeconds(8f);
        }
    }
    #endregion

    #region 1 1 Type
    void Spawn_1Direction()
    {
        //Pick random enemy type
        EnemyType type;
        switch (Random.Range(0, 3))
        {
            case 0:
                StartCoroutine(SpawnSquad());
                return;
            case 1:
                type = EnemyType.DiamondStraight;
                break;
            default:
                type = EnemyType.ArrowFollower;
                break;
        }

        //Spawn subwaves
        List<Transform> points = RandomPointsOfOneDirection();
        foreach (Transform point in points)
        {
            PopFromPool(type).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
        }
    }

    IEnumerator SpawnSquad ()
    {
        List<Transform> _points;
        switch (Random.Range(0, 2))
        {
            case 0:
                _points = squadSpawnPoint1;
                break;
            case 1:
            default:
                _points = squadSpawnPoint2;
                break;
        }

        bool pause = false;
        for (int i = 0; i < _points.Count; i ++)
        {
            PopFromPool(EnemyType.HatZigZag).GetComponent<EnemyBase>().Activation(_points[i].position, _points[i].rotation);
            pause = !pause;
            if (pause)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    #endregion

    #region 2 Broadway
    IEnumerator Spawn_2BroadwayDirection()
    {
        EnemyType type;
        int toSpawn;
        switch (Random.Range(0, 5))
        {
            case 0:
            case 1:
                type = EnemyType.Moon;
                toSpawn =  (int)(2 + (0.3f * wave + 0.5f * fightM.validPlayers.Count));
                break;
            case 2:
            case 3:
            case 4:
                type = EnemyType.PlaneRandom;
                toSpawn = (int)(4 + (0.3f * wave + 2 * fightM.validPlayers.Count));
                break;
            default:
                type = EnemyType.ArrowFollower;
                toSpawn = (int)(3 + (0.3f * wave + 1.5f * fightM.validPlayers.Count));
                break;
        }

        for (int i = 0; i < toSpawn; i++)
        {
            Transform point = AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
            PopFromPool(type).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region 3 Shooter + ???
    IEnumerator Spawn_3_Shooter()
    {
        //Spawn subwaves
        int toSpawn = (int)(2 + (0.3f *  wave + 1.5 * fightM.validPlayers.Count));
        EnemyType otherType = GetRandomEnemyType_Arcade();
        while (toSpawn > 0)
        {
            toSpawn--;
            UpdateRandomSpawnPoint();
            if (Random.value > 0.5f)
            {
                //Debug.Log(otherType);
                PopFromPool(otherType).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            }
            else
            {
                PopFromPool(EnemyType.XShooter).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region 4 3 types
    IEnumerator Spawn_4_3Types()
    {
        //Spawn subwaves
        int toSpawn = (int)(3 + (0.3f * wave + 1.5 * fightM.validPlayers.Count));
        EnemyType t1 = GetRandomEnemyType_Arcade();
        EnemyType t2 = GetRandomEnemyType_Arcade();
        EnemyType t3 = GetRandomEnemyType_Arcade();
        while (toSpawn > 0)
        {
            toSpawn--;
            UpdateRandomSpawnPoint();
            if (Random.value < 0.33f)
            {
                PopFromPool(t1).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            }
            else if (Random.value < 0.66f)
            {
                PopFromPool(t2).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            }
            else
            {
                PopFromPool(t3).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            }
            yield return new WaitForSeconds(2f);
        }
    }
    #endregion

    #region 5 Centipede
    bool centipedeSpawning = false;
    IEnumerator Spawn_5_centipede()
    {
        if (!centipedeSpawning)
        {
            centipedeSpawning = true;

            int players = fightM.validPlayers.Count;
            UpdateRandomSpawnPoint();
            Vector3 p = RandomSpawnPosition;
            Quaternion r = RandomSpawnRotation;

            //Head
            Transform prevPart = PopFromPool(EnemyType.CentipedeHead).transform;
            prevPart.GetComponent<EnemyBase>().Activation(p, r);

            //Body
            Transform newPart;
            bool leftUp = false;
            for (int i = 0; i < Random.Range(3, (int)(6 + fightM.validPlayers.Count + 0.3f * wave)); i++)
            {
                //Debug.Break();
                yield return new WaitForSeconds(0.25f);
                newPart = PopFromPool(EnemyType.CentipedeBody).transform;
                //Debug.Log("prevPart: " + prevPart);
                newPart.GetComponent<EnemyBase>().Activation(p, r, prevPart, leftUp);
                leftUp = !leftUp;
                prevPart = newPart;
            }

            centipedeSpawning = false;
        }
    }
    #endregion

    #region Util
    void UpdateRandomSpawnPoint()
    {
        switch (Random.Range(0, 4))
        {
            case 0: //Top
                RandomSpawnPosition = new Vector3(Random.Range(-4f, 4f), 5.5f, 0f);
                RandomSpawnRotation = Quaternion.Euler(0, 0, 180);
                break;
            case 1: //Bot
                RandomSpawnPosition = new Vector3(Random.Range(-4f, 4f), -5.5f, 0f);
                RandomSpawnRotation = Quaternion.Euler(0, 0, 0);
                break;
            case 2: //Left
                RandomSpawnPosition = new Vector3(-8f, Random.Range(-4f, 4f), 0f);
                RandomSpawnRotation = Quaternion.Euler(0, 0, 270);
                break;
            case 3: //Right
            default:
                RandomSpawnPosition = new Vector3(8f, Random.Range(-4f, 4f), 0f);
                RandomSpawnRotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }

    List<Transform> RandomPointsOfOneDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return LeftSpawnPoints;
            case 1:
                return RightSpawnPoints;
            case 2:
                return TopSpawnPoints;
            case 3:
            default:
                return BotSpawnPoints;
        }
    }

    Transform GetRandomSpawnPoint()
    {
        return AllSpawnPoints[Random.Range(0, AllSpawnPoints.Count)];
    }

    GameObject GetRandomEnemy_Arcade()
    {
        return GetPf_fromType(GetRandomEnemyType_Arcade());
    }

    GameObject GetRandomEnemy_Spooky()
    {
        return GetPf_fromType(GetRandomEnemyType_Spooky());
    }

    EnemyType GetRandomEnemyType_Arcade()
    {
        //Enum
        int i = Random.Range(0, 6);
        switch(i)
        {
            case 0:
                return EnemyType.ArrowFollower;
            case 1:
                return EnemyType.DiamondStraight;
            case 2:
                return EnemyType.HatZigZag;
            case 3:
                return EnemyType.Moon;
            case 4:
                return EnemyType.PlaneRandom;
            default:
                return EnemyType.XShooter;
        }
    }

    EnemyType GetRandomEnemyType_Spooky()
    {
        //Enum
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                return EnemyType.spooky1_zombie;
            case 1:
                return EnemyType.spooky2_bat;
            case 2:
            default:
                return EnemyType.spooky3_ghost;
        }
    }
    #endregion

    #region Spooky
    IEnumerator PeriodicSpawnSpooky ()
    {
        while (true)
        {
            if (activeEnemies.Count < 100)
            {
                switch (waveType)
                {
                    case 0:
                        StartCoroutine(SpawnZombies());
                        break;
                    case 1:
                        StartCoroutine(SpawnBats());
                        break;
                    case 2:                    
                        StartCoroutine(SpawnGhost());  //BUG
                        break;
                    case 3:
                    default:
                        StartCoroutine(SpawnArcher());
                        waveType = -1;
                        break;
                }
                waveType++;
                wave++;
                uiM.waveText.text = "Wave " + wave.ToString();
            }

            yield return new WaitForSeconds(8f);
        }
    }

    IEnumerator SpawnZombies ()
    {
        //Spawn subwaves
        int toSpawn = (int)(3 + (2f * wave + 2f * fightM.validPlayers.Count));
        for (int i = 0; i < toSpawn; i++)
        {
            UpdateRandomSpawnPoint();
            PopFromPool(EnemyType.spooky1_zombie).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnBats ()
    {
        int toSpawn = (int)(1 + (0.3f * wave + 1f * fightM.validPlayers.Count));
        for (int i = 0; i < toSpawn; i++)
        {
            UpdateRandomSpawnPoint();
            PopFromPool(EnemyType.spooky2_bat).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnArcher()
    {
        int toSpawn = (int)(1 + (0.3f * wave + 1f * fightM.validPlayers.Count));
        for (int i = 0; i < toSpawn; i++)
        {
            UpdateRandomSpawnPoint();
            PopFromPool(EnemyType.spooky4_archer).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnGhost()
    {
        int toSpawn = (int)(1 + (0.3f * wave + 1f * fightM.validPlayers.Count));
        for (int i = 0; i < toSpawn; i++)
        {
            UpdateRandomSpawnPoint();
            PopFromPool(EnemyType.spooky3_ghost).GetComponent<EnemyBase>().Activation(RandomSpawnPosition, RandomSpawnRotation);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region Object pool
    public GameObject PopFromPool(EnemyType type)
    {
        List<GameObject> pool = GetPool_fromType(type);

        foreach (GameObject enemy in pool)
        {
            if (!enemy.activeSelf)
            {
                activeEnemies.Add(enemy);
                enemy.SetActive(true);
                //Debug.Log("poped " + type);
                return enemy;
            }
        }
        //Debug.Log("spawned " + type);
        GameObject e = Instantiate(GetPf_fromType(type), offscreen, Quaternion.identity);
        e.GetComponent<EnemyBase>().Initialization();
        activeEnemies.Add(e);
        pool.Add(e);
        return e;
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        activeEnemies.Remove(go);
    }
    #endregion

    #region Object pool Util
    GameObject GetPf_fromType(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.DiamondStraight:
                return pf_Diamond_Straight;
            case EnemyType.HatZigZag:
                return pf_Hat_ZigZag;
            case EnemyType.PlaneRandom:
                return pf_Plane_Random;
            case EnemyType.ArrowFollower:
                return pf_Arrow_Follower;
            case EnemyType.XShooter:
                return pf_X_Shooter;
            case EnemyType.Moon:
                return pf_Moon;
            case EnemyType.CentipedeHead:
                return pf_centipedeHead;
            case EnemyType.CentipedeBody:
                return pf_centipedeBody;
            case EnemyType.CentipedeTail:
                return pf_centipedeTail;

            case EnemyType.spooky1_zombie:
                return pf_Zombie;
            case EnemyType.spooky2_bat:
                return pf_Bat;
            case EnemyType.spooky3_ghost:
                return pf_Ghost;
            default:
            case EnemyType.spooky4_archer:
                return pf_Archer;
        }
    }

    List<GameObject> GetPool_fromType(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.DiamondStraight:
                return pool_diamondStraight;
            case EnemyType.HatZigZag:
                return pool_Hat_ZigZag;
            case EnemyType.PlaneRandom:
                return pool_Plane_Random;
            case EnemyType.ArrowFollower:
                return pool_Arrow_Follower;
            case EnemyType.XShooter:
                return pool_X_Shooter;
            case EnemyType.Moon:
                return pool_Moon;
            case EnemyType.CentipedeHead:
                return pool_centipede_Head;
            case EnemyType.CentipedeBody:
                return pool_centipede_Body;
            case EnemyType.CentipedeTail:
                return pool_centipede_Tail;

            case EnemyType.spooky1_zombie:
                return pool_zombie;
            case EnemyType.spooky2_bat:
                return pool_bat;
            case EnemyType.spooky3_ghost:
                return pool_ghost;
            default:
            case EnemyType.spooky4_archer:
                return pool_archer;
        }
    }
    #endregion
}

public enum EnemyType
{
    DiamondStraight,
    HatZigZag,
    PlaneRandom,
    ArrowFollower,
    XShooter,
    Moon,
    CentipedeHead,
    CentipedeBody,
    CentipedeTail,

    spooky1_zombie,
    spooky2_bat,
    spooky3_ghost,
    spooky4_archer,
}

/*
 IEnumerator Spawn_2BroadwayDirection()
{
    //Spawn subwaves
    int toSpawn = (int)(10 + (0.6f * wave));
    while (toSpawn > 0)
    {
        toSpawn--;
        foreach (Transform point in AllSpawnPoints)
        {
            PopFromPool(EnemyType.DiamondStraight).GetComponent<EnemyBase>().Activation(point.position, point.rotation);
        }
        yield return new WaitForSeconds(1f);
    }
}
     */
