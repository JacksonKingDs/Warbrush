using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGTank : MonoBehaviour 
{
    #region Fields
    public float bound_minX;
    public float bound_minY;
    public float bound_maxX;
    public float bound_maxY;
    public int index;

    public RectTransform shootPoint;
    public GameObject pf_bullet;
    public Transform bulletParent;

    float moveSpeed = 0.5f;
    float knockbackSpeed = 0.7f;
    float rotSpeed = 50f;
    float rotAmount;

    bool inKnockback = false;

    RectTransform trans;
    Rigidbody2D rb;
    PolygonCollider2D playerCol;
    Image img;
    Color tankColor;

    List<BGTank> enemies;
    bool canShoot = false;
    #endregion

    #region MonoBehaviour    
    void Awake()
    {
        //Cache
        trans = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<PolygonCollider2D>();
        img = GetComponent<Image>();
        tankColor = img.color;

        //Init and Ref
        enemies = new List<BGTank>();
        foreach (BGTank i in FindObjectsOfType<BGTank>())
        {
            if (i != this)
            {
                enemies.Add(i);
            }
        }
    }

    void Start () 
	{
        //rb.velocity = moveSpeed * trans.up;
        StartCoroutine(RandomBehaviorUpdate());
        StartCoroutine(CanShootAgain());
    }

    IEnumerator CanShootAgain ()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            ShootUpdate();
        }
    }

    void Update () 
	{
        MoveUpdate();
    }
    #endregion

    #region Move and Shoot
    void MoveUpdate ()
    {
        OutOfBoundReflect();

        //Move forward if not in knockback. 
        if (!inKnockback)
        {
            rb.velocity = moveSpeed * trans.up;
            
        }
        else
        {
            rb.velocity = knockbackSpeed * kbDir;
        }
    }

    IEnumerator RandomBehaviorUpdate()
    {
        while (true)
        {
            if (!inKnockback)
            {
                rb.angularVelocity = Random.Range(-1f, 1f) * rotSpeed;
            }
            yield return new WaitForSeconds(Random.Range(0.2f, 3f));
        }
    }

    void OutOfBoundReflect()
    {
        Vector3 pos = trans.anchoredPosition;
        if (pos.x > bound_maxX) //Right
        {
            //UnityEngine.Debug.Log("right");
            Vector3 vel = rb.velocity;
            vel.x = -Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.x < bound_minX) //Left
        {
            //UnityEngine.Debug.Log("Lt");
            Vector3 vel = rb.velocity;
            vel.x = Mathf.Abs(vel.x);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y > bound_maxY) //Up
        {
            //UnityEngine.Debug.Log("Up");
            Vector3 vel = rb.velocity;
            vel.y = -Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
        else if (pos.y < bound_minY) //Down
        {
            //UnityEngine.Debug.Log("Dn");
            Vector3 vel = rb.velocity;
            vel.y = Mathf.Abs(vel.y);
            SetRigidbodyVelocity(vel);
        }
    }

    void SetRigidbodyVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
        if (!inKnockback) //Do not rotate tank towards moving direction during knockback
            trans.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
    #endregion

    #region Shoot
    void ShootUpdate()
    {
        Instantiate(pf_bullet, shootPoint.position, shootPoint.rotation, bulletParent).GetComponent<BGTankBullet>().Shoot(index);

        return;
        //Shoot
        if (canShoot)
        {
            canShoot = false;
            //Debug.Log("b");
            Instantiate(pf_bullet, shootPoint.position, shootPoint.rotation, bulletParent).GetComponent<BGTankBullet>().Shoot(index);
            //Debug.DrawRay(shootPoint.position, trans.up, Color.red, 5f);
        }

        return;
            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, trans.up, 100f);
            //Debug.DrawRay(shootPoint.position, trans.up * 5f, Color.red);
            if (hit.collider != null)
            {
                //Shoot if raycast aiming at an obstacle
                if (hit.collider.gameObject.layer == GM.layerObstacle)
                {
                    canShoot = false;
                    //Debug.Log("b");
                    Instantiate(pf_bullet, shootPoint.position, shootPoint.rotation, bulletParent).GetComponent<BGTankBullet>().Shoot(index);
                    //Debug.DrawRay(shootPoint.position, trans.up, Color.red, 5f);
                }
            }
        
    }
    #endregion

    #region Take hit
    Vector2 kbDir;
    public void TakeDamage (Vector3 enemyPos)
    {
        if (!inKnockback)
        {
            //Debug.Log("knockback");
            kbDir = (transform.position - enemyPos).normalized;
            //Debug.DrawRay(trans.position, kbDir, Color.yellow, 5f);
            StartCoroutine(GetHitBlink());            
        }
    }

    IEnumerator GetHitBlink()
    {
        inKnockback = true;

        //Do transparent blinks
        bool isWhite = false;
        for (int i = 0; i < 3; i++)
        {
            if (isWhite)
            {
                img.color = tankColor;
            }
            else
            {
                img.color = Color.clear;
            }
            isWhite = !isWhite;
            yield return new WaitForSeconds(0.1f);
        }
        img.color = tankColor;
        inKnockback = false;
    }
    #endregion
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == GM.layerObstacle)
        {
            OppositeBounce(col);

            //Dmg wall
            col.gameObject.GetComponent<BGObstacle>().TakeDmg();
        }
        else if (col.gameObject.layer == GM.layerPlayer)
        {
            ReflectBounce(col);
        }
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col != null)
    //    {
    //        //Debug.Log("BGTank col 1");
    //        //Bounce off wall
    //        if (col.gameObject.layer == GM.layerObstacle)
    //        {
    //            //Debug.Log("BGTank col 2a");
    //            ReflectBounce(col);

    //            //Dmg wall
    //            col.gameObject.GetComponent<BGObstacle>().TakeDmg();
    //        }
    //        else if (col.gameObject.layer == GM.layerPlayer)
    //        {
    //            //Debug.Log("BGTank col 2b");
    //            ReflectBounce(col);
    //        }
    //    }
    //}

    void ReflectBounce (Collider2D col)
    {
        RectTransform enemyTrans = col.GetComponent<RectTransform>();

        Vector2 pos = trans.anchoredPosition;
        Vector2 inDir = rb.velocity;
        Vector2 normal = enemyTrans.anchoredPosition - pos;
        Vector3 outAngle = Vector3.Reflect(inDir, normal);

        SetRigidbodyVelocity(outAngle);
    }

    void OppositeBounce(Collider2D col)
    {
        RectTransform enemyTrans = col.GetComponent<RectTransform>();
        Vector3 outDir = (trans.position - enemyTrans.position).normalized;
        //Debug.DrawRay(trans.position, outDir, Color.red, 1f);
        SetRigidbodyVelocity(outDir * moveSpeed);
    }
}