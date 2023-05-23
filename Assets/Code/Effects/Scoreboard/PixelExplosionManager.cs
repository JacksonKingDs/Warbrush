using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelExplosionManager : MonoBehaviour 
{
    #region Fields
    public List<Animator> explosions;

    int animState_Explode;
    #endregion
	
	#region MonoBehaviour    
	void Start () 
	{
        animState_Explode = Animator.StringToHash("PixelExplode");

    }
	
	//void Update () 
	//{
 //       if (Input.GetKeyDown(KeyCode.L))
 //       {
 //           Explode();
 //       }
	//}
	#endregion
	
	#region Methods
    public void Explode()
    {
        StartCoroutine(DoExplosions());
    }

    IEnumerator DoExplosions ()
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            explosions[i].Play(animState_Explode);
        }
    }
	#endregion
}
