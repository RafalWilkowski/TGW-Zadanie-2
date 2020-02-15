using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : MonoBehaviour where T:Component
{
    [SerializeField]
    T prefab;

    public static GenericObjectPool<T> Instance { get; private set; }
    protected Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        Instance = this;
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            AddObject(1);
        }
        var objectToGet = objects.Dequeue();
        objectToGet.gameObject.SetActive(true);
        return objectToGet;
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }
    private void AddObject(int counter)
    {
        for (int i = 0; i < counter; i++)
        {
            var newObject = GameObject.Instantiate(prefab);
            newObject.gameObject.SetActive(false);
            objects.Enqueue(newObject);
        }
    }
}
