using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void GoHome()
    {
        Time.timeScale = 1;
        GameManager.Instance.OnDestroyScene();
        SceneManager.LoadScene("StartScreen");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.OnDestroyScene();
        SceneManager.LoadScene("GameScene");      
    }
    public void Unpause()
    {
        this.gameObject.SetActive(false);
        TouchDetector.Instance.EnableInGameTouch();
        Time.timeScale = 1;
    }
}
