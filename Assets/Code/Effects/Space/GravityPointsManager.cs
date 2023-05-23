using UnityEngine;
using System.Collections;

public class GravityPointsManager : MonoBehaviour
{
    public static GravityPointsManager instance;
    
    public Transform[] allGravityPoints;
    public Rigidbody2D[] pointsToRandomize;
    float rotAmount = 0f;
    float rotSpeed = 50f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(RandomizeRotation());
    }

    IEnumerator RandomizeRotation ()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 3f));
            //Random rotation
            rotAmount = (Random.value < 0.5f) ? -0.5f : 0.5f;
        }
    }
    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(20, 20, 200, 20), "rot " + rotAmount + ", spd " + rotSpeed);
    //}

    void Update()
    {
        //foreach (var p in allGravityPoints)
        //{
        //    Debug.DrawRay(p.position, p.up, Color.yellow);
        //}
    }

    void FixedUpdate()
    {
        foreach (var r in pointsToRandomize)
        {
            r.angularVelocity = rotAmount * rotSpeed;
        }        
    }

    #region Space dust get orientation
    float dist;
    Vector3 totalDir;
    public Vector3 GetWeightedRotation (Vector2 pos, bool anitGravity)
    {
        //If doesn't work try adding up quaternion and then normalizing the sum
        //Quaternion totalRotation;

        totalDir = Vector3.zero;

        if (anitGravity)
        {
            foreach (var p in allGravityPoints)
            {
                dist = Vector2.SqrMagnitude(pos - (Vector2)p.position);
                if (dist < 10f)
                {
                    //Points distant to you have a stronger influence on your direction
                    totalDir = totalDir + p.up * dist;
                }
            }
        }
        else
        {
            foreach (var p in allGravityPoints)
            {
                dist = Vector2.SqrMagnitude(pos - (Vector2)p.position);
                if (dist < 10f)
                {
                    //Points closer to you have a stronger influence on your direction
                    totalDir = totalDir + p.up * (10f - dist);
                }

            }
        }
        
        return totalDir.normalized;
    }
    #endregion


    //#region Collision
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col != null)
    //    {
    //        GameObject go = col.gameObject;
    //        if (go.layer == GM.layerPlayer)
    //        {
    //            transform.rotation = go.transform.rotation;
    //        }
    //    }
    //}
    //#endregion
}
