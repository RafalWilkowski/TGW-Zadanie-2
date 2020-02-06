using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        creditsCanvas.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}