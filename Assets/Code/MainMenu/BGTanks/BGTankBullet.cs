using UnityEngine;
using System.Collections;

public class BGTankBullet : MonoBehaviour
{
    //Bound
    public GameObject pfx_explosion;

    public float BG_Bound_minX = -282f;
    public float BG_Bound_minY = -58.8f;
    public float BG_Bound_maxX = 282f;
    public float BG_Bound_maxY = 58.8f;

    float movespeed = 3f;
    int index;

    Rigidbody2D rb;
    RectTransform trans;
    Vector2 pos;
    Transform parent;

    public void Shoot (int index)
    {
        this.index = index;
        GetComponent<Collider2D>().enabled = true;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<RectTransform>();
        parent = trans.parent;

        rb.velocity = movespeed * trans.up;
    }

    void FixedUpdate()
    {
        pos = trans.anchoredPosition;

        //Destroy when hitting border
        if (pos.x > BG_Bound_maxX || pos.x < BG_Bound_minX ||
            pos.y > BG_Bound_maxY || pos.y < BG_Bound_minY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            GameObject go = col.gameObject;

            //If collided with a player and they are not the same index as self...
            //GM.gameMode == GameMode.Brawl && 
            if (go.layer == GM.layerObstacle)
            {
                go.GetComponent<BGObstacle>().TakeDmg();
                DestroyBullet(go.transform.position);
            }
            else if (go.layer == GM.layerEnemy)
            {
                if (index != go.GetComponent<BGTank>().index)
                {
                    //Debug.Log("my idnex " + index + "other index " + go.GetComponent<BGTank>().index);
                    go.GetComponent<BGTank>().TakeDamage(transform.position);
                    DestroyBullet(go.transform.position);
                }

                //go.GetComponent<BGTank>().TakeDamage(trans.anchoredPosition);
                
            }
            else if (go.layer == GM.layerDeadTank)
            {
                DestroyBullet(go.transform.position);
            }
        }
    }

    void DestroyBullet (Vector3 hitPos)
    {
        //Vector3 pos = (trans.position + hitPos) / 2f;
        //Instantiate(pfx_explosion, trans.position, Quaternion.identity, parent);
        Destroy(gameObject);
    }
}