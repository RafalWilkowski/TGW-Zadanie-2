using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]
    private AudioMixer masterMixer;
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

    public void ToggleSound()
    {
        float masterVolume = 0f;
        masterMixer.GetFloat("masterVol", out masterVolume);

        if(masterVolume == -80)
        {
            masterMixer.SetFloat("masterVol", 0f);
        }
        else
        {
            masterMixer.SetFloat("masterVol", -80f);
        }
    }

    public void Click()
    {
        clickSound.Play();
    }
}
