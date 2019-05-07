using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Animator menuBackground;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private float timeForSwitchingView;

    private Coroutine scrollOutUIcoroutine;
    private Coroutine scrollINuIcoroutine;

    private readonly Vector3 BKG_DEFAULT_POS = new Vector3(0, 0, 1);
    private readonly string SCROLL_OUT_ANIM_TRIG = "hide";
    private readonly string SCROLL_IN_ANIM_TRIG = "show";

    private void Start()
    {
        exitButton.SetActive(true);
        titleText.SetActive(true);
        startButton.SetActive(true);
        backButton.SetActive(false);
        menuBackground.gameObject.GetComponent<Transform>().position = BKG_DEFAULT_POS;
    }

    public void StartGame()
    {
        exitButton.SetActive(false);
        titleText.SetActive(false);

        if (scrollOutUIcoroutine != null)
        {
            StopCoroutine(scrollOutUIcoroutine);
            scrollOutUIcoroutine = null;
        }

        scrollOutUIcoroutine = StartCoroutine(ScrollOut());
    }

    public void ShowMenu()
    {
        backButton.SetActive(false);
        menuBackground.SetTrigger(SCROLL_IN_ANIM_TRIG);

        if (scrollINuIcoroutine != null)
        {
            StopCoroutine(scrollINuIcoroutine);
            scrollINuIcoroutine = null;
        }

        scrollINuIcoroutine = StartCoroutine(ScrollIn());
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator ScrollOut()
    {
        yield return new WaitForSeconds(timeForSwitchingView);

        menuBackground.SetTrigger(SCROLL_OUT_ANIM_TRIG);
        startButton.SetActive(false);
        backButton.SetActive(true);
    }

    private IEnumerator ScrollIn()
    {
        yield return new WaitForSeconds(timeForSwitchingView);

        exitButton.SetActive(true);
        titleText.SetActive(true);
        startButton.SetActive(true);
    }
}
