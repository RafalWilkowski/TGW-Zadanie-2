using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conteiner : MonoBehaviour
{
    [SerializeField]
    private TrashSpawner.ObjectColor objectColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TrashSpawner.ObjectColor GetObjectColor()
    {
        return objectColor;
    }
}
