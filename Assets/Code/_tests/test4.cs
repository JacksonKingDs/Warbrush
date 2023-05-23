using UnityEngine;
using System.Collections;

public class test4 : MonoBehaviour
{
    InputManager inputM;

    private void Start()
    {
    }

    void Update()
    {
        //Directions
        Vector3 toEnemy = Vector3.zero - transform.position;
        Vector3 up = transform.up;
        Debug.DrawRay(transform.position, toEnemy, Color.red);
        Debug.DrawRay(transform.position, up, Color.yellow);

        Vector3 halfWay1 = (toEnemy + up) / 2f;
        Debug.DrawRay(transform.position, halfWay1, Color.green);

        Vector3 halfWay2 = Vector2.Lerp(up, toEnemy, 0.3f);
        //Debug.DrawRay(transform.position, halfWay1, Color.blue);
    }
}

/*
 float angel = Vector3.Angel(obj1.transform.forward, Obj2.position-transform.position);
                if (Mathf.Abs(angel) > 30 )
                    print("Object2 if front Obj1");
     */
