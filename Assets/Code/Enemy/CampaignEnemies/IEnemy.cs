using UnityEngine;
using System.Collections;

public interface IEnemy
{
    void TakeDamage(int index, int dmg = 1);
}
