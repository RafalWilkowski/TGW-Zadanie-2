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
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _currentBeltSpeed = _minBeltSpeed;
        _currentBeltSpeed = Mathf.Clamp(_currentBeltSpeed, _minBeltSpeed, _maxBeltSpeed);
        float tapSizeThresholds = (_maxBeltSpeed - 1f) / _accelThreshold;
        float currentThresholds = (_maxBeltSpeed - _minBeltSpeed) / _accelThreshold;
        TouchDetector.Instance.SetTapSize(tapSizeThresholds, currentThresholds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //add velocity
        Rigidbody2D rb2D = collision.attachedRigidbody;
        if (rb2D)
        {
            rb2D.velocity = new Vector2(_currentBeltSpeed, 0);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //add velocity
        Rigidbody2D rb2D = collision.attachedRigidbody;
        if (rb2D && rb2D.velocity.magnitude == 0)
        {
            rb2D.velocity = new Vector2(_currentBeltSpeed, 0);
        }
    }

    public void CheckPointsThreshold(int points)
    {
        if (points / (_pointsThreshold * (_threshold + 1)) >= 1)
        {
            _threshold++;
            ChangeBeltSpeed();
            float tapSizeThresholds = (4.5f - _minBeltSpeed) / _accelThreshold;
            TouchDetector.Instance.IncreaseTapSize(tapSizeThresholds);
            //change a velocity of all object on belt
            List<Collider2D> colliders = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.layerMask = gameObject.layer;
            _boxCollider2D.OverlapCollider(filter, colliders);
            foreach(Collider2D collider in colliders)
            {
                Rigidbody2D rb2D = collider.attachedRigidbody;
                if (rb2D)
                {
                    rb2D.velocity = new Vector3(_currentBeltSpeed, 0);
                }
            }
        }
        
    }

    private void ChangeBeltSpeed()
    {
        _currentBeltSpeed += _accelThreshold;
        _currentBeltSpeed = Mathf.Clamp(_currentBeltSpeed, _minBeltSpeed, _maxBeltSpeed);
    }
}
