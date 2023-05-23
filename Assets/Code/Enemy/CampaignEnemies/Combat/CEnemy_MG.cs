using UnityEngine;
using System.Collections;

public class CEnemy_MG : CampaignEnemyBase
{
    public GameObject pf_bullet;
    public float initialWait;
    public int bullets = 3;

    void Awake()
    {
        base.OnAwake();
    }

    void Start()
    {
        base.OnStart();

        MaxHP = 3;
        HP = MaxHP;

        StartCoroutine(ShortIntervalUpdate());
    }
    
    IEnumerator ShortIntervalUpdate()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(initialWait);
        while (true)
        {
            for (int i = 0; i < bullets; i++)
            {
                //Shooting
                //Find cloest enemy
                pos = trans.position;

                Instantiate(pf_bullet, pos, Quaternion.LookRotation(Vector3.forward, trans.up)).GetComponent<BulletBase>().Shoot(GM.enemyIndex, null);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }
}
