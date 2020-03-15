using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private Image soundIcon;
    [SerializeField]
    private Sprite[] soundIcons;

    private bool sound = true;

    private void Awake()
    {
        soundIcon = GetComponent<Image>();
    }

    public void ToggleMusicIcon()
    {
        sound = !sound;

        if (sound)
        {
            soundIcon.sprite = soundIcons[0];
        }
        else
        {
            soundIcon.sprite = soundIcons[1];
        }
    }
}
