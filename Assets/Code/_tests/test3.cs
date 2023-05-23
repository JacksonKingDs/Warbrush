using UnityEngine;
using System.Collections;

public class test3 : MonoBehaviour
{
    public Transform enemy;

    Vector3 cross;
    float dot;
    float angel;

    bool onRight;
    bool onLeft;
    bool inFront;

    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetDir = enemy.position - transform.position;

        cross = Vector3.Cross(transform.up, targetDir); //Tells if left and right. Positive if left, negative if right.
        dot = Vector3.Dot(transform.up, targetDir); //Tells if front and back. Negative if behind

        angel = Vector3.Angle(transform.up, targetDir);

        onRight = (cross.z > 0.2f);
        onLeft = (cross.z < -0.2f);
        inFront = dot > 0;


        if (inFront) //In front
        {
            if (onRight)
            {
                RotRight();
            }
            else if (onLeft)
            {
                RotLeft();
            }
            else
            {
                Rot(0);
            }
        }
        else //Behind
        {
            if (onRight )
            {
                RotRight();
            }
            else //This force the tank to rotate when enemy is directly behind, does not allow for situations where the tank doesn't rotate.
            {
                RotLeft();
            }
        }
    }

    void RotRight ()
    {
        Rot(-1);
    }

    void RotLeft()
    {
        Rot(1);
    }

    void Rot (float rot)
    {
        rb.angularVelocity = rot * -6000f * Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "Cross: " + cross);
        GUI.Label(new Rect(20, 40, 200, 20), "Dot: " + dot);
        GUI.Label(new Rect(20, 60, 200, 20), "angel: " + angel);

        GUI.Label(new Rect(20, 100, 200, 20), "onRight: " + onRight);
        GUI.Label(new Rect(20, 120, 200, 20), "onLeft: " + onLeft);
        GUI.Label(new Rect(20, 140, 200, 20), "inFront: " + inFront);
    }
}

/*
 float angel = Vector3.Angel(obj1.transform.forward, Obj2.position-transform.position);
                if (Mathf.Abs(angel) > 30 )
                    print("Object2 if front Obj1");
     */
