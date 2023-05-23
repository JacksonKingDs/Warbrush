using UnityEngine;
using System.Collections;

public class WalkDust : MonoBehaviour, IPooledItem
{
    public ParticleSystem pfx;
    SettingsAndPrefabRefs refs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize (SettingsAndPrefabRefs refs)
    {
        this.refs = refs;
    }

    public void Activate (Vector3 position)
    {
        // pfx.Simulate(1f);
        pfx.time = 0f;
        pfx.Play();
        transform.position = position;
        StartCoroutine(DelayedDeactivate());
    }

    IEnumerator DelayedDeactivate ()
    {
        yield return new WaitForSeconds(0.4f);
        refs.Push_WalkDust(gameObject);
    }
}
