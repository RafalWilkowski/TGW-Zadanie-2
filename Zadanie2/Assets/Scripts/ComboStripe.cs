using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ComboStripe : MonoBehaviour
{
    public static Action OnTimeout;

    [SerializeField]
    private float _firstTimeToAdd;
    [SerializeField]
    private float _nextTimetoAdd;
    
    private Slider _slider;
    private bool _timeRunning = false;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeRunning)
        {
            if (_slider.value > 0)
            {
                _slider.value -= Time.deltaTime;
            }
            else
            {
                _timeRunning = false;
                OnTimeout?.Invoke();
            }
        }       
    }

    public void UpdateTime(bool reset)
    {
        if (!_timeRunning || reset)
        {
            _slider.value = 2;
        }
        else
        {
            _slider.value += 1;
        }
        _timeRunning = true;
    }
}
