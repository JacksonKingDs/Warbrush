using UnityEngine;
using System.Collections;

public class OceanRainSplatter : MonoBehaviour
{
    public float lifeTime = 0.2f; //0.5f for splash
    SettingsAndPrefabRefs refs;

    public void Initialize()
    {
        refs = SettingsAndPrefabRefs.instance;
    }

    public void Activation(Vector3 pos)
    {
        transform.position = pos;
        StartCoroutine(DelayedDeactivation());
    }

    IEnumerator DelayedDeactivation()
    {
        yield return new WaitForSeconds(lifeTime);
        Deactivate();
    }

    void Deactivate()
    {
        refs.Push_RainSplatter(gameObject);
    }
}