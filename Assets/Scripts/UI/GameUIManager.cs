using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public GameObject diePanel;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void CompleteLevel()
    {
        victoryPanel.SetActive(true);
    }

    public void PlayerDie()
    {
        diePanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }
}
