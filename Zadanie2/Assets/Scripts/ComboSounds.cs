using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSounds : MonoBehaviour
{
    public static ComboSounds Instance;
    [SerializeField]
    private AudioClip[] _comboSounds;
    private AudioSource _audio;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayRandomComboSound()
    {
        int randomClip = Random.Range(0, _comboSounds.Length);
        _audio.PlayOneShot(_comboSounds[randomClip]);

    }
}
