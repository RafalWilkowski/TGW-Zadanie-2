using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GemSpawner : MonoBehaviour
{
	public static GemSpawner Instance;

	[SerializeField]
	private bool _spawnStones = true;
	[SerializeField]
	private bool _spawnDynamites = true;
	[SerializeField]
	private float _gemSpawnRate = 1f;
	private float _spawnCooldown = 0;

	[SerializeField]
	private float _spawnStoneProbability = 5f;
	private bool _spawnStone = false;
	private int _dynamiteSpawnCounter = 0;
	[SerializeField] int maxDynamitesCount = 0;
	[SerializeField] float dynamiteSpawnProbability = 50f;
	//[SerializeField] int nextMaxDynamitesCountIncreaseThreshold = 0;

	[SerializeField]
	private GemSprite[] gemSprites;

	private Dictionary<int, Sprite> gemSpritesDic = new Dictionary<int, Sprite>();

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		//copying gemsprites array to dictionary
		foreach (GemSprite gemSprite in gemSprites)
		{
			gemSpritesDic.Add(gemSprite.id, gemSprite.sprite);
		}
		StartCoroutine(WaitForGameManager());
	}

	void SetMaxDynamiteCount(int score)
	{
		maxDynamitesCount = Mathf.FloorToInt(-40f / (score / 10000 + 12f) + 4f);

	}
	IEnumerator WaitForGameManager()
	{
		while (!GameManager.Instance)
		{
			yield return new WaitForEndOfFrame();
		}
		GameManager.Instance._scoreManager.OnMainScoreChange += SetMaxDynamiteCount;
	}
	// Update is called once per frame
	void Update()
	{
		if (_spawnCooldown > Time.time) return;

		_spawnCooldown = Time.time + _gemSpawnRate;
		float randY = UnityEngine.Random.Range(-1, 1);
		Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y - randY, -1.5f);

		if (_spawnDynamites && _dynamiteSpawnCounter < maxDynamitesCount && 100f * UnityEngine.Random.value < dynamiteSpawnProbability)
		{
			_dynamiteSpawnCounter += 1;
			var dynamite = DynamitePool.Instance.GetObjectFromPool();
			dynamite.transform.position = spawnPoint;
			dynamite.OnBombDisabled += Dynamite_OnBombDisabled;
			return;
		}

		float randomStone = UnityEngine.Random.Range(0f, 100f);
		_spawnStone = randomStone >= 100 - _spawnStoneProbability;

		if (_spawnStone && _spawnStones)
		{
			_spawnStone = false;
			var gemConteiner = StonePool.Instance.GetObjectFromPool();
			gemConteiner.transform.position = spawnPoint;
			if (gemConteiner.OnBreak == null) gemConteiner.OnBreak += SpawnGems;
		}
		else
		{
			SpawnGems(1, spawnPoint);
		}
	}

	private void Dynamite_OnBombDisabled(Bomb bomb)
	{
		_dynamiteSpawnCounter = Mathf.Max(0, _dynamiteSpawnCounter - 1);
		if (bomb) bomb.OnBombDisabled -= Dynamite_OnBombDisabled;
	}

	private void SpawnGems(int numberOfGems, Vector3 spawnPoint)
	{
		//randomize gems colors
		Array colorsArray = Enum.GetValues(typeof(ObjectColor));
		for (int i = 0; i < numberOfGems; i++)
		{
			int rand = UnityEngine.Random.Range(1, colorsArray.Length);
			ObjectColor colorType = (ObjectColor)colorsArray.GetValue(rand);

			// get gem from pool and initalize it
			var gem = GemPool.Instance.GetObjectFromPool();
			Sprite spriteType = gemSpritesDic[(int)colorType];
			gem.Init(colorType, spawnPoint, spriteType);
		}

	}
	private void OnDestroy()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance._scoreManager.OnMainScoreChange -= SetMaxDynamiteCount;
		}
	}
}

[Serializable]
public struct GemSprite
{
	public int id;
	public Sprite sprite;
}
