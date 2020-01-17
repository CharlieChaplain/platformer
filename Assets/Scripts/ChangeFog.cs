using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFog : MonoBehaviour
{
    public float fogEnd;
    public float transitionSpeed;

    private float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StopCoroutine("blendFog");
            StartCoroutine("blendFog");
        }
    }

    IEnumerator blendFog()
    {
        time = 0.0f;
        float fogBegin = RenderSettings.fogEndDistance;
        while (time < transitionSpeed)
        {
            time += Time.deltaTime;
            RenderSettings.fogEndDistance = Mathf.Lerp(fogBegin, fogEnd, time / transitionSpeed);

            yield return null;
        }

        yield return null;
    }

}
