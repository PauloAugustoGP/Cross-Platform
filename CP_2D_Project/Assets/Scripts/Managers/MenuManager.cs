using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject main;
    public GameObject credits;
    public Image fade;

    private float timeFade;
    private float fadeRate = 0;
    private bool fadeScene = false;
    
    void Update()
    {
        if(fadeScene)
        {
            fadeRate += 0.01f;
            fade.color = new Color(0, 0, 0, fadeRate);

            if (fadeRate >= 1f)
                SceneManager.LoadScene("CutScene");
        }
    }

    public void FadeToCinematic()
    {
        fadeScene = true;
        fade.gameObject.SetActive(true);
    }

    public void GoToCredits()
    {
        main.SetActive(false);
        credits.SetActive(true);
    }

    public void GoToMain()
    {
        credits.SetActive(false);
        main.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
