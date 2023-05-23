using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudioPlay : MonoBehaviour 
{
    #region Fields
    public float waitTime;
    #endregion
	
	#region MonoBehaviour    
	IEnumerator Start () 
	{
        yield return new WaitForSeconds(waitTime);
        GetComponent<AudioSource>().Play();
	}

	#endregion
}
