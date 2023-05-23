using UnityEngine;
using System.Collections;

public class CEnemy_spinningShooter : CampaignEnemyBase
{
    public GameObject pf_bullet;
    public Transform[] shootPoints;
    public Transform barrelAnchor;

    void Awake()
    {
        base.OnAwake();
    }

    void Start()
    {
        base.OnStart();

        MaxHP = 3;
        HP = MaxHP;

        StartCoroutine(IntervalUpdate());
    }

    private void FixedUpdate()
    {
        barrelAnchor.Rotate(new Vector3(0f, 0f, 1f));
    }

    bool hasTarget;
    bool visionClear;
    IEnumerator IntervalUpdate()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            yield return new WaitForSeconds(0.6f);

            foreach (var p in shootPoints)
            {
                Instantiate(pf_bullet, p.position, Quaternion.LookRotation(Vector3.forward, p.up)).GetComponent<BulletBase>().Shoot(GM.enemyIndex, null);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultTriggerEnter(collision);
    }

    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    DefaultCollisionEnter(col);
    //}
}
