using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemSpawner : MonoBehaviour
{
	public static GemSpawner Instance;

    [SerializeField]
    private Transform _minY;
    private float _minYf;
    [SerializeField]
    private Transform _maxY;
    private float _maxYf;
    [SerializeField]
	private bool _spawnStones = true;
	[SerializeField]
	private bool _spawnDynamites = true;
    [Header("SpawnRate Variables")]
	[SerializeField]
	private float _gemStartSpawnRate = 1f;
    private float _currentGemSpawnRate;
    [SerializeField]
    private float _pointsThreshold = 1000;
    private int _threshold = 0;
    private float _spawnCooldown = 0;
    [SerializeField]
    private float _accelThreshold = 0.01f;

    [SerializeField]
	private float _spawnStoneProbability = 5f;
	private bool _spawnStone = false;
	[Header("Dynamite spawner")]
	private int _dynamiteSpawnCounter = 0;
	[SerializeField] int maxDynamitesCount = 0;
	[SerializeField] float dynamiteSpawnProbability = 33f;
	[Header("Dynamite spawning progression")]
	[SerializeField] int maxDynamitesAsymptote = 4;
	[SerializeField] int dynamitesCountScoreScaling = 10000;
	[SerializeField] float dynamitesCountPaceScaling = 40f;
	[SerializeField] float dynamitesCountXTranslation = 12f;
	
	[SerializeField]
	private GemSprite[] gemSprites;

	private Dictionary<int, Sprite> gemSpritesDic = new Dictionary<int, Sprite>();

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
        _minYf = _minY.position.y;
        _maxYf = _maxY.position.y;
		//copying gemsprites array to dictionary
		foreach (GemSprite gemSprite in gemSprites)
		{
			gemSpritesDic.Add(gemSprite.id, gemSprite.sprite);
		}
		StartCoroutine(WaitForGameManager());
        _currentGemSpawnRate = _gemStartSpawnRate;

    }

	void SetMaxDynamiteCount(int score)
	{
		maxDynamitesCount = Mathf.FloorToInt(-dynamitesCountPaceScaling / (score / dynamitesCountScoreScaling + dynamitesCountXTranslation) + maxDynamitesAsymptote);

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

		_spawnCooldown = Time.time + 1f/_currentGemSpawnRate;
		float randY = UnityEngine.Random.Range(_minYf, _maxYf);
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

    public void CheckPointsThreshold(int points)
    {
        int correctThreshold = Mathf.FloorToInt(points / (_pointsThreshold));
        if (correctThreshold >= 1 && correctThreshold != _threshold)
        {
            _threshold = correctThreshold;
            _currentGemSpawnRate = _gemStartSpawnRate + _accelThreshold * correctThreshold;
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
