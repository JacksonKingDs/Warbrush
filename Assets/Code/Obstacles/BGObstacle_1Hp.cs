using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGObstacle_1Hp : MonoBehaviour, IObstacle
{
    #region Methods
    public void TakeDmg(int dmg = 1)
    {
        Destroy(gameObject);
    }
    #endregion
}