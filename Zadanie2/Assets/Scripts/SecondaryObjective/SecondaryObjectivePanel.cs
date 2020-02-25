using System.Collections;
using UnityEngine;

public class SecondaryObjectivePanel : MonoBehaviour
{
	SecondaryObjectiveSocket[] allSockets = new SecondaryObjectiveSocket[0];
	// Start is called before the first frame update
	void Start()
	{
		allSockets = GetComponentsInChildren<SecondaryObjectiveSocket>();

		StartCoroutine(WaitToSubscribe());
	}

	IEnumerator WaitToSubscribe()
	{
		while (!GameManager.Instance && !SecondaryObjectiveManager.Instance)
		{
			yield return new WaitForEndOfFrame();
		}
		SecondaryObjectiveManager.Instance.Subscribe(this);
		foreach (SecondaryObjectiveSocket socket in allSockets)
		{
			socket.OnGemInstalled += VerifyCompletion;
			socket.ResetSocket();
		}
		Activate(false);
	}

	public void Activate(bool status)
	{
		gameObject.SetActive(status);
	}

	void VerifyCompletion(Gem gem, SecondaryObjectiveSocket socket)
	{
		foreach (SecondaryObjectiveSocket s in allSockets)
		{
			if (s.IsActive && !s.IsFull)
				return;
		}
		
		SecondaryObjectiveManager.Instance?.CompleteSecondaryObjective();
	}

	public void ConfigureSocket(int index, int capacity, ObjectColor color)
	{
		allSockets[index].gameObject.SetActive(true);
		allSockets[index].InitializeGemSocket(capacity, color);
	}
	public void ResetAllActiveSockets()
	{
		foreach (SecondaryObjectiveSocket s in allSockets)
		{
			if (s.IsActive) s.ResetSocket();
		}
	}

	private void OnDestroy()
	{
		foreach (SecondaryObjectiveSocket socket in allSockets)
		{
			socket.OnGemInstalled -= VerifyCompletion;
		}
	}
}