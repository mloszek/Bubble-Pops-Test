using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Animator menuBackground;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject looseScreen;
    [SerializeField]
    private float timeForSwitchingView;

    private Coroutine scrollOutUIcoroutine;
    private Coroutine scrollINuIcoroutine;

    private readonly Vector3 BKG_DEFAULT_POS = new Vector3(0, 0, 1);
    private readonly string SCROLL_OUT_ANIM_TRIG = "hide";
    private readonly string SCROLL_IN_ANIM_TRIG = "show";

    private int currentScore;

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
        scoreText.text = "0";
        scoreText.enabled = true;

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

    public void SetWinScreen()
    {
        exitButton.SetActive(false);
        titleText.SetActive(false);
        backButton.SetActive(false);
        looseScreen.SetActive(false);
        winScreen.SetActive(true);
    }

    public void SetLooseScreen()
    {
        exitButton.SetActive(false);
        titleText.SetActive(false);
        backButton.SetActive(false);
        winScreen.SetActive(false);
        looseScreen.SetActive(true);
    }

    public void SetScoreText(int value)
    { 
        currentScore = int.Parse(scoreText.text.ToString());
        currentScore += value;
        scoreText.text = currentScore.ToString(); // I know it's terrible + there should be an event for updating score
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
