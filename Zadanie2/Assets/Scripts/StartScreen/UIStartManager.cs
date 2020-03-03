using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStartManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsCanvas;
    [SerializeField]
    private Text _hiscore;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Hiscore"))
        {
            int hiscore = PlayerPrefs.GetInt("Hiscore");
            _hiscore.text = "REWARD: " + hiscore;
        }
        else
        {
            PlayerPrefs.SetInt("Hiscore", 0);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
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