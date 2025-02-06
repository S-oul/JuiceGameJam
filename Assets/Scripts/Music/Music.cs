using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    List<AudioSource> sources = new List<AudioSource>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        sources = GetComponents<AudioSource>().ToList();
    }

    public IEnumerator ToNight()
    {
        float t = 0;
        while (t < 1)
        {
            sources[0].volume = Mathf.Lerp(0.05f, 0f, t);
            sources[1].volume = Mathf.Lerp(0f, 0.05f, t);

            t += Time.deltaTime;
            if (t >= 1) t = 1;
            yield return null;
        }
    }

    public IEnumerator ToJazz()
    {
        float t = 0;
        while (t < 1)
        {
            sources[0].volume = Mathf.Lerp(0f, 0.05f, t);
            sources[1].volume = Mathf.Lerp(0.05f, 0f, t);

            t += Time.deltaTime;
            if (t >= 1) t = 1;
            yield return null;
        }
    }

}
