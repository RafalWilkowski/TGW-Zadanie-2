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
        Conteiner[] containersOnGame = FindObjectsOfType<Conteiner>();
        foreach(Conteiner container in containersOnGame)
        {
            container.OnColorMatch += _scoreManager.CheckForCombo;
        }

        _scoreManager.OnComboChange += _uiManager.UpdateCombo;
        _scoreManager.OnScoreChange += _uiManager.UpdateNewScore;
        
    }
    public void AddScore()
    {
        _scoreManager.AddScore();
        _uiManager.UpdateScore(_scoreManager.CurrentScore);
    }

    [System.Serializable]
    private class ScoreManager
    {
        public Action<int> OnScoreChange;
        public Action<int,ObjectColor> OnComboChange;

        [SerializeField]
        private int _baseGemScore = 1000;
        [SerializeField]
        private int _baseComboScore = 500;
        [SerializeField]
        private float _comboFactor = 0.25f;

        public int CurrentScore { get ; private set ; }
        public int NewScore { get; private set; }
        public int Combo { get; private set; }
        private ObjectColor _lastGemColor = ObjectColor.NONE;

        public void AddScore()
        {
            NewScore += _baseGemScore  + (int)(_baseComboScore * _comboFactor * Combo);
            OnScoreChange?.Invoke(NewScore);
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
            }
            //Combo = combo ? Combo + 1 : 0;
            OnComboChange?.Invoke(Combo, color);
            AddScore();        
        }

    }
}



[System.Flags]
public enum ObjectColor { NONE = 0, RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }
