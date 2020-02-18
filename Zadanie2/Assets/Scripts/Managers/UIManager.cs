using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _combo;

    private void Start()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        _score.text = 0.ToString();
        _combo.text = 0.ToString();
    }

    public void UpdateScore(int currentScore)
    {
        _score.text = currentScore.ToString();
    }

    public void UpdateCombo(int currentCombo)
    {
        _combo.text = currentCombo.ToString();
    }
}
