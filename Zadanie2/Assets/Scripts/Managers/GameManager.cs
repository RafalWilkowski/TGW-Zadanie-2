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

        _scoreManager.OnScoreChange += _uiManager.UpdateScore;
        _scoreManager.OnComboChange += _uiManager.UpdateCombo;
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
        public Action<int> OnComboChange;

        [SerializeField]
        private int _baseGemScore = 1000;
        [SerializeField]
        private int _baseComboScore = 500;
        [SerializeField]
        private float _comboFactor = 0.25f;

        public int CurrentScore { get ; private set ; }
        public int Combo { get; private set; }
        private ObjectColor _lastGemColor = ObjectColor.NONE;

        public void AddScore()
        {
            CurrentScore += _baseGemScore  + (int)(_baseComboScore * _comboFactor * Combo);
            OnScoreChange?.Invoke(CurrentScore);
        }

        public void CheckForCombo(ObjectColor color)
        {
            bool result = color == _lastGemColor;
            _lastGemColor = color;
            Combo = result ? Combo + 1 : 0;
            OnComboChange?.Invoke(Combo);
            AddScore();        
        }

    }
}



[System.Flags]
public enum ObjectColor { NONE = 0, RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }
