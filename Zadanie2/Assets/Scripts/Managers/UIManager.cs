using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score;

    // Update is called once per frame
    public void UpdateScore(int currentScore)
    {
        _score.text = currentScore.ToString();
    }
}
