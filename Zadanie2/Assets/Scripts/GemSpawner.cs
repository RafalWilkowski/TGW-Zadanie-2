using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemSpawner : MonoBehaviour
{
    public static GemSpawner Instance;

    [SerializeField]
    private float _trashSpawnRate = 1f;
    private float _spawnCooldown = 0;

    private int _specialPackage = 0;

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
            _spawnCooldown = Time.time + _trashSpawnRate;
            _specialPackage += UnityEngine.Random.Range(0, 20);
            float randY = UnityEngine.Random.Range(-1, 1);
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y - randY, -1.5f);

            if (_specialPackage > 100)
            {
                _specialPackage = 0;             
                var gemConteiner = StonePool.Instance.GetObjectFromPool();
                gemConteiner.transform.position = spawnPoint;
            }
            else
            {
                SpawnGem(spawnPoint);
            }
        }
    }

    private void SpawnGem(Vector3 spawnPoint)
    {
        //randomize gems colors
        Array colorsArray = Enum.GetValues(typeof(ObjectColor));       
        int rand = UnityEngine.Random.Range(1, colorsArray.Length);
        ObjectColor colorType = (ObjectColor)colorsArray.GetValue(rand);
        
        // get gem from pool and initalize it
        var gem = GemPool.Instance.GetObjectFromPool();
        Sprite spriteType = gemSpritesDic[(int)colorType];
        gem.Init(colorType, spawnPoint,spriteType);
    }
    public void StoneBreak(Vector3 position)
    {
        /*int quantity = UnityEngine.Random.Range(0, _prefabs.Length + 1);
        print("Ilość:" + quantity.ToString());
        for (int i = 0; i < quantity; i++)
        {
            int rand = UnityEngine.Random.Range(0, _prefabs.Length-2);
            Instantiate(_prefabs[rand], position, Quaternion.identity);
        }*/
    }
}

[Serializable]
public struct GemSprite
{
    public int id;
    public Sprite sprite;
}
