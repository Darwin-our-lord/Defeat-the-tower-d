using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool pauseMenuUp = false;

    public GameObject MainMenu;
    public GameObject SettingsMenu;

    float LevelMenuOpen;

    // Update is called once per frame
    void Update()
    {

        //pauseMenu
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuUp && SceneManager.GetActiveScene().buildIndex != 0)

        {
            PauseMenu.SetActive(true);
            pauseMenuUp = true;
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuUp && SceneManager.GetActiveScene().buildIndex != 0)
        {
            PauseMenu.SetActive(false);
            pauseMenuUp = false;
            Time.timeScale = 1f;
        }


    }

    //also pause menu
    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void ReloadSceneButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //this shi is both dumbass
    public void QuitButton()
    {
        Application.Quit();
    }


    //main menu
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void BackButton()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

}
