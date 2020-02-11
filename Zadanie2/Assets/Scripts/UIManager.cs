using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private Text score;

    private int scoreCounter = 0;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        score.text = scoreCounter.ToString();
    }

    // Update is called once per frame
    public void AddScore()
    {
        scoreCounter++;
        score.text = scoreCounter.ToString();
    }
}
