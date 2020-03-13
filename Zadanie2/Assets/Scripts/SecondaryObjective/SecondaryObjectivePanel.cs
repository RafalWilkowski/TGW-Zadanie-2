using System.Collections;
using UnityEngine;

public class SecondaryObjectivePanel : MonoBehaviour
{
	SecondaryObjectiveSocket[] allSockets = new SecondaryObjectiveSocket[0];
	[SerializeField] Transform emergingPanel;
	[SerializeField] Vector3 emergenceStartPoint, emergenceEndPoint;
	[SerializeField] float emergenceTime, submergeDelay;
	// Start is called before the first frame update
	void Start()
	{
		allSockets = GetComponentsInChildren<SecondaryObjectiveSocket>();

		StartCoroutine(WaitToSubscribe());
	}

	IEnumerator WaitToSubscribe()
	{
		while (!GameManager.Instance || !SecondaryObjectiveManager.Instance)
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
		if (status)
		{
			Emerge();
		}
		else
		{
			Submerge();
		}
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

	void Emerge()
	{
		if (!emergingPanel) return;
		gameObject.SetActive(true);
		StartCoroutine(AnimatePanelCoroutine(emergenceStartPoint, emergenceEndPoint));
	}

	void Submerge()
	{

		if (!emergingPanel)
		{
			gameObject.SetActive(false);
			return;
		}
		StartCoroutine(AnimatePanelCoroutine(emergenceEndPoint, emergenceStartPoint, true, submergeDelay));
	}

	IEnumerator AnimatePanelCoroutine(Vector3 positionStart, Vector3 positionEnd, bool deactivatePanel = false, float delay = 0)
	{
		if ((emergingPanel.localPosition - positionEnd).sqrMagnitude > 1)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);

			float emergenceTimeStart = Time.time;
			while (Time.time - emergenceTimeStart < emergenceTime)
			{
				emergingPanel.localPosition = Vector3.Lerp(positionStart, positionEnd, (Time.time - emergenceTimeStart) / emergenceTime);
				yield return new WaitForEndOfFrame();
			}
		}
		emergingPanel.localPosition = positionEnd;
		if (deactivatePanel) gameObject.SetActive(false);
	}

}