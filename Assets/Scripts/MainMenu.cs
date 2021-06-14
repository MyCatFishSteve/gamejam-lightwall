using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public bool creditsOpen = false;
    public GameObject credits;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        creditsOpen = true;
        credits.SetActive(true);
    }

    public void Update()
    {
        if(creditsOpen == true && Input.GetMouseButtonDown(0))
        {
            creditsOpen = false;
            credits.SetActive(false);
        }
    }

}