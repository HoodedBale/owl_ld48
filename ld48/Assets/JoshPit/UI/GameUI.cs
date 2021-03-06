using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public GameObject credits;
    public ResetFade resetFade;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(StartGame2());
    }

    private IEnumerator StartGame2()
    {
        yield return new WaitForSeconds(0.5f);
        GameMan.gameMan.ResetPhases();
        mainMenu.SetActive(false);
        MusicMan.musicMan.PlayGameMusic();
    }

    public void Restart()
    {
        StartCoroutine(RestartFade());
    }

    public void OpenOptions()
    {
        StartCoroutine(OpenOptions2());
    }

    private IEnumerator OpenOptions2()
    {
        yield return new WaitForSeconds(0.5f);
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void OpenCredits()
    {
        StartCoroutine(OpenCredits2());
    }

    private IEnumerator OpenCredits2()
    {
        yield return new WaitForSeconds(0.5f);
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
    }

    public void CloseCredits()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
    }

    public void PointerHover()
    {
        SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.HOVER);
    }

    public void PointerClick()
    {
        SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.CLICK);
    }

    IEnumerator RestartFade()
    {
        resetFade.StartFade();

        while (!resetFade.reset)
        {
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }


}
