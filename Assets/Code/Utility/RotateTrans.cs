using UnityEngine;
using System.Collections;

public class RotateTrans : MonoBehaviour
{
    public float speed = 1f;
    Transform trans;


    void Awake()
    {
        trans = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trans.Rotate(new Vector3(0f, 0f, speed));
    }
}
