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
            container.OnColorMatch += AddScore;
        }
    }
    public void AddScore()
    {
        _scoreManager.AddScore();
        _uiManager.UpdateScore(_scoreManager.CurrentScore);
    }

    [System.Serializable]
    private class ScoreManager
    {
        [SerializeField]
        private int baseGemScore = 1000;
        public int CurrentScore { get ; private set ; }
        private int newScore = 0;
        public int Combo { get; private set; }

        public void AddScore()
        {
            CurrentScore += baseGemScore;
        }

    }
}



[System.Flags]
public enum ObjectColor { RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }
