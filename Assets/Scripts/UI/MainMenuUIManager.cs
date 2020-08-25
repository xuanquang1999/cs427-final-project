using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public GameObject mainMenu;

    public void OpenLevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        mainMenu.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void StartGame(int levelId)
    {
        GameManager.currentLevelId = levelId;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
