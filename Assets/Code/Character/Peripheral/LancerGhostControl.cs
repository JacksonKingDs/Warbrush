using UnityEngine;
using System.Collections;

public class LancerGhostControl : MonoBehaviour
{
    const float aliveTime = 0.5f;
    float counter;

    SpriteRenderer sprite;
    Color tankColor;
    Color curColor;
    float fadeSpeed = 5f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color tankColor)
    {
        //Reference the 
        this.tankColor = tankColor;
    }

    public void Reveal (Transform t)
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
        gameObject.SetActive(true);
        sprite.color = curColor = tankColor;

        counter = aliveTime;
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade ()
    {
        while (counter > 0f)
        {
            counter -= Time.deltaTime;

            //Fade out color
            curColor.a -= fadeSpeed * Time.deltaTime;
            sprite.color = curColor;

            yield return null;
        }

        //This automatically returns the object back to the object pool.
        gameObject.SetActive(false); 
    }
}
