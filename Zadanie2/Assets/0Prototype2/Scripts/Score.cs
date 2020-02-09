using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	private Text _scoreText;
	[SerializeField] private Text _comboText;
	private int _score = 0;

	private void Start()
	{
		_scoreText = GetComponent<Text>();
	}
	public void AddScore()
	{
		_score += (int)(PrototypeManager.Instance.BaseScore * (1 + 0.5f * PrototypeManager.Instance.Combo));
		_scoreText.text = _score.ToString();
	}


}
