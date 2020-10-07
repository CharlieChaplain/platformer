using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    public CanvasGroup irisWipe;

    public List<GameObject> irisComponents;

    private bool coroutineActive = false;
    private int toggle = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            TriggerIrisWipe();
    }

    public void TriggerIrisWipe()
    {
        if (!coroutineActive)
        {
            StopCoroutine("IrisWipe");
            StartCoroutine("IrisWipe");
            coroutineActive = true;
        }
    }


    IEnumerator IrisWipe()
    {
        irisWipe.alpha = 1.0f;
        float scale = 220.0f;
        float maskOffset = 675.0f;

        //for loop will go between 0 and 100 incrementing if shrinking the iris, and between 100 and 0 decrementing if growing
        //the 50s are there because it is half of 100
        for (int i = (-50 * toggle) + 50; toggle * i < (50 * toggle) + 50; i += toggle)
        {
            //linear
            //scale = 220.0f - 2.2f * i;

            //weird quadratic thing. Makes the shrink look more linear as it gets smaller
            scale = 1.0f / 43.0f * (-i + 100.0f) * (-i + 100.0f);
            //maskoffset snaps the 4 cardinal masks around the iris texture to it's boundary (minus one for a small bit of overlap)
            maskOffset = irisComponents[0].GetComponent<RectTransform>().rect.width / 2.0f * scale + 349.0f;


            irisComponents[0].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
            irisComponents[1].GetComponent<RectTransform>().localPosition = new Vector3(0, maskOffset, 0);
            irisComponents[2].GetComponent<RectTransform>().localPosition = new Vector3(-maskOffset, 0, 0);
            irisComponents[3].GetComponent<RectTransform>().localPosition = new Vector3(maskOffset, 0, 0);
            irisComponents[4].GetComponent<RectTransform>().localPosition = new Vector3(0, -maskOffset, 0);

            yield return new WaitForSeconds(0.0167f);
        }
        toggle *= -1;
        coroutineActive = false;
    }
}
