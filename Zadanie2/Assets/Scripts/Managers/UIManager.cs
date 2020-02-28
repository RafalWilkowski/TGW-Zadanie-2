using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _mainScore;
    [SerializeField]
    private Animator _mainScoreAnim;
    private bool _firstGem = true;
    private bool flipNewScore = false;
    private Text _currentNewScore;
    private Animator _currentNewScoreAnim;
    [SerializeField]
    private Text _newScore1;
    [SerializeField]
    private Animator _newScore1Anim;
    [SerializeField]
    private Text _newScore2;
    [SerializeField]
    private Animator _newScore2Anim;
    [SerializeField]
    private Text _combo;
    [SerializeField]
    private Animator _comboAnim;
    [SerializeField]
    private GemColors[] colors;
    [SerializeField]
    private Canvas gameOverPanel;
    [SerializeField]
    private Text _finalText;

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
        _currentNewScoreAnim = _newScore1Anim;
        _newScore1Anim.Play("score_hidden");
        _newScore2Anim.Play("score_hidden");
        _combo.text = 0.ToString();
        _comboAnim.Play("score_hidden");
    }
    //public void SendComboScore(int cuu)
    public void UpdateScore(int currentScore)
    {
        _mainScore.text = currentScore.ToString();
        _mainScoreAnim.Play("score_anim");
    }

    public void UpdateNewScore(int newScore)
    {
        _currentNewScore.text = newScore.ToString();
    }
    public void UpdateCombo(int currentCombo, ObjectColor color)
    {
        if (currentCombo != 0)
        {
            _comboAnim.Play("score_anim");
            _currentNewScoreAnim.Play("score_anim");
            _combo.text = "COMBO x " + (currentCombo + 1).ToString();
        } 
        else _combo.text = "" ;

        UpdateNewScorePanelColor(color);             
    }
    public void GlideNewScore()
    {
        if(_currentNewScore.text != "0" && GameManager.Instance._scoreManager.Combo != 0) _currentNewScoreAnim.Play("score_glide");

        FlipNewScoresText();

    }
    public void HideCombo()
    {
        _comboAnim.Play("score_hidden");
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
            _currentNewScoreAnim = _newScore2Anim;
            
        }
        else
        {
            _currentNewScore = _newScore1;
            _currentNewScoreAnim = _newScore1Anim;
        }
        _currentNewScore.gameObject.transform.SetSiblingIndex(3);
    }

    public void ActivateGameOverPanel(int finalScore)
    {
        gameOverPanel.gameObject.SetActive(true);
        _finalText.text = "GAME OVER \n \n YOUR SCORE IS : " + finalScore;
    }
}

[Serializable]
public struct GemColors
{
    public int id;
    public Color32 color;
}
