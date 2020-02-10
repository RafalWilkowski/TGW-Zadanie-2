using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

public class PrototypeManager : MonoBehaviour
{
	public static PrototypeManager Instance;

	[field: SerializeField]
	public float BaseScore { get; private set; } = 100;

	[SerializeField]
	bool shouldRandomizeContainers = true;

	[SerializeField]
	private float[] _containersPositions;

	[SerializeField]
	List<Conteiner> _containers;

	[field: SerializeField]

	public int Combo { get; private set; } = 1;

	[SerializeField]
	private float _resetTime;

	private ColorType lastGemColor = ColorType.NONE;

	private void Awake()
	{
		Instance = this;
	}

	public IEnumerator ResetGame()
	{
		yield return new WaitForSeconds(_resetTime);
		SceneManager.LoadScene(0);
	}

	public void CurrentConteiner(ColorType color)
	{
		if (shouldRandomizeContainers)
		{
			RandomizeContainersOrder(color);
		}
		else
		{
			PushContainers(color);
		}
	}

	void RandomizeContainersOrder(ColorType color)
	{
		List<int> randomIntList = new List<int> { 0, 1, 2, 3 };
		//selected container
		int first = (int)color;
		//remove selectedcontainer from random list
		randomIntList.RemoveAt(first);

		Queue<int> queueConteiners = new Queue<int>();

		queueConteiners.Enqueue(first);
		//randomize conatiners
		int counter = randomIntList.Count;
		for (int i = 0; i < counter; i++)
		{
			int random = Random.Range(0, randomIntList.Count);
			queueConteiners.Enqueue(randomIntList[random]);
			randomIntList.Remove(randomIntList[random]);
		}

		//set conainers on positions
		for (int i = 0; i < _containers.Count; i++)
		{
			_containers[queueConteiners.Dequeue()].transform.position = new Vector3(_containersPositions[i], -3, 0);
		}
	}

	void PushContainers(ColorType color)
	{
		Conteiner selectedContainer = null;
		foreach (Conteiner c in _containers)
		{
			if (c.ContainerColor.Equals(color))
			{
				selectedContainer = c;
				break;
			}
		}
		if (!selectedContainer) return;

		_containers.Remove(selectedContainer);
		_containers.Insert(0, selectedContainer);

		for (int i = 0; i < _containers.Count; i++)
		{
			_containers[i].transform.position = new Vector3(_containersPositions[i], -3, 0);
		}
	}

	public bool CheckForCombo(Gem gem)
	{
		if (!gem) return false;
		
		bool result = gem.GetGemColor() == lastGemColor;
		lastGemColor = gem.GetGemColor();
		Combo = result ? Combo + 1 : 1;
		return result;
	}
}