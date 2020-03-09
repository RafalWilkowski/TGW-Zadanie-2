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
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.OnDestroyScene();
        SceneManager.LoadScene(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
    public void Unpause()
    {
        SceneManager.UnloadScene(3);
        Time.timeScale = 1;
    }
}
