using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGObstacle_FightScene : MonoBehaviour, IObstacle
{
    #region Fields
    public Sprite[] blockSprites;

    SpriteRenderer img;
    int hp = 3;
    #endregion

    #region MonoBehaviour    
    private void Awake()
    {
        img = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Methods
    public void TakeDmg(int dmg = 1)
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else if (hp < blockSprites.Length)
        {
            img.sprite = blockSprites[hp];
        }
    }
    #endregion
}