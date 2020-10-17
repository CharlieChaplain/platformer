using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunkinBlink : MonoBehaviour
{
    public Texture[] tex;

    private float timer = 4f;
    private bool blinking = false;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (blinking)
            {
                PlayerManager.Instance.playerTex.mainTexture = tex[0];
                timer = Random.Range(1f, 4f);
                blinking = false;
            }
            else
            {
                PlayerManager.Instance.playerTex.mainTexture = tex[1];
                timer = 0.2f;
                blinking = true;
            }   
        }
    }
}
