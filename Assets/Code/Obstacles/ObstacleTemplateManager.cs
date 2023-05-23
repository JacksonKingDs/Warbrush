using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTemplateManager : MonoBehaviour 
{
    #region Fields
    public GameObject[] combat_obstacles;
    public GameObject[] combat_night;
    public GameObject obstacle_spooky;
    public GameObject obstacle_desert;
    public GameObject obstacle_space;
    public GameObject obstacle_belts;
    public GameObject[] campaignObstacles;
    #endregion

    #region MonoBehaviour    
    void Start ()
    {
        switch (GM.gameMode)
        {
            case GameMode.PVP_Combat:
                combat_obstacles[GM.combatMapIndex % combat_obstacles.Length].SetActive(true);
                break;
            case GameMode.PVP_Night:
                //Debug.Log(GM.nightMapIndex);
                combat_night[GM.nightMapIndex % combat_night.Length].SetActive(true);
                break;
            case GameMode.Coop_Torch:
                obstacle_spooky.SetActive(true);
                //obstacle_spooky[GM.spookyMapIndex % obstacle_spooky.Length].SetActive(true);
                break;
            case GameMode.PVP_Desert:
                obstacle_desert.SetActive(true);
                break;
            //case GameMode.Coop_Beach:
            //    break;
            case GameMode.Hanabi:
                obstacle_space.SetActive(true);
                break;
            case GameMode.Campaign:
                campaignObstacles[GM.campaignMapIndex].SetActive(true);
                break;
            default:
                break;
        }
    }
	#endregion
}