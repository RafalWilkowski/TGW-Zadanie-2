using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryObjectiveManager : MonoBehaviour
{
	public bool IsActive { get => ObjectivePanel && ObjectivePanel.IsActive; }
	public static SecondaryObjectiveManager Instance { get; private set; }
	public SecondaryObjectivePanel ObjectivePanel { get; private set; }
    private bool _missionCompleted = false;
	[SerializeField] int baseInterval = 10000;
	[SerializeField] int intervalPerCapacity = 10000;

	[SerializeField] int baseRetryCost = 9000;
	[SerializeField] int retryCostPerLevel = 1000;

	[SerializeField] int rewardPerCapacity = 3000;
	[SerializeField] int rewardPerLevel = 3000;

	[SerializeField] float baseTimeToComplete = 10f;
	[SerializeField] float extraTimeToCompletePerCapacity = 2.5f;

	[SerializeField] float minSocketCapacityScaling = 26;
	[SerializeField] float maxSocketCapacityScaling = 17;
	[SerializeField] float maxSocketCountScaling = 4;
	[SerializeField] float capacityProgressionRate = 2;

	[SerializeField] AudioClip emergenceClip, completionClip, failureClip;
	[SerializeField] AudioSource audioSource;

	[SerializeField] SpriteAssignment[] assignedSprites;
    [SerializeField] private SecondaryScoreText _secondaryScoreText;
    private AudioSource _clockAudio;

	int nextScoreRequirement = 0;

	float objectiveTimer;
	Slider objectiveTimeSlider;
	int totalObjectives = 1;
	int secondaryObjectiveLevel = 1;
	int totalCapacity = 3;
	int maxSockets = 3;
	int minCapacity = 1;
	int maxCapacity = 3;


	void CalculateRequiredScore()
	{
		nextScoreRequirement += secondaryObjectiveLevel * baseInterval + (totalCapacity - 2) * intervalPerCapacity;
	}

	void CalculateRetryScore()
	{
		nextScoreRequirement += baseRetryCost + retryCostPerLevel * secondaryObjectiveLevel;
	}

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;

		if (objectiveTimeSlider)
		{
			objectiveTimeSlider.value = objectiveTimeSlider.maxValue = baseTimeToComplete;
        }

		StartCoroutine(WaitForGameManager());
		CalculateRequiredScore();
		if (audioSource)
		{
			audioSource.playOnAwake = false;
			audioSource.loop = false;
		}
	}

	IEnumerator WaitForGameManager()
	{
		while (!GameManager.Instance)
			yield return new WaitForEndOfFrame();

		GameManager.Instance._scoreManager.OnNewScoreChange += InitiateSecondaryObjective;
	}

	private void OnDestroy()
	{
		if (GameManager.Instance)
			GameManager.Instance._scoreManager.OnNewScoreChange -= InitiateSecondaryObjective;
        if(ObjectivePanel)
            ObjectivePanel.OnPanelDeactivated -= _secondaryScoreText.ShowScore;
    }

	void InitiateSecondaryObjective(int score)
	{
		if (!ObjectivePanel || IsActive || GameManager.Instance._scoreManager.CurrentScore < nextScoreRequirement) return;
        _missionCompleted = false;
        
        ObjectivePanel.Activate(true);
		ObjectivePanel.ResetAllActiveSockets();
        _clockAudio.Play();
        List<int> indices = new List<int>(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
		int availableCapacity = totalCapacity;

		for (int i = 0; i < maxSockets && availableCapacity > 0; i++)
		{
			int index = Random.Range(0, 8 - i);

			int targetCapacity = i == maxSockets - 1 ? availableCapacity :
				(maxSockets - i) * minCapacity < availableCapacity ? Mathf.Min(maxCapacity, availableCapacity / (maxSockets - i))
				: minCapacity;
			availableCapacity -= targetCapacity;

			ObjectivePanel.ConfigureSocket(indices[index], targetCapacity, GetRandomGemColor());
			indices.RemoveAt(index);
		}
		PlaySound(emergenceClip);
		StartObjectiveTimer(baseTimeToComplete + (totalCapacity - 3) * extraTimeToCompletePerCapacity);
	}

	public void CompleteSecondaryObjective()
	{
		if (!ObjectivePanel) return;

        _missionCompleted = true;
        _secondaryScoreText.MissionComleted((totalCapacity - 2) * rewardPerCapacity + rewardPerLevel * secondaryObjectiveLevel);

		maxSockets = Mathf.Min(9, Mathf.FloorToInt((3 + Mathf.FloorToInt((secondaryObjectiveLevel - 1) / maxSocketCountScaling)) / minCapacity));
		secondaryObjectiveLevel += 1;
		totalCapacity = 3 + Mathf.FloorToInt((secondaryObjectiveLevel - 1) / maxSocketCountScaling);
		minCapacity = Mathf.FloorToInt(secondaryObjectiveLevel / minSocketCapacityScaling) + 1;
		maxCapacity = Mathf.FloorToInt(secondaryObjectiveLevel / maxSocketCapacityScaling) + 3;
		CalculateRequiredScore();

		PlaySound(completionClip);

		EndSecondaryObjective("Secondary mission was COMPLETED!");
	}

	public void FailSecondaryObjective()
	{
		if (!ObjectivePanel) return;
		CalculateRetryScore();

		PlaySound(failureClip);
		ObjectivePanel.ResetAllActiveSockets();
		EndSecondaryObjective("Secondary mission was FAILED!");
	}

	void EndSecondaryObjective(string optionalMessage = "")
	{
		if (!string.IsNullOrEmpty(optionalMessage))
			Debug.Log(optionalMessage);

		totalObjectives += 1;

		ObjectivePanel.Activate(false);
	}
	public void Subscribe(SecondaryObjectivePanel panel)
	{
		ObjectivePanel = panel;
		if (ObjectivePanel)
		{
			objectiveTimeSlider = ObjectivePanel.GetComponentInChildren<Slider>();
            ObjectivePanel.OnPanelDeactivated += _secondaryScoreText.ShowScore;
            _clockAudio = objectiveTimeSlider.GetComponent<AudioSource>();
        }
	}

	ObjectColor GetRandomGemColor()
	{
		return
			new ObjectColor[4]
			{ObjectColor.RED, ObjectColor.BLUE, ObjectColor.GREEN, ObjectColor.YELLOW}
			[Random.Range(0, 4)];
	}

	void StartObjectiveTimer(float time)
	{
		if (time <= 0) return;
		ResetSlider(time);
		StartCoroutine(UpdateObjectiveTimer(time));
	}

	void ResetSlider(float toValue)
	{
		if (!objectiveTimeSlider) return;
		objectiveTimeSlider.maxValue = objectiveTimeSlider.value = toValue;
	}

	void UpdateSlider(float currentValue)
	{
		if (!objectiveTimeSlider) return;
		objectiveTimeSlider.value = Mathf.Max(0, currentValue);
	}

	IEnumerator UpdateObjectiveTimer(float time)
	{
		float startTime = Time.time;
		while (IsActive && !_missionCompleted)
		{
			if (Time.time >= startTime + time)
			{
				objectiveTimeSlider.value = 0;
				FailSecondaryObjective();
				yield break;
			}
			yield return null;
			UpdateSlider(time + startTime - Time.time);
		}
        _clockAudio.Stop();
    }

    void PlaySound(AudioClip soundClip)
	{
		if (!audioSource || !soundClip) return;
		audioSource.PlayOneShot(soundClip);
	}

	public Sprite GetAssignedSprite(ObjectColor color)
	{
		if (assignedSprites == null || assignedSprites.Length == 0) return null;
		foreach (SpriteAssignment spriteAssignment in assignedSprites)
		{
			if (color == spriteAssignment.Color)
				return spriteAssignment.GetSpriteByColor(color);
		}

		return null;
	}

	[System.Serializable]
	class SpriteAssignment
	{
		[SerializeField] ObjectColor color;
		[SerializeField] Sprite assignedSprite;
		public ObjectColor Color { get => color; }
		public Sprite GetSpriteByColor(ObjectColor col)
		{
			if (col.Equals(color))
			{
				return assignedSprite;
			}
			return null;
		}
	}
}