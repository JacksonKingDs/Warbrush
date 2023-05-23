using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGObstacle : MonoBehaviour 
{
    #region Fields

    public Sprite[] blockSprites;

    Image img;
    int hp = 3;
    #endregion

    #region MonoBehaviour    
    private void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start () 
	{
	}
	
	void Update () 
	{
	}
    #endregion

    #region Methods
    public void TakeDmg()
    {
        hp--;
        if (hp < 0)
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