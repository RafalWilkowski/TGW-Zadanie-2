using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fuse : MonoBehaviour
{
    public Action OnExplode;

    private float _timeToExplode = 5f;
    // Start is called before the first frame update
    public void Init(float time)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeToExplode = -Time.deltaTime;
        if (_timeToExplode <= 0) OnExplode?.Invoke();
    }
}
