using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 10;
    private float _timeDelayToVibrate = 1f;
    private Vector3 _startScale;
    [SerializeField]
    private float _maxZscale = 1.1f;
    private float _scaleDifference = 0;
    [SerializeField]
    private float _vibrationSpeed = 1f;
    private Vector3 _cutScaleVector;
    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale;
        _scaleDifference = _maxZscale - transform.localScale.z;
        _cutScaleVector = new Vector3(_vibrationSpeed, _vibrationSpeed, _vibrationSpeed);
        float randomTime = Random.Range(0, 1f);
        _timeDelayToVibrate = Time.time + randomTime;
        StartCoroutine(WaitToVibrate());
    }

    // Update is called once per frame
    void Update()
    {
        float newRotationZ = transform.rotation.eulerAngles.z + _rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, newRotationZ));
        if(transform.localScale.z > 1)
        {
            ChangeScaleSize();
        }
        else
        {
            transform.localScale = new Vector3(_startScale.x + _scaleDifference, _startScale.y+ _scaleDifference, _maxZscale);
        }
    }

    private IEnumerator WaitToVibrate()
    {
        while(Time.time < _timeDelayToVibrate)
        {
            yield return null;
        }
        transform.localScale = new Vector3(_startScale.x + _scaleDifference, _startScale.y + _scaleDifference, _maxZscale);
    }

    private void ChangeScaleSize()
    {
        transform.localScale -= _cutScaleVector * Time.deltaTime;
    }
}
