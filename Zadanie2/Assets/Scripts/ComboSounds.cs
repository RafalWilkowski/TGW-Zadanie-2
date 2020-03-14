using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSounds : MonoBehaviour
{
    public static ComboSounds Instance;
    [SerializeField]
    private AudioClip[] _comboSounds;
    private int _randomClip = 0;
    private int _lastComboClip = 7;
    private AudioSource _audio;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _lastComboClip = 7;
    }

    public void PlayRandomComboSound()
    {
        while(true)
        {
            _randomClip = Random.Range(0, _comboSounds.Length);
            if(_randomClip != _lastComboClip)
            {
                _lastComboClip = _randomClip;
                break;
            }
        } 
        _audio.PlayOneShot(_comboSounds[_randomClip]);

    }
}
