using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    float _fps = 0.0f;
    Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }
    void Update()
    {
        _fps += (Time.unscaledDeltaTime - _fps) * 0.1f;
        int fps = Mathf.RoundToInt( 1.0f / this._fps);      
        _text.text = fps + " fps";
    }

}
