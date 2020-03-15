﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]
    private AudioMixer _masterMixer;
    [SerializeField]
    private AudioSource clickSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
            Debug.Log("AudioManager zniszczony");
        }
        
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("musicVol"))
        {
            PlayerPrefs.SetFloat("musicVol", 0.0f);
            PlayerPrefs.SetFloat("sfxVol", 0.0f);
            PlayerPrefs.SetInt("musicVolInt", 0);
            PlayerPrefs.SetInt("sfxVolInt", 0);
        }
        else
        {
            bool musicMute = (PlayerPrefs.GetInt("musicVolInt") == 0) ? false : true;
            if (musicMute)
            {
                _masterMixer.SetFloat("musicVol", -80.0f);
            }
            else
            {
                _masterMixer.SetFloat("musicVol", 0f);
            }

            bool sfxMute = (PlayerPrefs.GetInt("sfxVolInt") == 0) ? false : true;
            if (sfxMute)
            {
                _masterMixer.SetFloat("sfxVol", -80.0f);
            }
            else
            {
                _masterMixer.SetFloat("sfxVol", 0f);
            }
        }
        
    }

    public void ToggleMusic()
    {
        float musicVolume = 0f;
        _masterMixer.GetFloat("musicVol", out musicVolume);

        if(musicVolume == -80)
        {
            _masterMixer.SetFloat("musicVol", 0f);
            PlayerPrefs.SetInt("musicVolInt", 0);
        }
        else
        {
            _masterMixer.SetFloat("musicVol", -80f);
            PlayerPrefs.SetInt("musicVolInt", 1);
        }
    }
    public void ToggleSFX()
    {
        float sfxVolume = 0f;
        _masterMixer.GetFloat("sfxVol", out sfxVolume);

        if (sfxVolume == -80)
        {
            _masterMixer.SetFloat("sfxVol", 0f);
            PlayerPrefs.SetInt("sfxVolInt", 0);
        }
        else
        {
            _masterMixer.SetFloat("sfxVol", -80f);
            PlayerPrefs.SetInt("sfxVolInt", 1);
        }
    }
    public void Click()
    {
        clickSound.Play();
    }
}
