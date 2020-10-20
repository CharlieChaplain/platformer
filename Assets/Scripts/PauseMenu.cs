﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; //the canvas element that has all the pause menu stuff on it

    public static bool isPaused; //can be accessed by other scripts

    public Animator anim;

    private bool justPressed; //prevents pause spam

    public GameObject finger; //the finger cursor

    private enum MenuState { main, inv, stats }
    private MenuState currentState;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        justPressed = false;
        currentState = MenuState.main;
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
        yield return new WaitForSeconds(.3f);
        pauseMenu.SetActive(false);
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
