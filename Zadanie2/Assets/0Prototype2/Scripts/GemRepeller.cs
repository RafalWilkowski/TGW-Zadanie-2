using System.Collections.Generic;
using UnityEngine;

public class GemRepeller : MonoBehaviour
{
	Gem ownGem;
	[SerializeField] RepelMode repelMode;
	[SerializeField] float repelAmplifier = 100;
	[SerializeField]List<Gem> repelledGems = new List<Gem>();
	// Start is called before the first frame update
	void Start()
	{
		ownGem = GetComponentInParent<Gem>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		foreach (Gem gem in repelledGems)
		{
			Repel(gem);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Gem gem = collision.GetComponentInParent<Gem>();
		if (gem && gem != ownGem)
		{
			RegisterGem(gem);
		}

	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		Gem gem = collision.GetComponentInParent<Gem>();
		if (gem && gem != ownGem)
		{
			UnregisterGem(gem);
		}
	}
	private void OnTriggerEnter(Collider collider)
	{
		Gem gem = collider.GetComponentInParent<Gem>();
		if (gem && gem != ownGem)
		{
			RegisterGem(gem);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		Gem gem = collider.GetComponentInParent<Gem>();
		if (gem && gem != ownGem)
		{
			UnregisterGem(gem);
		}
	}
	void RegisterGem(Gem gem)
	{
		if (gem && gem != ownGem && !repelledGems.Contains(gem))
		{
			repelledGems.Add(gem);
			if (!isActiveAndEnabled) enabled = true;
		}
	}

	void UnregisterGem(Gem gem)
	{
		if (gem && gem != ownGem && repelledGems.Contains(gem))
		{
			repelledGems.Remove(gem);
			if (repelledGems.Count == 0) enabled = false;
		}
	}
	private void Repel(Gem gem)
	{
		if (!gem) return;

		switch (repelMode)
		{
			case RepelMode.Radial:
				Vector3 vec = gem.transform.position - transform.position;
				gem.ApplyForce(vec.normalized * repelAmplifier / Mathf.Max(1f, vec.sqrMagnitude));
				break;
			case RepelMode.Horizontal:
				float deltaX = gem.transform.position.x - transform.position.x;
				gem.ApplyForce(Mathf.Sign(deltaX) * Vector3.right * repelAmplifier / Mathf.Max(1f, deltaX * deltaX));
				break;
		}
	}

	enum RepelMode
	{
		Radial,
		Horizontal
	}
}
