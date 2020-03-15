using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXButton : MonoBehaviour
{
    private Image soundIcon;
    [SerializeField]
    private Sprite[] soundIcons;
    private Button _button;

    private bool _sfx = true;

    private void Awake()
    {
        soundIcon = GetComponent<Image>();
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ToggleMusicIcon);
        _button.onClick.AddListener(AudioManager.Instance.ToggleSFX);
        _button.onClick.AddListener(AudioManager.Instance.Click);

        bool sfxMute = (PlayerPrefs.GetInt("sfxVolInt") == 0) ? false : true;
        if (sfxMute)
        {
            _sfx = false;
            soundIcon.sprite = soundIcons[1];
        }
        else
        {
            _sfx = true;
            soundIcon.sprite = soundIcons[0];
        }
    }
    public void ToggleMusicIcon()
    {
        _sfx = !_sfx;

        if (_sfx)
        {
            soundIcon.sprite = soundIcons[0];
        }
        else
        {
            soundIcon.sprite = soundIcons[1];
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
