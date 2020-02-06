using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objectsToSpawn;
    [SerializeField]
    private Transform _maxY;
    [SerializeField]
    private Transform _minY;
    [SerializeField]
    private float _spawnRateMin;
    [SerializeField]
    private float _spawnRateMax;
    private float _spawnCooldown = 0;

    void Update()
    {
        if(_spawnCooldown < Time.time)
        {
            float spawnRate = Random.Range(_spawnRateMin, _spawnRateMax);
            _spawnCooldown = Time.time + spawnRate;
            int random = Random.Range(0, _objectsToSpawn.Length);
            float randomY = Random.Range(_minY.position.y, _maxY.position.y);
            Vector3 spawnPosition = new Vector3(transform.position.x, randomY, -1);
            Instantiate(_objectsToSpawn[random], spawnPosition, Quaternion.identity);
        }
    }
}
