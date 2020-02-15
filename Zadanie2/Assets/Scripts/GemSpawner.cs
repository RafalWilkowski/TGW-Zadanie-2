using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public static GemSpawner Instance;
    public enum ObjectColor { RED, YELLOW,GREEN,BLUE}


    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private float trashSpawnRate = 1f;
    private float spawnCooldown = 0;

    private int specialPackage = 0;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(spawnCooldown <= Time.time)
        {
            spawnCooldown = Time.time + trashSpawnRate;
            specialPackage += Random.Range(20, 40);
            float randY = Random.Range(-1, 1);
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y - randY, -1.5f);

            if (specialPackage > 100)
            {
                specialPackage = 0;
                Instantiate(prefabs[prefabs.Length - 1], spawnPoint, Quaternion.identity);
            }
            else
            {
                int rand = Random.Range(0, prefabs.Length - 1);
                Instantiate(prefabs[rand], spawnPoint, Quaternion.identity);
            }
        }
    }

    public void Boom(Vector3 position)
    {
        int quantity = Random.Range(0, prefabs.Length + 1);
        print("Ilość:" + quantity.ToString());
        for (int i = 0; i < quantity; i++)
        {
            int rand = Random.Range(0, prefabs.Length-2);
            Instantiate(prefabs[rand], position, Quaternion.identity);
        }
    }
}
