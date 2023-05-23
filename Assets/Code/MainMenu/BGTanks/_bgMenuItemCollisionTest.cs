using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _bgMenuItemCollisionTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            Debug.Log("A");
            //Bounce off wall
            if (col.gameObject.layer == GM.layerObstacle)
            {
                Debug.Log("Test col objstacle");

                //Dmg wall
                col.gameObject.GetComponent<BGObstacle>().TakeDmg();
            }
            else if (col.gameObject.layer == GM.layerEnemy)
            {
                col.GetComponent<BGTank>().TakeDamage(transform.position);
                Debug.Log("Test col player");
            }
        }
    }
}
