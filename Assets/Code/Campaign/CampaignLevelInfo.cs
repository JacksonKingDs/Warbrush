using UnityEngine;
using System.Collections.Generic;

public class CampaignLevelInfo : MonoBehaviour
{
    public static CampaignLevelInfo active;

    public Transform[] respawnPoints;
    public bool canPaint;
    public List<Transform> enemies;

    protected void Awake()
    {
        active = this;
        Debug.Log("set active CampaignLevelInfo");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void TryRemoveEnemy (Transform enemy)
    {
        if (active.enemies.Contains(enemy))
        {
            active.enemies.Remove(enemy);
        }
    }
}
