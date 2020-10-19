using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; //the canvas element that has all the pause menu stuff on it

    public static bool isPaused; //can be accessed by other scripts

    public Animator anim;

    private bool justPressed; //prevents pause spam

    private enum MenuState { main, inv }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        justPressed = false;
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
        anim.SetTrigger("pause");
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void Unpause()
    {
        anim.SetTrigger("unpause");
        isPaused = false;
        Time.timeScale = 1f;
        StopCoroutine("WaitForUnpause");
        StartCoroutine("WaitForUnpause");
    }

    IEnumerator WaitForUnpause()
    {
        yield return new WaitForSeconds(.6f);
        pauseMenu.SetActive(false);
    }

    //handles all interaction with menus
    void MenuNav()
    {

    }
}
