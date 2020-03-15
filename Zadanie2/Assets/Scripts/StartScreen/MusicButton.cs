using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private Image soundIcon;
    [SerializeField]
    private Sprite[] soundIcons;
    private Button _button;

    private bool _music = true;

    private void Awake()
    {
        soundIcon = GetComponent<Image>();
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ToggleMusicIcon);
        _button.onClick.AddListener(AudioManager.Instance.ToggleMusic);
        _button.onClick.AddListener(AudioManager.Instance.Click);

        bool musicMute = (PlayerPrefs.GetInt("musicVolInt") == 0) ? false : true;
        if (musicMute)
        {
            _music = false;
            soundIcon.sprite = soundIcons[1];
        }
        else
        {
            _music = true;
            soundIcon.sprite = soundIcons[0];
        }
    }
    public void ToggleMusicIcon()
    {
        _music = !_music;

        if (_music)
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
