using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _mainScore;
    private bool _firstGem = true;
    private bool flipNewScore = false;
    private Text _currentNewScore;
    [SerializeField]
    private Text _newScore1;
    [SerializeField]
    private Text _newScore2;
    [SerializeField]
    private Text _combo;
    [SerializeField]
    private GemColors[] colors;

    private void Start()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        _mainScore.text = 0.ToString();
        _newScore1.text = 0.ToString();
        _newScore2.text = 0.ToString();
        _currentNewScore = _newScore1;
        _newScore2.GetComponent<Animator>().Play("score_hidden");
        _combo.text = 0.ToString();
    }
    //public void SendComboScore(int cuu)
    public void UpdateScore(int currentScore)
    {
        _mainScore.text = currentScore.ToString();
        _mainScore.GetComponent<Animator>().Play("score_anim");
    }

    public void UpdateNewScore(int newScore)
    {
        _currentNewScore.text = newScore.ToString();
    }
    public void UpdateCombo(int currentCombo, ObjectColor color)
    {
        _combo.text = currentCombo.ToString();
        UpdateNewScorePanelColor(color);       
        _currentNewScore.GetComponent<Animator>().Play("score_anim");

    }
    public void GlideNewScore()
    {
        _currentNewScore.GetComponent<Animator>().Play("score_glide");
        FlipNewScoresText();
        _combo.text = 0.ToString();
    }
        private void UpdateNewScorePanelColor(ObjectColor color)
    {
        Color32 colorToDraw = new Color32();
        foreach (GemColors gemColor in colors)
        {
            if (gemColor.id == (int)color)
            {
                colorToDraw = gemColor.color;
                break;
            }
        }
        _combo.color = colorToDraw;
        _currentNewScore.color = colorToDraw;
    }
   private void FlipNewScoresText()
    {
        
        if (_currentNewScore == _newScore1)
        {
            _currentNewScore = _newScore2;
            
        }
        else
        {
            _currentNewScore = _newScore1;
        }
        _currentNewScore.gameObject.transform.SetSiblingIndex(3);
    }
}
[Serializable]
public struct GemColors
{
    public int id;
    public Color32 color;
}
