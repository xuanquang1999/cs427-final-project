using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int currentLevelId;

    private int coinCount;
    private int score;

    public GameObject player;
    public GameObject UIManager;
    public GameObject[] levels;

    void Start()
    {
        levels[currentLevelId].SetActive(true);
        coinCount = 0;
    }

    //void CompleteLevel()
    //{
    //    UIManager.GetComponent<GameUIManager>().CompleteLevel();
    //}

    void LoadLevel(int levelId)
    {
        currentLevelId = levelId;
        SceneManager.LoadScene("GameScene");
    }

    public void ReloadLevel()
    {
        LoadLevel(currentLevelId);
    }
}
