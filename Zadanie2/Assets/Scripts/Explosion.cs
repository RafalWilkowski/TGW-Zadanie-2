using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion: MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
    private AudioSource _audio;
    private bool dead = false;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_audio.isPlaying && !dead)
        {
            dead = true;
            GameManager.Instance.GameOver();
        }
    }
}
