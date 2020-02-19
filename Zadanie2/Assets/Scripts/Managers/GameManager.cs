using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private ScoreManager _scoreManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //update UI callbacks
        _scoreManager.OnComboChange += _uiManager.UpdateCombo;
        _scoreManager.OnNewScoreChange += _uiManager.UpdateNewScore;
        _scoreManager.OnMainScoreChange += _uiManager.UpdateScore;
        //update UI comboStripe
        ComboStripe comboStripe = FindObjectOfType<ComboStripe>();
        
        // container callback
        Conteiner[] containersOnGame = FindObjectsOfType<Conteiner>();
        foreach(Conteiner container in containersOnGame)
        {
            container.OnColorMatch += _scoreManager.CheckForCombo;
            container.OnColorMatched += comboStripe.UpdateTime;
        }

        

        //adding newScore to mainScore callback
        NewScoreText [] newScores = FindObjectsOfType<NewScoreText>();
        foreach(NewScoreText newScoreText in newScores)
        {
            newScoreText.OnGlideFinished += _scoreManager.AddMainScore;
        }
        
    }

    [System.Serializable]
    private class ScoreManager
    {
        public Action<int> OnNewScoreChange;
        public Action<int,ObjectColor> OnComboChange;
        public Action<int> OnMainScoreChange;

        [SerializeField]
        private int _baseGemScore = 1000;
        [SerializeField]
        private int _baseComboScore = 500;
        [SerializeField]
        private float _comboFactor = 0.25f;

        public int CurrentScore { get ; private set ; }
        public int NewScore { get; private set; }
        private int _scoreToAdd = 0;
        public int Combo { get; private set; }
        private ObjectColor _lastGemColor = ObjectColor.NONE;

        public void AddMainScore()
        {
            CurrentScore += _scoreToAdd;
            _scoreToAdd = 0;
            OnMainScoreChange?.Invoke(CurrentScore);
        }
        public void AddNewScore()
        {
            NewScore += _baseGemScore  + (int)(_baseComboScore * _comboFactor * Combo);
            OnNewScoreChange?.Invoke(NewScore);
        }

        public void CheckForCombo(ObjectColor color)
        {
            
            bool combo = color == _lastGemColor;
            _lastGemColor = color;
            if (combo)
            {
                Combo++;
    
            }
            else
            {
                Combo = 0;
                _scoreToAdd = NewScore;
                NewScore = 0;
            }
            //Combo = combo ? Combo + 1 : 0;
            OnComboChange?.Invoke(Combo, color);
            AddNewScore();        
        }

    }
}



[System.Flags]
public enum ObjectColor { NONE = 0, RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }
