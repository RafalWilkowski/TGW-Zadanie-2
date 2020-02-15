using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conteiner : MonoBehaviour
{
    [SerializeField]
    private GemSpawner.ObjectColor objectColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GemSpawner.ObjectColor GetObjectColor()
    {
        return objectColor;
    }
}
