using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text _scoreText;
    private int _score = 0;

    private void Start()
    {
        _scoreText = GetComponent<Text>(); 
    }
    public void AddScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

   
}
