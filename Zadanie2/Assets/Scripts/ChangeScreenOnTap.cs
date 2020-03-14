using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScreenOnTap : MonoBehaviour
{
    [SerializeField]
    private float _tapInactiveTime = 2f;
    private float _tapCooldown = 0;
    private Image _ownImage;
    private int _currentScreen = 0;
    [SerializeField]
    private Sprite[] _screens;

    private Animator _anim;

    private void Start()
    {
        _tapCooldown = Time.time + _tapInactiveTime;
        _anim = GetComponent<Animator>();
        _ownImage = GetComponent<Image>();
    }
    private void Update()
    {
        
        if(Input.touchCount > 0)
        {
            if(Time.time > _tapCooldown)
            {
                _tapCooldown = Time.time + _tapInactiveTime;
                _anim.SetTrigger("ChangeScreen");
            }
        }
    }

    public void ChangeScreen()
    {
        if (_currentScreen < _screens.Length)
        {
            _ownImage.sprite = _screens[_currentScreen];
            _currentScreen++;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
