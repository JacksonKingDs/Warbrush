using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test5_enemySpawner : MonoBehaviour 
{
    #region Fields
    public GameObject pf_testEnemy;
    [HideInInspector] public List<EnemyBase> enemies = new List<EnemyBase>();
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{

	}
	
	void Update () 
	{
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 5;
            enemies.Add(Instantiate(pf_testEnemy, p, Quaternion.identity).GetComponent<EnemyBase>());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    EnemyBase e;
                    if (e = hit.collider.GetComponent<EnemyBase>())
                    {
                        enemies.Remove(e);
                        Destroy(e.gameObject);
                    }
                }
                catch
                {}
            }
        }

        //Debug draw line
        foreach (var enemy in enemies)
        {
            Debug.DrawLine(Vector3.zero, enemy.transform.position, Color.red);
        }
	}

    public void AddEnemy (EnemyBase enemy)
    {
        enemies.Add(enemy);
    }
	#endregion
	
	#region Methods
	#endregion
}
