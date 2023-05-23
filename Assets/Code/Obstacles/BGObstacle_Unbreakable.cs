using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGObstacle_Unbreakable : MonoBehaviour, IObstacle
{
    #region Fields
    int hp = 999;
    #endregion

    #region Methods
    public void TakeDmg(int dmg = 1)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}