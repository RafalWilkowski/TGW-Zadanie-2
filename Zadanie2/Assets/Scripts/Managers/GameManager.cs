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
    public ScoreManager _scoreManager;

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
        
        ComboStripe.OnTimeout += _uiManager.GlideNewScore;
        ComboStripe.OnTimeout += _scoreManager.BreakCombo;

        // container callback
        Conteiner.OnColorMatch += _scoreManager.CheckForCombo;
        ComboStripe comboStripe = FindObjectOfType<ComboStripe>();
        Conteiner.OnColorMatched += comboStripe.UpdateTime;
        
        //adding newScore to mainScore callback        
        NewScoreText.OnGlideFinished += _scoreManager.AddMainScore;
        NewScoreText.OnGlideFinished += _uiManager.HideCombo;
    }

    [System.Serializable]
    public class ScoreManager
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
        private bool _comboBreakedByTimeout = true;
        private ObjectColor _lastGemColor = ObjectColor.NONE;

        public void AddMainScore()
        {
            CurrentScore += _scoreToAdd;
            //_scoreToAdd = 0;
            OnMainScoreChange?.Invoke(CurrentScore);
        }
		public void AddMainScore(int score)
		{
			CurrentScore += score;
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
            
            if (combo && !_comboBreakedByTimeout)
            {
                Combo++;
                AddNewScore();

                _lastGemColor = color;

                OnComboChange?.Invoke(Combo, color);
               
            }
            else
            {
                if (!_comboBreakedByTimeout)//if(_lastGemColor != ObjectColor.NONE || !_comboBreaked)
                {
                    ComboStripe.OnTimeout?.Invoke();
                    //_scoreToAdd = NewScore;
                }
                _comboBreakedByTimeout = false;
                //AddNewScore();
                CurrentScore += 100;
                OnMainScoreChange?.Invoke(CurrentScore);
                //NewScore = 0;
                AddMainScore();
                _lastGemColor = color;

               // OnComboChange?.Invoke(Combo, color);
            }
            
           

        }

        public void BreakCombo()
        {
            _comboBreakedByTimeout = true;
            Combo = 0;
            _scoreToAdd = NewScore;
            NewScore = 0;
        }
    }
}



[System.Flags]
public enum ObjectColor { NONE = 0, RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }
