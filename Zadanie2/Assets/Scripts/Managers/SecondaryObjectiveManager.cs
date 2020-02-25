using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryObjectiveManager : MonoBehaviour
{
	public bool IsActive { get; private set; }
	public static SecondaryObjectiveManager Instance { get; private set; }
	public SecondaryObjectivePanel ObjectivePanel { get; private set; }

	[SerializeField] int emergenceScoreStep = 50000;
	[SerializeField] int completionScore = 20000;
	[SerializeField] float baseTimeToComplete = 10f;
	[SerializeField] float extraTimeToCompletePerCapacity = 2.5f;
	float objectiveTimer;
	Slider objectiveTimeSlider;
	int totalObjectives = 0;
	int secondaryObjectiveLevel = 1;
	int totalCapacity = 3;
	int maxSockets = 3;
	int minCapacity = 1;
	int maxCapacity = 3;

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
		
		if (objectiveTimeSlider)
		{
			objectiveTimeSlider.value = objectiveTimeSlider.maxValue = baseTimeToComplete;
		}

		StartCoroutine(WaitForGameManager());
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
	}

	void InitiateSecondaryObjective(int score)
	{
		if (!ObjectivePanel || IsActive || GameManager.Instance._scoreManager.CurrentScore < emergenceScoreStep * totalObjectives) return;
		IsActive = true;
		ObjectivePanel.Activate(IsActive);
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

		StartObjectiveTimer(baseTimeToComplete + (totalCapacity - 3) * extraTimeToCompletePerCapacity);
	}

	public void CompleteSecondaryObjective()
	{
		if (!ObjectivePanel) return;

		GameManager.Instance._scoreManager.AddMainScore(completionScore);

		maxSockets = Mathf.FloorToInt((3 + Mathf.FloorToInt((secondaryObjectiveLevel - 1) / 3)) / minCapacity);
		secondaryObjectiveLevel += 1;
		totalCapacity = 3 + Mathf.FloorToInt((secondaryObjectiveLevel - 1) / 2);
		minCapacity = Mathf.FloorToInt(secondaryObjectiveLevel / 15) + 1;
		maxCapacity = Mathf.FloorToInt(secondaryObjectiveLevel / 6) + 3;

		EndSecondaryObjective("Secondary mission was COMPLETED!");
	}

	public void FailSecondaryObjective()
	{
		if (!ObjectivePanel) return;
		EndSecondaryObjective("Secondary mission was FAILED!");
	}

	void EndSecondaryObjective(string optionalMessage = "")
	{
		if (!string.IsNullOrEmpty(optionalMessage))
			Debug.Log(optionalMessage);

		totalObjectives += 1;
		ObjectivePanel.ResetAllActiveSockets();
		ObjectivePanel.Activate(false);
		IsActive = false;
	}
	public void Subscribe(SecondaryObjectivePanel panel)
	{
		ObjectivePanel = panel;
		if (ObjectivePanel)
		{
			objectiveTimeSlider = ObjectivePanel.GetComponentInChildren<Slider>();
		}
	}

	ObjectColor GetRandomGemColor()
	{
		return
			new ObjectColor[4]
			{ObjectColor.RED, ObjectColor.BLUE, ObjectColor.GREEN, ObjectColor.YELLOW}
			[Random.Range(0, 3)];
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

	void UpdateSlider( float currentValue)
	{
		if (!objectiveTimeSlider) return;
		objectiveTimeSlider.value = Mathf.Max(0, currentValue);
	}

	IEnumerator UpdateObjectiveTimer(float time)
	{
		float startTime = Time.realtimeSinceStartup;
		while (IsActive)
		{
			if (Time.realtimeSinceStartup >= startTime + time)
			{
				objectiveTimeSlider.value = 0;
				FailSecondaryObjective();
				yield break;
			}
			yield return new WaitForFixedUpdate();
			UpdateSlider(time + startTime - Time.realtimeSinceStartup);
		}
	}
}