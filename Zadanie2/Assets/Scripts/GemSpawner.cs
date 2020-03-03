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

    private int _specialPackage = 0;
    private int _dynamiteSpawnCounter = 0;

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
        foreach(GemSprite gemSprite in gemSprites)
        {
            gemSpritesDic.Add(gemSprite.id, gemSprite.sprite);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(_spawnCooldown <= Time.time)
        {
            _spawnCooldown = Time.time + _gemSpawnRate;
            _specialPackage += UnityEngine.Random.Range(20, 50);
            _dynamiteSpawnCounter += UnityEngine.Random.Range(10, 40);
            float randY = UnityEngine.Random.Range(-1, 1);
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y - randY, -1.5f);

            if (_specialPackage > 100 && _spawnStones)
            {
              _specialPackage = 0;             
              var gemConteiner = StonePool.Instance.GetObjectFromPool();
              gemConteiner.transform.position = spawnPoint;
              if(gemConteiner.OnBreak == null) gemConteiner.OnBreak += SpawnGems;
            }
            else if(_dynamiteSpawnCounter> 100 && _spawnDynamites)
            {
              _dynamiteSpawnCounter = 0;
              var dynamite = DynamitePool.Instance.GetObjectFromPool();
              dynamite.transform.position = spawnPoint;
            }
            else
            {
              SpawnGems(1, spawnPoint);
            }
        }
    }

    private void SpawnGems(int numberOfGems, Vector3 spawnPoint)
    {
        //randomize gems colors
        Array colorsArray = Enum.GetValues(typeof(ObjectColor));    
        for(int i = 0; i < numberOfGems; i++)
        {
            int rand = UnityEngine.Random.Range(1, colorsArray.Length);
            ObjectColor colorType = (ObjectColor)colorsArray.GetValue(rand);

            // get gem from pool and initalize it
            var gem = GemPool.Instance.GetObjectFromPool();
            Sprite spriteType = gemSpritesDic[(int)colorType];
            gem.Init(colorType, spawnPoint, spriteType);
        }
        
    }
}

[Serializable]
public struct GemSprite
{
    public int id;
    public Sprite sprite;
}
