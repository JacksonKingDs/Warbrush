using UnityEngine;
using System.Collections;

public class DummyTankController : TankControllerBase
{
    public override void GetsHitByAttack(Vector3 bulletPos, int enemyIndex) //Called by the bullet that hits this
    {}

    void Awake()
    {
        index = -3;
        trans = transform;
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<PolygonCollider2D>();


    }

    void Update()
    {}

    void FixedUpdate()
    {}
}