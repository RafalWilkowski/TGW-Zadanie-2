using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField]
	private UIManager _uiManager;
	[SerializeField]
	public ScoreManager _scoreManager;
    [SerializeField]
    private Belt _belt;
    [SerializeField]
    private GemSpawner _gemSpawner;

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
        _scoreManager.OnMainScoreChange += _belt.CheckPointsThreshold;
        _scoreManager.OnMainScoreChange += _gemSpawner.CheckPointsThreshold;
		//update UI comboStripe             
		ComboStripe.OnTimeout += _uiManager.GlideNewScore;
		ComboStripe.OnTimeout += _scoreManager.BreakCombo;
		// UpdateTime combostripe
		ComboStripe comboStripe = FindObjectOfType<ComboStripe>();
		_scoreManager.OnTimeUpdate += comboStripe.UpdateTime;
		// container callback
		Conteiner.OnColorMatch += _scoreManager.CheckForCombo;
		//adding newScore to mainScore callback        
		NewScoreText.OnGlideFinished += _scoreManager.ScoreGlidedToMainScore;
		NewScoreText.OnGlideFinished += _uiManager.HideCombo;
	}

	public void GameOver()
	{
		//save highscore
		int currentHighScore = PlayerPrefs.GetInt("Hiscore");
		int currentGameScore = _scoreManager.CurrentScore;
		bool newHiscore = false;
		if (currentHighScore < currentGameScore)
		{
			//save new highscore
			PlayerPrefs.SetInt("Hiscore", currentGameScore);
			newHiscore = true;
		}
		_uiManager.ActivateGameOverPanel(currentGameScore, newHiscore);
		Time.timeScale = 0;
	}

	public void LoadStartLevel()
	{
		Time.timeScale = 1;
		OnDestroyScene();
		SceneManager.LoadScene(0);

	}

    public void OnDestroyScene()
	{
		_scoreManager.OnComboChange -= _uiManager.UpdateCombo;
		_scoreManager.OnNewScoreChange -= _uiManager.UpdateNewScore;
		_scoreManager.OnMainScoreChange -= _uiManager.UpdateScore;
        _scoreManager.OnMainScoreChange -= _belt.CheckPointsThreshold;
        _scoreManager.OnMainScoreChange -= _gemSpawner.CheckPointsThreshold;

        ComboStripe.OnTimeout -= _uiManager.GlideNewScore;
		ComboStripe.OnTimeout -= _scoreManager.BreakCombo;

		ComboStripe comboStripe = FindObjectOfType<ComboStripe>();
		_scoreManager.OnTimeUpdate -= comboStripe.UpdateTime;

		Conteiner.OnColorMatch -= _scoreManager.CheckForCombo;
      
		NewScoreText.OnGlideFinished -= _scoreManager.ScoreGlidedToMainScore;
		NewScoreText.OnGlideFinished -= _uiManager.HideCombo;
	}

	[System.Serializable]
	public class ScoreManager
	{
		public Action<int> OnNewScoreChange;
		public Action<int, ObjectColor> OnComboChange;
		public Action<int> OnMainScoreChange;
		public Action<bool> OnTimeUpdate;

		[SerializeField]
		private int _baseGemScore = 1000;
		[SerializeField]
		private int _baseComboScore = 500;
		[SerializeField]
		private float _comboFactor = 0.25f;

		public int CurrentScore { get; private set; }
		public int NewScore { get; private set; }
		private Queue<int> _scoreToAddQueue = new Queue<int>();
		private int _scoreToAdd = 0;
		public int Combo { get; private set; }
		private bool _comboBreakedByTimeout = true;
		private ObjectColor _lastGemColor = ObjectColor.NONE;

		public void AddMainScore(int scoreToAdd)
		{
			CurrentScore += scoreToAdd;
			OnMainScoreChange?.Invoke(CurrentScore);
		}

		public void AddNewScore()
		{
			NewScore += _baseGemScore + (int)(_baseComboScore * _comboFactor * Combo);
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
				// update time UI combostripe
				OnTimeUpdate?.Invoke(false);
				//update score UI elements
				OnComboChange?.Invoke(Combo, color);
			}
			else
			{
				if (!_comboBreakedByTimeout)
				{
					// break combo and glide UI score to mainscore
					ComboStripe.OnTimeout?.Invoke();
				}
				_comboBreakedByTimeout = false;
				AddMainScore(_baseGemScore);
				_lastGemColor = color;
				//// update time UI combostripe
				OnTimeUpdate?.Invoke(true);
			}
		}

		public void BreakCombo()
		{
			_comboBreakedByTimeout = true;
			Combo = 0;
			if (NewScore != 0)
			{
				_scoreToAddQueue.Enqueue(NewScore);
				NewScore = 0;
			}
		}

		public void ScoreGlidedToMainScore()
		{
			int scoreGlided = _scoreToAddQueue.Dequeue();
			AddMainScore(scoreGlided);
		}
	}
}

[System.Flags]
public enum ObjectColor { NONE = 0, RED = 1, YELLOW = 2, GREEN = 4, BLUE = 8 }