using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GemSpawner : MonoBehaviour
{
    public static GemSpawner Instance;

    [SerializeField]
    private float _gemSpawnRate = 1f;
    private float _spawnCooldown = 0;

    [SerializeField]
    private float[] _wagiPrawdopodobienstwa;
    private float[] _rozKumultatywny;
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
        // preparing gem container (breakable stone) odds
        _rozKumultatywny = new float[_wagiPrawdopodobienstwa.Length];
        StoneBreakOdds();
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
            float randY = UnityEngine.Random.Range(-1, 1);
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y - randY, -1.5f);

            if (_specialPackage > 100)
            {
              _specialPackage = 0;             
              var gemConteiner = StonePool.Instance.GetObjectFromPool();
              gemConteiner.transform.position = spawnPoint;
              if(gemConteiner.OnBreak == null) gemConteiner.OnBreak += StoneBreak;
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
        int quantity = 0;
        float randomFloat = UnityEngine.Random.Range(0f, 1f);

        for(int i = 0; i < _rozKumultatywny.Length; i++)
        {
            float compareFloat = _rozKumultatywny[i];
            if (randomFloat < compareFloat)
            {
                quantity = i;                
                break;
            }               
        }
        
        Debug.Log(randomFloat + " : : " + quantity);
        
        for (int i = 0; i < quantity; i++)
        {
            SpawnGem(position);
        }
    }

    public void StoneBreakOdds()
    {
        float totalWeight = _wagiPrawdopodobienstwa.Sum();
        List<float> rozkladPrawd = new List<float>();
        int lenght = _wagiPrawdopodobienstwa.Length;

        // rozkład pradwopodobienstwa
        for (int i = 0; i < lenght; i++)
        {
            float someValue = _wagiPrawdopodobienstwa[i] / totalWeight;
            rozkladPrawd.Add(someValue);
        }
        // rozklad kumultatywny
        for(int i = lenght - 1 ; i >= 0; --i)
        {           
            float suma = rozkladPrawd.Sum();
            _rozKumultatywny[i] = suma;
            rozkladPrawd.RemoveAt(rozkladPrawd.Count - 1);                     
        }
    }
}

[Serializable]
public struct GemSprite
{
    public int id;
    public Sprite sprite;
}
