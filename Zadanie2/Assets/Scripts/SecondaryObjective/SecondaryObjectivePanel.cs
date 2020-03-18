using System.Collections;
using UnityEngine;

public class SecondaryObjectivePanel : MonoBehaviour
{
	SecondaryObjectiveSocket[] allSockets = new SecondaryObjectiveSocket[0];
	[SerializeField] Transform emergingPanel;
	[SerializeField] Vector3 emergenceStartPoint, emergenceEndPoint;
	[SerializeField] float emergenceTime, submergeDelay;
	[SerializeField] GameObject[] artifacts;
    [SerializeField] SecondaryScoreText _secondaryScoreText;

	public bool IsActive { get; private set; } = false;

	int activeArtifactIndex = -1;
	// Start is called before the first frame update
	void Start()
	{
        _secondaryScoreText.gameObject.SetActive(false);

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
		IsActive = true;
		gameObject.SetActive(true);
		Debug.LogFormat("{0} was activated!", this);
		if (artifacts != null && artifacts.Length > 0)
		{
			activeArtifactIndex = Random.Range(0, artifacts.Length);
			artifacts[activeArtifactIndex]?.SetActive(true);
			Debug.LogFormat("Active artifact index set to {0}, item: {1}", activeArtifactIndex, artifacts[activeArtifactIndex]);
		}
		StartCoroutine(AnimatePanelCoroutine(emergenceStartPoint, emergenceEndPoint));
	}

	void Submerge()
	{
		if (!gameObject.activeInHierarchy) return;

		if (!emergingPanel)
		{
			gameObject.SetActive(false);
			return;
		}
		StartCoroutine(AnimatePanelCoroutine(emergenceEndPoint, emergenceStartPoint, true, submergeDelay));
	}

	IEnumerator AnimatePanelCoroutine(Vector3 positionStart, Vector3 positionEnd, bool deactivatePanel = false, float delay = 0)
	{
        if (!deactivatePanel)
        {
            _secondaryScoreText.gameObject.SetActive(true);
        }
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
		if (deactivatePanel)
		{
            _secondaryScoreText.ShowScore();
            while (_secondaryScoreText.gameObject.activeSelf)
            {
                yield return null;
            }
            gameObject.SetActive(false);
			if (artifacts != null && artifacts.Length > 0 && activeArtifactIndex >= 0)
			{
				artifacts[activeArtifactIndex]?.SetActive(false);
			}
			IsActive = false;
			Debug.LogFormat("{0} was deactivated", this);
		}
	}

}