﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SecondaryScoreText : MonoBehaviour
{
    private int _pointsCarried = 0;
    private Text _ownText;
    private Animator _anim;

    private void OnEnable()
    {
        _anim.Play("secondaryscore_hidden");
        _pointsCarried = 0;
    }
    public void OnGlideEnded()
    {
        GameManager.Instance._scoreManager.AddMainScore(_pointsCarried);
        _pointsCarried = 0;
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        _ownText = GetComponent<Text>();
        _anim = GetComponent<Animator>();
    }
    public void MissionComleted(int points)
    {
        _pointsCarried = points;
        _ownText.text = points.ToString();
        _anim.SetTrigger("Scored");
    }

    
}
