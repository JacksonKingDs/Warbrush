using UnityEngine;
using System.Collections;

public class CEnemy_Stationary : CampaignEnemyBase
{
    public Transform barrel;
    public GameObject pf_bullet;
    public LayerMask rayHitLayer;

    void Awake()
    {
        base.OnAwake();
    }

    IEnumerator Start()
    {
        base.OnStart();

        MaxHP = 3;
        HP = MaxHP;        
        rotSpeed = 0.2f;

        curDir = tgtDir = trans.up;

        yield return new WaitForSeconds(2f);
        StartCoroutine(ShootingIntervalUpdate());
        StartCoroutine(AimingIntervalUpdate());
    }

    private void FixedUpdate()
    {  
        //curDir = Vector3.RotateTowards(curDir, tgtDir, rotSpeed, 0.0f);
    }

    IEnumerator ShootingIntervalUpdate()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(Random.Range(0f, 4f));
        while (true)
        {
            yield return new WaitForSeconds(4f);
            if (hasTarget)
            {
                Instantiate(pf_bullet, trans.position, Quaternion.LookRotation(Vector3.forward, clearTargetDir)).GetComponent<BulletBase>().Shoot(GM.enemyIndex, null);
            }
        }
    }

    bool hasTarget;
    Vector3 clearTargetDir;
    Vector3 shortestTargetDir;
    IEnumerator AimingIntervalUpdate()
    {
        yield return new WaitForSeconds(0.8f);
        yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            //Shooting
            //Find cloest enemy
            float shortestClearDist = float.MaxValue;
            float shortestAllDist = float.MaxValue;
            pos = trans.position;
            hasTarget = false;

            foreach (int i in sceneM.validPlayers)
            {
                Vector2 dir = sceneM.tanksTrans[i].position - pos;

                //Check if direction is clear, THEN calculate if this enemy is closest
                RaycastHit2D hit = Physics2D.Raycast(pos, dir, 20f, rayHitLayer);
                if (hit.collider != null && hit.collider.gameObject.layer == GM.layerPlayer)
                {
                    float dist = dir.sqrMagnitude;
                    if (dist < shortestClearDist)
                    {
                        hasTarget = true;

                        clearTargetDir = dir;
                        shortestClearDist = dist;
                        shortestAllDist = dist;
                        barrel.rotation = Quaternion.LookRotation(Vector3.forward, clearTargetDir);
                    }
                }
                //If being blocked, check if a clear target has being found. If no clear target has being found, then consider the possibility that all players are out of view, and 
                //we want the turret to still shoot at the closest player that's behind walls. 
                else if (!hasTarget)
                {
                    float dist = dir.sqrMagnitude;
                    if (dist < shortestAllDist)
                    {
                        shortestTargetDir = dir;
                        shortestAllDist = dist;
                        barrel.rotation = Quaternion.LookRotation(Vector3.forward, shortestTargetDir);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }
}
