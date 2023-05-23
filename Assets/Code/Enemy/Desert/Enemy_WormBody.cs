using UnityEngine;
using System.Collections.Generic;

public class Enemy_WormBody : MonoBehaviour, IEnemy
{

    //Cache
    [HideInInspector] public Transform leader;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform trans;

    float followDist = 0.6f;
    float followDistSqr;

    //Stats
    float moveSpeed;
    float rotSpeed;

    //Current state
    float rotDir = 0f; //Rotating left or right etc.

    //Refs
    GM gm;
    BGTextureManager BG_Painter;
    FightSceneManager sceneM;
    AudioManager audioM;
    Camerashake camShake;

    void Start()
    {
        gm = GM.instance;
        BG_Painter = BGTextureManager.instance;
        sceneM = FightSceneManager.instance;
        audioM = AudioManager.instance;

        trans = transform;
        followDistSqr = followDist * followDist;
        camShake = Camerashake.instance;
    }

    List<Vector3> LeaderPositions = new List<Vector3>();
    Vector3 dirToLeader;
    void Update ()
    {
        LeaderPositions.Add(leader.position);
        if (LeaderPositions.Count>50)
        {
            transform.position = LeaderPositions[0];
            LeaderPositions.RemoveAt(0);
        }

        ////Facing leader
        //dirToLeader = leader.position - trans.position;
        //if (dirToLeader.sqrMagnitude > followDistSqr)
        //{
        //    transform.position = transform.position + (dirToLeader.normalized * followDist);
        //}
    }

    #region Public get hit
    public virtual void TakeDamage(int index, int dmg = 1)
    {
        if (index >= 0 && index < 4)
            FightSceneManager.landed[index] ++;
    }
    #endregion

    #region Hits Player Effect
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;
            if (go.layer == GM.layerPlayer)
            {
                TankControllerBase targetPlayer = go.GetComponent<TankControllerBase>();

                HitsPlayerEffect(go);

                targetPlayer.GetsHitByAttack(trans.position, GM.enemyIndex);
            }
        }
    }

    protected virtual void HitsPlayerEffect(GameObject go)
    {
        Instantiate(SettingsAndPrefabRefs.instance.Pfx_BoxExplode, (trans.position + go.transform.position) / 2f, Quaternion.identity); //Pfx
        AudioManager.instance.Spawn_Hits1(); //Sfx

        SpawnSplatter(go);
        camShake.DoSmallShake();  //Cam shake
    }

    protected virtual void SpawnSplatter(GameObject go)
    {
        BG_Painter.PaintSplatterFlower(transform.position, transform.position - go.transform.position, GM.enemyIndex);
    }
    #endregion
}