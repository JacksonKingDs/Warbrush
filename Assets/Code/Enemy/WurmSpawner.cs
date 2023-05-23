using UnityEngine;
using System.Collections;

public class WurmSpawner : MonoBehaviour
{
    public GameObject pf_wurm;
    public GameObject pf_wurmBody;
    SpriteRenderer rend;
    float xLimit = 3.5f;
    float yLimit = 1.5f;
    float spawnWurmInterval = 12f;
    Vector3 spinnerRotation = new Vector3(0f, 0f, 2f);
    int count;

    IEnumerator Start()
    {
        rend = GetComponent<SpriteRenderer>();

        while (true)
        {
            yield return new WaitForSeconds(spawnWurmInterval);
            spawnWurmInterval += 10f;
            count += 1;
            StartCoroutine(WurmSpawn());
            if (count >= 3)
            {
                yield break;
            }
        }
    }

    IEnumerator WurmSpawn ()
    {
        transform.position = new Vector3(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit), -0.05f);
        rend.enabled = true;
        yield return new WaitForSeconds(2.5f);
        //for (int i = 0; i < 50; i++)
        //{
        //    yield return new WaitForSeconds(0.05f);
        //    //transform.Rotate(spinnerRotation);
        //}
        rend.enabled = false;

        Vector3 v = transform.position;
        v.z = -0.02f;
        transform.position = v;

        GameObject prevPart = Instantiate(pf_wurm, transform.position, Quaternion.identity) as GameObject;
        for (int i = 0; i < 4; i++)
        {
            GameObject newPart = Instantiate(pf_wurmBody, transform.position, Quaternion.identity) as GameObject;
            newPart.GetComponent<Enemy_WormBody>().leader = prevPart.transform;
            prevPart = newPart;
        }
    }
}
