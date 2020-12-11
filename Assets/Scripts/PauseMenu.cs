using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; //the canvas element that has all the pause menu stuff on it

    public static bool isPaused; //can be accessed by other scripts

    public Animator menuAnim;
    public Animator pageAnim;

    private bool justPressed; //prevents pause spam

    public GameObject finger; //the finger cursor

    public Camera sceneCam; //accessed to change the post process value
    private Material sepiaMat;
    private float lerpTimer;
    private float dur = 0.15f;

    private enum MenuState { main, inv, stats }
    private MenuState currentState;
    public GameObject[] menus;

    public AudioClip openMenu;
    public AudioClip closeMenu;

    private DisplayInventory invDisplay;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        justPressed = false;
        currentState = MenuState.main;
        sepiaMat = sceneCam.GetComponent<ApplyPostProcess>().PPMat;

        sepiaMat.SetFloat("_amount", 0);

        invDisplay = pauseMenu.GetComponentInChildren<DisplayInventory>();
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
        for(int i = 1; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        menus[0].SetActive(true);
        menuAnim.SetTrigger("pause");
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
        menuAnim.SetTrigger("unpause");
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
            MenuScroll(1);

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MenuScroll(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            finger.transform.position = finger.transform.position + Vector3.down * 90f;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            finger.transform.position = finger.transform.position + Vector3.up * 90f;
        }

        //does stuff depending on which menu is active
        switch (currentState)
        {
            case MenuState.main:
                MainMenu();
                break;
            case MenuState.inv:
                InvMenu();
                break;
            case MenuState.stats:
                StatsMenu();
                break;
            default:
                break;
        }
    }

    void MenuScroll(int direction)
    {
        menus[(int)currentState].SetActive(false);
        pageAnim.gameObject.SetActive(true);
        if(direction > 0)
            pageAnim.Play("PageTurn");
        else if(direction < 0)
            pageAnim.Play("PageTurnReverse");
        StopCoroutine("WaitForPage");
        StartCoroutine(WaitForPage(pageAnim.GetCurrentAnimatorClipInfo(0)[0], direction));
    }

    IEnumerator WaitForPage(AnimatorClipInfo clip, int direction)
    {
        yield return new WaitForSecondsRealtime(clip.clip.length);
        pageAnim.gameObject.SetActive(false);

        if (direction > 0)
        {
            if (currentState == MenuState.stats)
            currentState = 0;
            else
                currentState++;
        }
        else if(direction < 0)
        {
            if (currentState == 0)
                currentState = MenuState.stats;
            else
                currentState--;
        }

        menus[(int)currentState].SetActive(true);
    }

    void MainMenu()
    {

    }
    void InvMenu()
    {
        //REPLACE KEYS WITH CONFIGURABLE VARIABLES AT SOME POINT
        if (Input.GetKeyDown(KeyCode.RightBracket))
            invDisplay.ChangeInventories(1);
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
            invDisplay.ChangeInventories(-1);
    }
    void StatsMenu()
    {

    }
}
