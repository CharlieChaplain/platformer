using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; //the canvas element that has all the pause menu stuff on it

    public static bool isPaused; //can be accessed by other scripts

    public Animator anim;

    private bool justPressed; //prevents pause spam

    public GameObject finger; //the finger cursor

    public Camera sceneCam; //accessed to change the post process value
    private Material sepiaMat;
    private float lerpTimer;
    private float dur = 0.15f;

    private enum MenuState { main, inv, stats }
    private MenuState currentState;

    public AudioClip openMenu;
    public AudioClip closeMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        justPressed = false;
        currentState = MenuState.main;
        sepiaMat = sceneCam.GetComponent<ApplyPostProcess>().PPMat;

        sepiaMat.SetFloat("_amount", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)) && !justPressed)
        {
            justPressed = true;
            if (isPaused)
            {
                Unpause();
            }
            else if (!isPaused)
            {
                Pause();
            }
        }

        if (justPressed && !(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Joystick1Button7)))
            justPressed = false;

        if (isPaused)
        {
            MenuNav();
        }
    }

    void Pause()
    {
        currentState = MenuState.main;
        anim.SetTrigger("pause");
        SoundManager.Instance.PlaySound(openMenu, PlayerManager.Instance.player.transform.position);
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        StopCoroutine("LerpPP");
        lerpTimer = 0f;
        StartCoroutine(LerpPP(0, 1f, dur));
    }

    void Unpause()
    {
        anim.SetTrigger("unpause");
        SoundManager.Instance.PlaySound(closeMenu, PlayerManager.Instance.player.transform.position);
        isPaused = false;
        Time.timeScale = 1f;
        StopCoroutine("WaitForUnpause");
        StartCoroutine("WaitForUnpause");

        StopCoroutine("LerpPP");
        lerpTimer = 0f;
        StartCoroutine(LerpPP(1f, 0, dur));
    }

    IEnumerator WaitForUnpause()
    {
        yield return new WaitForSeconds(.3f);
        pauseMenu.SetActive(false);
    }

    IEnumerator LerpPP(float start, float end, float duration)
    {
        while(lerpTimer < duration)
        {
            lerpTimer += Time.unscaledDeltaTime;
            float amount = Mathf.Lerp(start, end, lerpTimer / duration);
            sepiaMat.SetFloat("_amount", amount);
            yield return null;
        }
    }

    //handles all interaction with menus
    void MenuNav()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentState == MenuState.stats)
                currentState = 0;
            else
                currentState++;

            anim.SetTrigger("nextMenu");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentState == 0)
                currentState = MenuState.stats;
            else
                currentState--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            finger.transform.position = finger.transform.position + Vector3.down * 90f;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            finger.transform.position = finger.transform.position + Vector3.up * 90f;
        }
    }
}
