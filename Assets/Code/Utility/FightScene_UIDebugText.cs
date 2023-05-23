using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScene_UIDebugText : MonoBehaviour 
{
    #region Fields
    public static FightScene_UIDebugText instance;
    public Text t1;
    public Text t2;
    #endregion

    #region MonoBehaviour    
    private void Awake()
    {
        instance = this;
    }
    public void DisplayTextA(string s)
    {
        t1.text = s;
    }

    public void DisplayTextB(string s)
    {
        t2.text = s;
    }

    void Start () 
	{
	}
	
	void Update () 
	{
	}
	#endregion
	
	#region Methods
	#endregion
}
