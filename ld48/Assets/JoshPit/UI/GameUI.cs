using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
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
        GameMan.gameMan.ResetPhases();
        mainMenu.SetActive(false);
    }

    public void Restart()
    {
        StartCoroutine(RestartFade());
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }
    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
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
