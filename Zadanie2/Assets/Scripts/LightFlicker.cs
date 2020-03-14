using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightFlicker : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField]
    private float _minIntensity = 0f;
    [SerializeField]
    private float _maxIntensity = 1f;
    [SerializeField]
    private float _flickeringSpeed = 1f;

    private void OnEnable()
    {
        StartCoroutine(Flick());
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Flick()
    {
        while (gameObject.activeSelf)
        {
            float random = Random.Range(_minIntensity, _maxIntensity);
            _sprite.color = new Color(random, random, random);
            yield return new WaitForSeconds(_flickeringSpeed);
        }
        
    }
}
