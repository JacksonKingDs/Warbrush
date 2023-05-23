using UnityEngine;
using System.Collections;

public interface IPooledItem
{
    void Initialize(SettingsAndPrefabRefs refs);

    void Activate(Vector3 position);
}
