using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    private float _currentBeltSpeed = 0;
    [SerializeField]
    private float _minBeltSpeed = 2f;
    [SerializeField]
    private float _maxBeltSpeed = 6f;
    [SerializeField]
    private float _accelThreshold = 0.02f;
    [SerializeField]
    private int _pointsThreshold = 20000;
    private int _threshold = 0;

    private void Start()
    {
        _currentBeltSpeed = _minBeltSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Rigidbody2D rb2D = collision.attachedRigidbody;
        if (rb2D)
        {
            rb2D.velocity = new Vector3(_currentBeltSpeed, 0);
        }
    }

    public void CheckPointsThreshold(int points)
    {
        if (points / (_pointsThreshold * (_threshold + 1)) >= 1)
        {
            _threshold++;
            //Add function changing speed of all object on belt
            ChangeBeltSpeed();
            Debug.Log(_threshold);
        }
        
    }

    private void ChangeBeltSpeed()
    {
        _currentBeltSpeed += _accelThreshold;
    }
}
