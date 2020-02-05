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
            Debug.Log("More than one AudioManager!!!!");
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
