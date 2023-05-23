using UnityEngine;
using System.Collections;

//Attach to audio prefabs
public class SelfDestroyAfter : MonoBehaviour
{
    public float length;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(length);
        Destroy(gameObject);
    }
}

